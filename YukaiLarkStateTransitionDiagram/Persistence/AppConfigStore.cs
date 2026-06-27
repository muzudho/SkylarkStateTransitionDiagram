namespace YukaiLarkStateTransitionDiagram.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Xna.Framework;

/// <summary>
/// アプリケーション設定を永続化するためのストアです。
/// </summary>
public sealed class AppConfig
{
    public const int MaxRecentFiles = 10;
    public const string DefaultThemeName = "YukaiLark";

    public string? LastOpenedFile { get; set; }
    public string SelectedThemeName { get; set; } = DefaultThemeName;
    public List<string> RecentFiles { get; set; } = new();
    public Dictionary<string, List<string>> NormalNodePaletteOverrides { get; set; } = new();

    public void AddRecentFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var fullPath = Path.GetFullPath(path);
        LastOpenedFile = fullPath;
        RecentFiles = (RecentFiles ?? new List<string>())
            .Where(file => !string.Equals(file, fullPath, StringComparison.OrdinalIgnoreCase))
            .Prepend(fullPath)
            .Where(File.Exists)
            .Take(MaxRecentFiles)
            .ToList();
    }

    public Color[]? GetNormalNodePaletteOverride(string themeName)
    {
        if (NormalNodePaletteOverrides is null
            || !NormalNodePaletteOverrides.TryGetValue(themeName, out var entries)
            || entries.Count != Theme.BoardTheme.NormalNodePaletteLength)
        {
            return null;
        }

        var colors = new Color[Theme.BoardTheme.NormalNodePaletteLength];
        for (var i = 0; i < colors.Length; i++)
        {
            if (!TryParseColor(entries[i], out colors[i]))
            {
                return null;
            }
        }

        return colors;
    }

    public void SetNormalNodePaletteOverride(string themeName, IReadOnlyList<Color> colors)
    {
        if (string.IsNullOrWhiteSpace(themeName) || colors.Count != Theme.BoardTheme.NormalNodePaletteLength)
        {
            return;
        }

        NormalNodePaletteOverrides ??= new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        NormalNodePaletteOverrides[themeName] = colors.Select(FormatColor).ToList();
    }

    private static string FormatColor(Color color)
        => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

    internal static bool TryParseColor(string text, out Color color)
    {
        color = default;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var hex = text.Trim();
        if (hex.StartsWith('#'))
        {
            hex = hex[1..];
        }

        if (hex.Length != 6
            || !byte.TryParse(hex[..2], System.Globalization.NumberStyles.HexNumber, null, out var r)
            || !byte.TryParse(hex[2..4], System.Globalization.NumberStyles.HexNumber, null, out var g)
            || !byte.TryParse(hex[4..6], System.Globalization.NumberStyles.HexNumber, null, out var b))
        {
            return false;
        }

        color = new Color(r, g, b);
        return true;
    }
}

public static class AppConfigStore
{
    private static readonly UTF8Encoding Utf8NoBom = new(false);
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    private static string ConfigDirectory
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YukaiLarkStateTransitionDiagram");

    private static string ConfigPath => Path.Combine(ConfigDirectory, "config.json");

    public static AppConfig Load()
    {
        try
        {
            if (!File.Exists(ConfigPath))
            {
                return new AppConfig();
            }

            var json = File.ReadAllText(ConfigPath, Encoding.UTF8);
            var config = JsonSerializer.Deserialize<AppConfig>(json, Options) ?? new AppConfig();
            if (string.IsNullOrWhiteSpace(config.SelectedThemeName))
            {
                config.SelectedThemeName = AppConfig.DefaultThemeName;
            }
            config.RecentFiles = (config.RecentFiles ?? new List<string>())
                .Where(file => !string.IsNullOrWhiteSpace(file))
                .Select(Path.GetFullPath)
                .Where(File.Exists)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(AppConfig.MaxRecentFiles)
                .ToList();
            config.NormalNodePaletteOverrides = (config.NormalNodePaletteOverrides ?? new Dictionary<string, List<string>>())
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Key)
                    && pair.Value is { Count: Theme.BoardTheme.NormalNodePaletteLength }
                    && pair.Value.All(entry => AppConfig.TryParseColor(entry, out _)))
                .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
            if (config.LastOpenedFile is not null && !File.Exists(config.LastOpenedFile))
            {
                config.LastOpenedFile = config.RecentFiles.FirstOrDefault();
            }
            return config;
        }
        catch
        {
            return new AppConfig();
        }
    }

    public static void Save(AppConfig config)
    {
        try
        {
            Directory.CreateDirectory(ConfigDirectory);
            var json = JsonSerializer.Serialize(config, Options);
            File.WriteAllText(ConfigPath, json, Utf8NoBom);
        }
        catch
        {
            // 設定保存に失敗しても、図の保存・読込操作は成功扱いにする。
        }
    }
}

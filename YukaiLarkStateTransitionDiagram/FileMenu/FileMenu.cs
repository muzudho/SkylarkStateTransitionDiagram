namespace YukaiLarkStateTransitionDiagram;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YukaiLarkStateTransitionDiagram.Persistence;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public partial class Game1
{
    private const int RecentFileMenuMaxItems = AppConfig.MaxRecentFiles;

    private bool _isFileMenuOpen;
    private bool _isStartupFileMenu;

    private void OpenFileMenu(bool isStartup)
    {
        _isFileMenuOpen = true;
        _isStartupFileMenu = isStartup;
        _status = "ユカイラーク: 最近保存したファイルを開く？ Nで新規、Oで読込、数字で最近ファイル。";
    }

    private void CloseFileMenu()
    {
        _isFileMenuOpen = false;
        _isStartupFileMenu = false;
    }

    private void HandleFileMenuKeyboard(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.N))
        {
            CloseFileMenu();
            CreateNewDiagram();
            return;
        }

        if (IsNewKeyPress(keyboard, Keys.O))
        {
            CloseFileMenu();
            LoadDiagramFromDialog();
            return;
        }

        if (IsNewKeyPress(keyboard, Keys.Escape))
        {
            var wasStartup = _isStartupFileMenu;
            CloseFileMenu();
            if (wasStartup && _currentFilePath is null && _nodes.Count == 0 && _transitions.Count == 0)
            {
                _status = "空の状態遷移図を開きました。Ctrl+Nで開始・終了マーク付きの新規作成もできます。";
            }
            else
            {
                _status = "最近ファイルメニューを閉じました。";
            }
            return;
        }

        if (TryGetRecentFileShortcutIndex(keyboard, out var recentIndex))
        {
            LoadRecentFileByIndex(recentIndex);
        }
    }

    private void HandleFileMenuMouse(MouseState mouse)
    {
        if (mouse.LeftButton != ButtonState.Pressed || _previousMouse.LeftButton != ButtonState.Released)
        {
            return;
        }

        var point = mouse.Position;
        var (newButton, openButton) = GetFileMenuActionRectangles();
        if (newButton.Contains(point))
        {
            CloseFileMenu();
            CreateNewDiagram();
            return;
        }

        if (openButton.Contains(point))
        {
            CloseFileMenu();
            LoadDiagramFromDialog();
            return;
        }

        var recentFiles = GetRecentFiles();
        for (var i = 0; i < recentFiles.Count; i++)
        {
            if (GetRecentFileMenuItemRectangle(i).Contains(point))
            {
                LoadRecentFileByIndex(i);
                return;
            }
        }
    }

    private void LoadRecentFileByIndex(int index)
    {
        var recentFiles = GetRecentFiles();
        if (index < 0 || index >= recentFiles.Count)
        {
            return;
        }

        var path = recentFiles[index];
        if (!File.Exists(path))
        {
            _appConfig.RecentFiles.RemoveAll(file => string.Equals(file, path, StringComparison.OrdinalIgnoreCase));
            AppConfigStore.Save(_appConfig);
            _status = $"{Path.GetFileName(path)} が見つからないため、最近ファイルから外しました。";
            return;
        }

        CloseFileMenu();
        LoadDiagramFromPath(path);
    }

    private bool TryGetRecentFileShortcutIndex(KeyboardState keyboard, out int index)
    {
        for (var i = 0; i < RecentFileMenuMaxItems; i++)
        {
            var key = i == 9 ? Keys.D0 : Keys.D1 + i;
            var numPadKey = i == 9 ? Keys.NumPad0 : Keys.NumPad1 + i;
            if (IsNewKeyPress(keyboard, key) || IsNewKeyPress(keyboard, numPadKey))
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

    private List<string> GetRecentFiles()
        => (_appConfig.RecentFiles ?? new List<string>())
            .Where(file => !string.IsNullOrWhiteSpace(file) && File.Exists(file))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(RecentFileMenuMaxItems)
            .ToList();

    private void DrawFileMenuOverlay()
    {
        if (!_isFileMenuOpen)
        {
            return;
        }

        var viewport = GraphicsDevice.Viewport;
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(18, 26, 28, 150));

        var panel = GetFileMenuPanelRectangle();
        _spriteBatch.Draw(_pixel, new Rectangle(panel.X + 6, panel.Y + 8, panel.Width, panel.Height), new Color(16, 22, 24, 105));
        _spriteBatch.Draw(_pixel, panel, new Color(255, 253, 239, 245));
        DrawScreenRectangleOutline(panel, new Color(83, 178, 176), 2);

        if (_yukaiLarkMascotTexture is not null)
        {
            var mascotSize = 88;
            var mascot = new Rectangle(panel.X + 24, panel.Y + 24, mascotSize, mascotSize);
            _spriteBatch.Draw(_yukaiLarkMascotTexture, mascot, Color.White);
        }

        var textX = panel.X + 130;
        var textY = panel.Y + 24;
        var prompt = _isStartupFileMenu ? "最近保存したファイルを開く？" : "最近保存したファイル";
        DrawUiText("ユカイラーク", new Vector2(textX, textY), new Color(51, 84, 102), 18, true);
        DrawUiText(prompt, new Vector2(textX, textY + 28), new Color(38, 55, 62), 24, true);
        DrawUiText("新しく始めるか、ファイルを読込むこともできます。", new Vector2(textX, textY + 64), new Color(88, 105, 112), 15, false);

        var (newButton, openButton) = GetFileMenuActionRectangles();
        DrawFileMenuButton(newButton, "N", "新規作成", "空の状態遷移図で始める", enabled: true);
        DrawFileMenuButton(openButton, "O", "読込", "JSONファイルを選ぶ", enabled: true);

        var recentFiles = GetRecentFiles();
        var recentTitleY = GetRecentFileMenuItemRectangle(0).Y - 28;
        DrawUiText("最近保存したファイル", new Vector2(panel.X + 24, recentTitleY), new Color(51, 84, 102), 16, true);
        if (recentFiles.Count == 0)
        {
            DrawUiText("まだ最近ファイルはありません。", new Vector2(panel.X + 24, recentTitleY + 34), new Color(110, 126, 132), 15, false);
            return;
        }

        for (var i = 0; i < recentFiles.Count; i++)
        {
            DrawRecentFileMenuItem(i, recentFiles[i]);
        }
    }

    private void DrawFileMenuButton(Rectangle bounds, string key, string title, string description, bool enabled)
    {
        var fill = enabled ? new Color(238, 250, 239) : new Color(230, 232, 232);
        var edge = enabled ? new Color(83, 178, 176) : new Color(160, 166, 168);
        _spriteBatch.Draw(_pixel, bounds, fill);
        DrawScreenRectangleOutline(bounds, edge, 2);
        DrawUiText(key, new Vector2(bounds.X + 14, bounds.Y + 11), new Color(51, 84, 102), 18, true);
        DrawUiText(title, new Vector2(bounds.X + 48, bounds.Y + 9), new Color(38, 55, 62), 17, true);
        DrawUiText(description, new Vector2(bounds.X + 48, bounds.Y + 34), new Color(88, 105, 112), 13, false);
    }

    private void DrawRecentFileMenuItem(int index, string path)
    {
        var bounds = GetRecentFileMenuItemRectangle(index);
        _spriteBatch.Draw(_pixel, bounds, index % 2 == 0 ? new Color(250, 252, 246, 245) : new Color(238, 250, 239, 245));
        DrawScreenRectangleOutline(bounds, new Color(178, 219, 203), 1);

        var shortcut = index == 9 ? "0" : (index + 1).ToString();
        DrawUiText(shortcut, new Vector2(bounds.X + 12, bounds.Y + 11), new Color(51, 84, 102), 16, true);
        DrawUiText(GetRecentFileDisplayText(path), new Vector2(bounds.X + 44, bounds.Y + 3), new Color(38, 55, 62), 15, true);
        DrawUiText(Path.GetDirectoryName(path) ?? string.Empty, new Vector2(bounds.X + 44, bounds.Y + 24), new Color(98, 116, 122), 12, false);
    }

    private (Rectangle NewButton, Rectangle OpenButton) GetFileMenuActionRectangles()
    {
        var panel = GetFileMenuPanelRectangle();
        var buttonWidth = Math.Max(220, (panel.Width - 60) / 2);
        var y = panel.Y + 116;
        return (
            new Rectangle(panel.X + 24, y, buttonWidth, 62),
            new Rectangle(panel.X + panel.Width - 24 - buttonWidth, y, buttonWidth, 62));
    }

    private Rectangle GetRecentFileMenuItemRectangle(int index)
    {
        var panel = GetFileMenuPanelRectangle();
        return new Rectangle(panel.X + 24, panel.Y + 224 + index * 44, panel.Width - 48, 40);
    }

    private Rectangle GetFileMenuPanelRectangle()
    {
        var viewport = GraphicsDevice.Viewport;
        var recentCount = Math.Max(1, GetRecentFiles().Count);
        var width = Math.Clamp(viewport.Width - 64, 560, 780);
        var height = Math.Min(viewport.Height - 64, 248 + recentCount * 44);
        var x = (viewport.Width - width) / 2;
        var y = Math.Max(24, (viewport.Height - height) / 2);
        return new Rectangle(x, y, width, height);
    }

    private static string GetRecentFileDisplayText(string path)
    {
        var fileName = Path.GetFileName(path);
        if (fileName.Length <= 54)
        {
            return fileName;
        }

        return fileName[..51] + "...";
    }
}
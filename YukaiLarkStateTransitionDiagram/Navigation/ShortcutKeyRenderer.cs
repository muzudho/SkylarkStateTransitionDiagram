namespace YukaiLarkStateTransitionDiagram.Navigation;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YukaiLarkStateTransitionDiagram.Theme;

/// <summary>
/// ショートカットキーとその名前の描画
/// </summary>
public sealed class ShortcutKeyRenderer : IDisposable
{
    private static readonly TimeSpan HelpPageDuration = TimeSpan.FromSeconds(5);
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _pixel;
    private readonly Dictionary<string, Texture2D> _uiTextTextureCache = new();
    private IKeyCapTheme _keyCapTheme;

    private readonly record struct HelpHint(string Key, string Description);
    private readonly record struct HelpPage(HelpHint[] Hints);

    public ShortcutKeyRenderer(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Texture2D pixel, IKeyCapTheme keyCapTheme)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _pixel = pixel;
        _keyCapTheme = keyCapTheme;
    }

    public IKeyCapTheme KeyCapTheme
    {
        get => _keyCapTheme;
        set => _keyCapTheme = value;
    }

    public void Dispose()
    {
        foreach (var texture in _uiTextTextureCache.Values)
        {
            texture.Dispose();
        }

        _uiTextTextureCache.Clear();
    }

    /// <summary>
    /// ショートカットキーの描画
    /// </summary>
    /// <param name="viewport">描画領域のビューポート</param>
    /// <param name="totalGameTime">ゲーム開始からの経過時間</param>
    /// <param name="isEditingLabel">ラベル編集中かどうか</param>
    /// <param name="isExportSelecting">エクスポート選択中かどうか</param>
    /// <param name="selectedNode">選択されているノード</param>
    /// <param name="selectedTransition">選択されている遷移</param>
    public void DrawBottomHelp(
        Viewport viewport,
        TimeSpan totalGameTime,
        bool isEditingLabel,
        bool isExportSelecting,
        DiagramNode? selectedNode,
        DiagramTransition? selectedTransition)
    {
        if (viewport.Height < 360)
        {
            return;
        }

        var y = viewport.Height - 34;
        _spriteBatch.Draw(_pixel, new Rectangle(0, y, viewport.Width, 34), new Color(17, 19, 23, 210));

        var pages = GetHelpPages(isEditingLabel, isExportSelecting, selectedNode, selectedTransition);
        if (pages.Length == 0)
        {
            return;
        }

        var pageIndex = (int)(totalGameTime.TotalSeconds / HelpPageDuration.TotalSeconds) % pages.Length;
        var page = pages[pageIndex];
        var position = new Vector2(12, y + 6);
        for (var i = 0; i < page.Hints.Length; i++)
        {
            var hint = page.Hints[i];
            position = DrawShortcutHint(position, hint.Key, hint.Description);
            if (i < page.Hints.Length - 1)
            {
                position = DrawHelpSeparator(position);
            }
        }
    }

    private static HelpPage[] GetHelpPages(
        bool isEditingLabel,
        bool isExportSelecting,
        DiagramNode? selectedNode,
        DiagramTransition? selectedTransition)
    {
        if (isExportSelecting)
        {
            return
            [
                new HelpPage
                (
                    [
                        new HelpHint("左ドラッグ", "範囲作成・調整"),
                        new HelpHint("Enter", "撮影"),
                        new HelpHint("Alt", "吸着なし"),
                        new HelpHint("右クリック/Esc", "キャンセル")
                    ]
                ),
                new HelpPage
                (
                    [
                        new HelpHint("Ctrl+P", "PNG出力開始"),
                        new HelpHint("0-9", "テーマ"),
                        new HelpHint("空白", "表示移動")
                    ]
                )
            ];
        }

        if (isEditingLabel)
        {
            return
            [
                new HelpPage
                (
                    [
                        new HelpHint("Enter", "確定"),
                        new HelpHint("Esc", "キャンセル"),
                        new HelpHint("Backspace", "1文字削除")
                    ]
                )
            ];
        }

        if (selectedTransition is not null)
        {
            return
            [
                new HelpPage
                (
                    [
                        new HelpHint("F2・Enter", "ラベル編集"),
                        new HelpHint("Tab", "ラベル左右切替"),
                        new HelpHint("Delete", "遷移削除")
                    ]
                ),
                new HelpPage
                (
                    [
                        new HelpHint("Shift+ドラッグ", "遷移作成"),
                        new HelpHint("Shift+同一状態", "自己ループ"),
                        new HelpHint("Ctrl+S", "保存")
                    ]
                ),
                new HelpPage
                (
                    [
                        new HelpHint("Ctrl+O", "読込"),
                        new HelpHint("Ctrl+P", "PNG出力"),
                        new HelpHint("0-9", "テーマ")
                    ]
                )
            ];
        }

        if (selectedNode is not null)
        {
            return
            [
                new HelpPage
                (
                    [
                        new HelpHint("F2・Enter", "ラベル編集"),
                        new HelpHint("T", "状態種別変更"),
                        new HelpHint("C", "状態色変更"),
                        new HelpHint("Delete", "状態削除")
                    ]
                ),
                new HelpPage
                (
                    [
                        new HelpHint("N", "状態追加"),
                        new HelpHint("Ctrl+N", "新規作成"),
                        new HelpHint("Ctrl+S", "保存")
                    ]
                ),
                new HelpPage
                (
                    [
                        new HelpHint("Ctrl+O", "読込"),
                        new HelpHint("Ctrl+P", "PNG出力"),
                        new HelpHint("0-9", "テーマ")
                    ]
                )
            ];
        }

        return
        [
            new HelpPage
            (
                [
                    new HelpHint("N", "状態追加"),
                    new HelpHint("Ctrl+N", "新規作成"),
                    new HelpHint("Ctrl+S", "保存")
                ]
            ),
            new HelpPage
            (
                [
                    new HelpHint("Ctrl+Shift+S", "名前を付けて保存"),
                    new HelpHint("Ctrl+O", "読込"),
                    new HelpHint("R", "サンプル図に戻す")
                ]
            ),
            new HelpPage
            (
                [
                    new HelpHint("F2・Enter", "ラベル編集"),
                    new HelpHint("T", "状態種別変更"),
                    new HelpHint("C", "状態色変更"),
                    new HelpHint("Delete", "削除")
                ]
            ),
            new HelpPage
            (
                [
                    new HelpHint("Shift+ドラッグ", "遷移作成"),
                    new HelpHint("Shift+同一状態", "自己ループ"),
                    new HelpHint("Tab", "ラベル左右切替")
                ]
            ),
            new HelpPage
            (
                [
                    new HelpHint("Ctrl+P", "PNG出力"),
                    new HelpHint("0-9", "テーマ"),
                    new HelpHint("空白", "表示移動")
                ]
            )
        ];
    }

    /// <summary>
    /// ショートカットキーのヒントを描画する
    /// </summary>
    /// <param name="position"></param>
    /// <param name="key"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    private Vector2 DrawShortcutHint(Vector2 position, string key, string description)
    {
        var x = DrawKeyCap(key, position);
        x = DrawUiText(description, new Vector2(x + 6, position.Y + 3), _keyCapTheme.DescriptionTextColor, 14, false);
        return new Vector2(x + 12, position.Y);
    }

    /// <summary>
    /// ヘルプの区切り線を描画する
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2 DrawHelpSeparator(Vector2 position)
    {
        var x = DrawUiText("/", new Vector2(position.X, position.Y + 3), _keyCapTheme.SeparatorTextColor, 14, false);
        return new Vector2(x + 12, position.Y);
    }

    /// <summary>
    /// キーキャップを描画する
    /// </summary>
    /// <param name="text">キーキャップに表示するテキスト</param>
    /// <param name="position">描画位置</param>
    /// <returns>描画後のX座標</returns>
    private float DrawKeyCap(string text, Vector2 position)
    {
        var textTexture = GetUiTextTexture(text, _keyCapTheme.FontSize, true);
        var width = Math.Max(_keyCapTheme.MinWidth, textTexture.Width + (_keyCapTheme.HorizontalPadding * 2));
        var height = _keyCapTheme.Height;
        var bounds = new Rectangle((int)position.X, (int)position.Y, width, height);

        _spriteBatch.Draw(_pixel, bounds, _keyCapTheme.FaceColor);
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), _keyCapTheme.TopEdgeColor);
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, 1, bounds.Height), _keyCapTheme.TopEdgeColor);
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Bottom - 2, bounds.Width, 2), _keyCapTheme.BottomEdgeColor);
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.Right - 1, bounds.Y, 1, bounds.Height), _keyCapTheme.BottomEdgeColor);
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, 1), _keyCapTheme.InnerHighlightColor);

        var textPosition = new Vector2(
            bounds.X + (bounds.Width - textTexture.Width) / 2f,
            bounds.Y + (bounds.Height - textTexture.Height) / 2f);
        _spriteBatch.Draw(textTexture, textPosition, _keyCapTheme.LabelTextColor);
        return bounds.Right;
    }

    /// <summary>
    /// UIテキストを描画する
    /// </summary>
    /// <param name="text"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="size"></param>
    /// <param name="bold"></param>
    /// <returns></returns>
    private float DrawUiText(string text, Vector2 position, Color color, float size, bool bold)
    {
        var texture = GetUiTextTexture(text, size, bold);
        _spriteBatch.Draw(texture, position, color);
        return position.X + texture.Width;
    }

    /// <summary>
    /// UIテキストのテクスチャを取得する
    /// </summary>
    /// <param name="text">テキスト内容</param>
    /// <param name="size">フォントサイズ</param>
    /// <param name="bold">太字かどうか</param>
    /// <returns>テクスチャ</returns>
    private Texture2D GetUiTextTexture(string text, float size, bool bold)
    {
        var cacheKey = $"ui|{size}|{bold}|{text}";
        if (_uiTextTextureCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        var texture = TextRenderer.CreateUiTextTexture(_graphicsDevice, text, size, bold);
        _uiTextTextureCache[cacheKey] = texture;
        return texture;
    }
}

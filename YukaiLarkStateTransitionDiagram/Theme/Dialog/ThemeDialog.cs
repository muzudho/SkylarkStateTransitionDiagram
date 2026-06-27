namespace YukaiLarkStateTransitionDiagram;

using System;
using System.Collections.Generic;
using System.Linq;
using YukaiLarkStateTransitionDiagram.Persistence;
using YukaiLarkStateTransitionDiagram.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public partial class Game1
{
    private const int ThemeMenuPageSize = 5;
    private static readonly Keys[] ThemeDigitKeys =
    [
        Keys.D0,
        Keys.D1,
        Keys.D2,
        Keys.D3,
        Keys.D4,
        Keys.D5,
        Keys.D6,
        Keys.D7,
        Keys.D8,
        Keys.D9
    ];
    private static readonly Keys[] ThemeNumPadKeys =
    [
        Keys.NumPad0,
        Keys.NumPad1,
        Keys.NumPad2,
        Keys.NumPad3,
        Keys.NumPad4,
        Keys.NumPad5,
        Keys.NumPad6,
        Keys.NumPad7,
        Keys.NumPad8,
        Keys.NumPad9
    ];

    private readonly List<IKeyCapTheme> _themeShortcutThemes = KeyCapThemes.ShortcutThemes.ToList();
    private IKeyCapTheme _keyCapTheme = KeyCapThemes.Current;
    private BoardTheme _boardTheme = BoardThemes.ForKeyCapTheme(KeyCapThemes.Current);
    private bool _isThemeMenuOpen;
    private int _themeMenuPage;
    private IKeyCapTheme? _themeMenuOriginalTheme;
    private List<IKeyCapTheme>? _themeMenuOriginalShortcutThemes;

    private void OpenThemeMenu()
    {
        _isThemeMenuOpen = true;
        _themeMenuOriginalTheme = _keyCapTheme;
        _themeMenuOriginalShortcutThemes = _themeShortcutThemes.ToList();
        _themeMenuPage = GetThemePageForTheme(_keyCapTheme);
        _status = "テーマを選んでください。クリックや0-9で試し、決定またはキャンセルで閉じます。";
    }

    private void CloseThemeMenu()
    {
        _isThemeMenuOpen = false;
        _themeMenuOriginalTheme = null;
        _themeMenuOriginalShortcutThemes = null;
    }

    private void ConfirmThemeMenu()
    {
        var confirmedTheme = _keyCapTheme;
        SaveSelectedTheme(confirmedTheme);
        CloseThemeMenu();
        _status = $"テーマを {confirmedTheme.Name} に決定しました。次回起動時にもこのテーマを使います。";
    }

    private void CancelThemeMenu()
    {
        var originalTheme = _themeMenuOriginalTheme;
        var originalShortcutThemes = _themeMenuOriginalShortcutThemes;
        if (originalTheme is not null)
        {
            ApplyKeyCapTheme(originalTheme);
            _themeMenuPage = GetThemePageForTheme(originalTheme);
        }

        if (originalShortcutThemes is not null)
        {
            _themeShortcutThemes.Clear();
            _themeShortcutThemes.AddRange(originalShortcutThemes);
        }

        CloseThemeMenu();
        _status = "テーマ選択をキャンセルしました。";
    }

    private void HandleThemeMenuKeyboard(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.Escape))
        {
            CancelThemeMenu();
            return;
        }

        if (IsNewKeyPress(keyboard, Keys.Enter))
        {
            ConfirmThemeMenu();
            return;
        }

        if (IsNewKeyPress(keyboard, Keys.PageUp))
        {
            MoveThemeMenuPage(-1);
            return;
        }

        if (IsNewKeyPress(keyboard, Keys.PageDown))
        {
            MoveThemeMenuPage(1);
            return;
        }

        if (TryGetThemeShortcutIndex(keyboard, out var themeIndex))
        {
            ApplyThemeShortcut(themeIndex);
        }
    }

    private void HandleThemeMenuMouse(MouseState mouse)
    {
        if (mouse.LeftButton != ButtonState.Pressed || _previousMouse.LeftButton != ButtonState.Released)
        {
            return;
        }

        var point = mouse.Position;
        if (GetThemeMenuConfirmButtonRectangle().Contains(point))
        {
            ConfirmThemeMenu();
            return;
        }

        if (GetThemeMenuCancelButtonRectangle().Contains(point))
        {
            CancelThemeMenu();
            return;
        }

        if (GetThemeMenuPreviousPageButtonRectangle().Contains(point))
        {
            MoveThemeMenuPage(-1);
            return;
        }

        if (GetThemeMenuNextPageButtonRectangle().Contains(point))
        {
            MoveThemeMenuPage(1);
            return;
        }

        for (var pageIndex = 0; pageIndex < GetThemeMenuPageCount(); pageIndex++)
        {
            if (GetThemeMenuPageIndicatorRectangle(pageIndex).Contains(point))
            {
                SelectThemeMenuPage(pageIndex);
                return;
            }
        }

        var startIndex = GetThemeMenuVisibleStartIndex();
        var visibleCount = GetThemeMenuVisibleItemCount();
        for (var visibleIndex = 0; visibleIndex < visibleCount; visibleIndex++)
        {
            var themeIndex = startIndex + visibleIndex;
            if (GetThemeMenuItemRectangle(visibleIndex).Contains(point))
            {
                ApplyKeyCapTheme(KeyCapThemes.AllThemes[themeIndex]);
                return;
            }
        }
    }

    private Rectangle GetThemeButtonRectangle(Viewport viewport)
    {
        if (viewport.Width < 480)
        {
            return Rectangle.Empty;
        }

        var label = $"テーマ: {_keyCapTheme.Name}";
        var width = Math.Min(260, Math.Max(150, GetUiTextTexture(label, 14, true).Width + 48));
        return new Rectangle(viewport.Width - width - 12, 8, width, 28);
    }

    private Rectangle GetThemeMenuPanelRectangle()
    {
        var viewport = GraphicsDevice.Viewport;
        var width = Math.Clamp(viewport.Width - 64, 520, 680);
        var height = Math.Min(viewport.Height - 64, 148 + ThemeMenuPageSize * 48);
        var x = (viewport.Width - width) / 2;
        var y = Math.Max(24, (viewport.Height - height) / 2);
        return new Rectangle(x, y, width, height);
    }

    private Rectangle GetThemeMenuItemRectangle(int visibleIndex)
    {
        var panel = GetThemeMenuPanelRectangle();
        var x = panel.X + 24;
        var y = panel.Y + 92 + visibleIndex * 48;
        return new Rectangle(x, y, panel.Width - 48, 40);
    }

    private Rectangle GetThemeMenuConfirmButtonRectangle()
    {
        var panel = GetThemeMenuPanelRectangle();
        return new Rectangle(panel.Right - 174, panel.Bottom - 44, 150, 32);
    }

    private Rectangle GetThemeMenuCancelButtonRectangle()
    {
        var panel = GetThemeMenuPanelRectangle();
        return new Rectangle(panel.Right - 288, panel.Bottom - 44, 102, 32);
    }

    private Rectangle GetThemeMenuPagerRailRectangle()
    {
        var panel = GetThemeMenuPanelRectangle();
        var cancelButton = GetThemeMenuCancelButtonRectangle();
        return new Rectangle(panel.X + 24, panel.Bottom - 44, Math.Max(126, cancelButton.X - panel.X - 36), 32);
    }

    private Rectangle GetThemeMenuPreviousPageButtonRectangle()
    {
        var rail = GetThemeMenuPagerRailRectangle();
        return new Rectangle(rail.X + 4, rail.Y + 4, 34, 24);
    }

    private Rectangle GetThemeMenuNextPageButtonRectangle()
    {
        var rail = GetThemeMenuPagerRailRectangle();
        return new Rectangle(rail.Right - 38, rail.Y + 4, 34, 24);
    }

    private Rectangle GetThemeMenuPageIndicatorRectangle(int pageIndex)
    {
        const int width = 14;
        const int gap = 7;
        var pageCount = GetThemeMenuPageCount();
        var rail = GetThemeMenuPagerRailRectangle();
        var totalWidth = pageCount * width + Math.Max(0, pageCount - 1) * gap;
        var x = rail.X + (rail.Width - totalWidth) / 2 + pageIndex * (width + gap);
        return new Rectangle(x, rail.Y + 9, width, 14);
    }

    private int GetThemeMenuVisibleStartIndex()
        => Math.Min(_themeMenuPage * ThemeMenuPageSize, Math.Max(0, KeyCapThemes.AllThemes.Count - 1));

    private int GetThemeMenuVisibleItemCount()
    {
        var startIndex = GetThemeMenuVisibleStartIndex();
        return Math.Min(ThemeMenuPageSize, KeyCapThemes.AllThemes.Count - startIndex);
    }

    private int GetThemeMenuPageCount()
        => Math.Max(1, (int)MathF.Ceiling(KeyCapThemes.AllThemes.Count / (float)ThemeMenuPageSize));

    private int GetThemePageForTheme(IKeyCapTheme theme)
    {
        var themeIndex = GetThemeIndex(theme);
        return Math.Clamp(themeIndex, 0, Math.Max(0, KeyCapThemes.AllThemes.Count - 1)) / ThemeMenuPageSize;
    }

    private int GetThemeIndex(IKeyCapTheme theme)
    {
        for (var i = 0; i < KeyCapThemes.AllThemes.Count; i++)
        {
            if (ReferenceEquals(KeyCapThemes.AllThemes[i], theme))
            {
                return i;
            }
        }

        return 0;
    }

    private int? GetShortcutIndexForTheme(IKeyCapTheme theme)
    {
        for (var i = 0; i < _themeShortcutThemes.Count; i++)
        {
            if (ReferenceEquals(_themeShortcutThemes[i], theme))
            {
                return i;
            }
        }

        return null;
    }

    private void MoveThemeMenuPage(int delta)
    {
        var pageCount = GetThemeMenuPageCount();
        _themeMenuPage = (_themeMenuPage + delta + pageCount) % pageCount;
        _status = $"テーマ表ページ {_themeMenuPage + 1}/{pageCount} を表示しています。";
    }

    private void SelectThemeMenuPage(int pageIndex)
    {
        var pageCount = GetThemeMenuPageCount();
        _themeMenuPage = Math.Clamp(pageIndex, 0, pageCount - 1);
        _status = $"テーマ表ページ {_themeMenuPage + 1}/{pageCount} を表示しています。";
    }

    private bool TryGetThemeShortcutIndex(KeyboardState keyboard, out int themeIndex)
    {
        for (var i = 0; i < ThemeDigitKeys.Length; i++)
        {
            if (IsNewKeyPress(keyboard, ThemeDigitKeys[i]) || IsNewKeyPress(keyboard, ThemeNumPadKeys[i]))
            {
                themeIndex = i;
                return true;
            }
        }

        themeIndex = -1;
        return false;
    }

    private void ApplyThemeShortcut(int shortcutIndex)
    {
        if (shortcutIndex < 0 || shortcutIndex >= _themeShortcutThemes.Count)
        {
            return;
        }

        var previousTheme = _keyCapTheme;
        var nextTheme = _themeShortcutThemes[shortcutIndex];
        var previousShortcutIndex = GetShortcutIndexForTheme(previousTheme);
        ApplyKeyCapTheme(nextTheme);
        _themeMenuPage = GetThemePageForTheme(nextTheme);

        if (!ReferenceEquals(previousTheme, nextTheme))
        {
            _themeShortcutThemes[shortcutIndex] = previousTheme;
            if (previousShortcutIndex is not null && previousShortcutIndex.Value != shortcutIndex)
            {
                _themeShortcutThemes[previousShortcutIndex.Value] = nextTheme;
            }

            _status = $"テーマを {shortcutIndex}: {nextTheme.Name} に切り替えました。{shortcutIndex}キーには {previousTheme.Name} を割り当て直しました。";
        }
        else
        {
            _status = $"テーマ {shortcutIndex}: {nextTheme.Name} を選択中です。";
        }
    }

    private void ApplyConfiguredTheme()
    {
        _keyCapTheme = FindThemeByName(_appConfig.SelectedThemeName);
        _boardTheme = BoardThemes.ForKeyCapTheme(_keyCapTheme);
    }

    private static IKeyCapTheme FindThemeByName(string? themeName)
    {
        if (!string.IsNullOrWhiteSpace(themeName))
        {
            foreach (var theme in KeyCapThemes.AllThemes)
            {
                if (string.Equals(theme.Name, themeName, StringComparison.OrdinalIgnoreCase))
                {
                    return theme;
                }
            }
        }

        return KeyCapThemes.YukaiLark;
    }

    private void SaveSelectedTheme(IKeyCapTheme theme)
    {
        _appConfig.SelectedThemeName = theme.Name;
        AppConfigStore.Save(_appConfig);
    }

    private void ApplyKeyCapTheme(IKeyCapTheme theme)
    {
        _keyCapTheme = theme;
        _boardTheme = BoardThemes.ForKeyCapTheme(_keyCapTheme);
        _edgeRenderer.Theme = _boardTheme;
        _nodeRenderer.Theme = _boardTheme;
        _shortcutKeyRenderer.KeyCapTheme = _keyCapTheme;
        _shortcutKeyRenderer.BoardTheme = _boardTheme;
        _status = $"テーマを {_keyCapTheme.Name} に切り替えました。背景とPNG出力にも反映します。";
    }
}

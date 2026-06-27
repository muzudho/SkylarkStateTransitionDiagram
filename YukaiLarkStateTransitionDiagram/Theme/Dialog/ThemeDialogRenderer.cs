namespace YukaiLarkStateTransitionDiagram;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YukaiLarkStateTransitionDiagram.Theme;

public partial class Game1
{
    private void DrawThemeButton(Viewport viewport)
    {
        var bounds = GetThemeButtonRectangle(viewport);
        if (bounds.Width <= 0)
        {
            return;
        }

        var label = $"テーマ: {_keyCapTheme.Name}";
        _spriteBatch.Draw(_pixel, bounds, WithAlpha(_keyCapTheme.FaceColor, 236));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), WithAlpha(_keyCapTheme.TopEdgeColor, 245));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, 1, bounds.Height), WithAlpha(_keyCapTheme.TopEdgeColor, 245));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Bottom - 2, bounds.Width, 2), WithAlpha(_keyCapTheme.BottomEdgeColor, 245));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.Right - 1, bounds.Y, 1, bounds.Height), WithAlpha(_keyCapTheme.BottomEdgeColor, 245));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, 1), WithAlpha(_keyCapTheme.InnerHighlightColor, 210));
        DrawScreenRectangleOutline(bounds, WithAlpha(_boardTheme.HeaderBorderColor, 210), 1);
        DrawUiText("T", new Vector2(bounds.X + 8, bounds.Y + 6), _keyCapTheme.LabelTextColor, 13, true);
        DrawUiText(label, new Vector2(bounds.X + 32, bounds.Y + 5), _keyCapTheme.LabelTextColor, 14, true);
    }

    private void DrawThemeMenuOverlay()
    {
        if (!_isThemeMenuOpen)
        {
            return;
        }

        var viewport = GraphicsDevice.Viewport;
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), WithAlpha(Blend(_boardTheme.BackgroundColor, Color.Black, 0.42f), 150));

        var panel = GetThemeMenuPanelRectangle();
        _spriteBatch.Draw(_pixel, new Rectangle(panel.X + 6, panel.Y + 8, panel.Width, panel.Height), WithAlpha(Blend(_boardTheme.BackgroundColor, Color.Black, 0.42f), 115));
        _spriteBatch.Draw(_pixel, panel, WithAlpha(_boardTheme.PanelBackgroundColor, 246));
        DrawScreenRectangleOutline(panel, WithAlpha(_boardTheme.PanelTopEdgeColor, 235), 2);

        DrawUiText("テーマ選択", new Vector2(panel.X + 24, panel.Y + 20), _boardTheme.PanelPrimaryTextColor, 24, true);
        DrawUiText("クリックや0-9キーで切り替えます。後ろの画面で見た目を確認できます。", new Vector2(panel.X + 24, panel.Y + 56), _boardTheme.PanelSecondaryTextColor, 15, false);
        DrawThemeMenuActionButtons();
        DrawThemeMenuPager();
        DrawThemeMenuTableHeader();
        DrawThemeMenuShortcutList();

        var pageCount = GetThemeMenuPageCount();
        var pageText = $"({_themeMenuPage + 1}/{pageCount})";
        var pageTexture = GetUiTextTexture(pageText, 15, true);
        var table = GetThemeMenuThemeTableRectangle();
        DrawUiText(pageText, new Vector2(table.Right - pageTexture.Width, panel.Y + 62), _boardTheme.PanelMutedTextColor, 15, true);

        var startIndex = GetThemeMenuVisibleStartIndex();
        var visibleCount = GetThemeMenuVisibleItemCount();
        for (var visibleIndex = 0; visibleIndex < visibleCount; visibleIndex++)
        {
            DrawThemeMenuItem(startIndex + visibleIndex, visibleIndex);
        }
    }

    private void DrawThemeMenuTableHeader()
    {
        var table = GetThemeMenuThemeTableRectangle();
        var shortcutList = GetThemeMenuShortcutListRectangle();
        var headerY = table.Y - 24;

        DrawUiText("テーマ一覧", new Vector2(table.X, headerY), _boardTheme.PanelPrimaryTextColor, 15, true);
        DrawUiText("キー", new Vector2(table.X + 58, headerY), _boardTheme.PanelMutedTextColor, 13, true);
        DrawUiText("テーマ", new Vector2(table.X + 92, headerY), _boardTheme.PanelMutedTextColor, 13, true);
        DrawUiText("0-9 割り当て", new Vector2(shortcutList.X, headerY), _boardTheme.PanelPrimaryTextColor, 15, true);
    }

    private void DrawThemeMenuShortcutList()
    {
        var bounds = GetThemeMenuShortcutListRectangle();
        _spriteBatch.Draw(_pixel, bounds, WithAlpha(Blend(_boardTheme.PanelBackgroundColor, _boardTheme.BackgroundColor, 0.16f), 220));
        DrawScreenRectangleOutline(bounds, WithAlpha(_boardTheme.PanelTopEdgeColor, 176), 1);

        const int rowHeight = 22;
        for (var index = 0; index < 10; index++)
        {
            var row = new Rectangle(bounds.X + 8, bounds.Y + 8 + index * rowHeight, bounds.Width - 16, rowHeight - 2);
            var hasTheme = index < _themeShortcutThemes.Count;
            var theme = hasTheme ? _themeShortcutThemes[index] : null;
            var selected = theme is not null && ReferenceEquals(theme, _keyCapTheme);
            var themeBoard = theme is null ? _boardTheme : BoardThemes.ForKeyCapTheme(theme);
            var faceColor = theme?.FaceColor ?? _keyCapTheme.FaceColor;
            var bottomEdgeColor = theme?.BottomEdgeColor ?? _keyCapTheme.BottomEdgeColor;
            var labelTextColor = theme?.LabelTextColor ?? _boardTheme.PanelMutedTextColor;
            var rowFill = theme is null
                ? WithAlpha(Blend(_boardTheme.PanelBackgroundColor, Color.Black, 0.08f), 120)
                : WithAlpha(Blend(themeBoard.PanelBackgroundColor, faceColor, selected ? 0.34f : 0.2f), (byte)(selected ? 238 : 220));

            _spriteBatch.Draw(_pixel, row, rowFill);
            DrawScreenRectangleOutline(row, WithAlpha(bottomEdgeColor, (byte)(selected ? 224 : 150)), 1);

            var keyBounds = new Rectangle(row.X + 3, row.Y + 2, 28, row.Height - 4);
            _spriteBatch.Draw(_pixel, keyBounds, WithAlpha(faceColor, 226));
            DrawScreenRectangleOutline(keyBounds, WithAlpha(bottomEdgeColor, 220), 1);

            var keyText = index.ToString();
            var keyTexture = GetUiTextTexture(keyText, 12, true);
            DrawUiText(keyText, new Vector2(keyBounds.X + (keyBounds.Width - keyTexture.Width) / 2, keyBounds.Y + 2), labelTextColor, 12, true);

            var name = theme?.Name ?? "-";
            var textColor = theme is null ? _boardTheme.PanelMutedTextColor : themeBoard.PanelPrimaryTextColor;
            DrawUiText(name, new Vector2(row.X + 40, row.Y + 2), textColor, 12, true);
        }
    }

    private void DrawThemeMenuActionButtons()
    {
        DrawThemeMenuActionButton(GetThemeMenuConfirmButtonRectangle(), "このテーマにする");
        DrawThemeMenuActionButton(GetThemeMenuCancelButtonRectangle(), "キャンセル");
    }

    private void DrawThemeMenuPager()
    {
        var rail = GetThemeMenuPagerRailRectangle();
        _spriteBatch.Draw(_pixel, rail, WithAlpha(Blend(_boardTheme.PanelBackgroundColor, _boardTheme.BackgroundColor, 0.22f), 218));
        DrawScreenRectangleOutline(rail, WithAlpha(_boardTheme.PanelTopEdgeColor, 190), 1);
        _spriteBatch.Draw(_pixel, new Rectangle(rail.X + 2, rail.Y + 2, rail.Width - 4, 1), WithAlpha(_keyCapTheme.InnerHighlightColor, 150));

        DrawThemeMenuPageButton(GetThemeMenuPreviousPageButtonRectangle(), "<");
        DrawThemeMenuPageButton(GetThemeMenuNextPageButtonRectangle(), ">");

        for (var pageIndex = 0; pageIndex < GetThemeMenuPageCount(); pageIndex++)
        {
            DrawThemeMenuPageIndicator(pageIndex);
        }
    }

    private void DrawThemeMenuPageButton(Rectangle bounds, string label)
    {
        var enabled = GetThemeMenuPageCount() > 1;
        var face = enabled ? _keyCapTheme.FaceColor : Blend(_keyCapTheme.FaceColor, _boardTheme.PanelBackgroundColor, 0.54f);
        var text = enabled ? _keyCapTheme.LabelTextColor : _boardTheme.PanelMutedTextColor;
        _spriteBatch.Draw(_pixel, bounds, WithAlpha(face, 232));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), WithAlpha(_keyCapTheme.TopEdgeColor, 226));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, 1, bounds.Height), WithAlpha(_keyCapTheme.TopEdgeColor, 226));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Bottom - 2, bounds.Width, 2), WithAlpha(_keyCapTheme.BottomEdgeColor, 226));
        _spriteBatch.Draw(_pixel, new Rectangle(bounds.Right - 1, bounds.Y, 1, bounds.Height), WithAlpha(_keyCapTheme.BottomEdgeColor, 226));

        var labelTexture = GetUiTextTexture(label, 18, true);
        var labelX = bounds.X + (bounds.Width - labelTexture.Width) / 2;
        var labelY = bounds.Y + (bounds.Height - labelTexture.Height) / 2 - 1;
        DrawUiText(label, new Vector2(labelX, labelY), text, 18, true);
    }

    private void DrawThemeMenuPageIndicator(int pageIndex)
    {
        var bounds = GetThemeMenuPageIndicatorRectangle(pageIndex);
        var selected = pageIndex == _themeMenuPage;
        var fill = selected
            ? WithAlpha(_keyCapTheme.BottomEdgeColor, 238)
            : WithAlpha(Blend(_boardTheme.PanelBackgroundColor, _keyCapTheme.FaceColor, 0.16f), 226);
        var edge = selected
            ? WithAlpha(_keyCapTheme.InnerHighlightColor, 238)
            : WithAlpha(_boardTheme.PanelTopEdgeColor, 172);

        _spriteBatch.Draw(_pixel, bounds, fill);
        DrawScreenRectangleOutline(bounds, edge, 1);
        if (selected)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(bounds.X + 4, bounds.Y + 4, bounds.Width - 8, bounds.Height - 8), WithAlpha(_keyCapTheme.InnerHighlightColor, 178));
        }
    }

    private void DrawThemeMenuActionButton(Rectangle bounds, string label)
    {
        _spriteBatch.Draw(_pixel, bounds, WithAlpha(_keyCapTheme.FaceColor, 228));
        DrawScreenRectangleOutline(bounds, WithAlpha(_keyCapTheme.BottomEdgeColor, 232), 1);
        var labelTexture = GetUiTextTexture(label, 14, true);
        var labelX = bounds.X + Math.Max(8, (bounds.Width - labelTexture.Width) / 2);
        DrawUiText(label, new Vector2(labelX, bounds.Y + 8), _keyCapTheme.LabelTextColor, 14, true);
    }

    private void DrawThemeMenuItem(int themeIndex, int visibleIndex)
    {
        var theme = KeyCapThemes.AllThemes[themeIndex];
        var bounds = GetThemeMenuItemRectangle(visibleIndex);
        var themeField = new Rectangle(bounds.X, bounds.Y, bounds.Width - 92, bounds.Height);
        var selected = ReferenceEquals(theme, _keyCapTheme);
        var themeBoard = BoardThemes.ForKeyCapTheme(theme);
        var fill = WithAlpha(Blend(themeBoard.PanelBackgroundColor, theme.FaceColor, 0.18f), 232);
        var edge = WithAlpha(theme.BottomEdgeColor, 190);

        _spriteBatch.Draw(_pixel, themeField, fill);
        DrawScreenRectangleOutline(themeField, edge, 1);
        _spriteBatch.Draw(_pixel, new Rectangle(themeField.X + 12, themeField.Y + 10, 34, 24), theme.FaceColor);
        DrawScreenRectangleOutline(new Rectangle(themeField.X + 12, themeField.Y + 10, 34, 24), theme.BottomEdgeColor, 1);

        var shortcut = GetShortcutIndexForTheme(theme);
        var shortcutText = shortcut is null ? "-" : shortcut.Value.ToString();
        DrawUiText(shortcutText, new Vector2(themeField.X + 58, themeField.Y + 12), theme.LabelTextColor, 16, true);
        DrawUiText(theme.Name, new Vector2(themeField.X + 92, themeField.Y + 11), themeBoard.PanelPrimaryTextColor, 17, true);
        if (selected)
        {
            DrawUiText("選択中", new Vector2(themeField.Right + 18, bounds.Y + 13), _boardTheme.PanelPrimaryTextColor, 14, true);
        }
    }
}

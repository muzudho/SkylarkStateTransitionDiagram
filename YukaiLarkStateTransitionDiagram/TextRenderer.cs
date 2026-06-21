namespace YukaiLarkStateTransitionDiagram;

using System;
using System.Linq;
using DrawingBitmap = System.Drawing.Bitmap;
using DrawingBrushes = System.Drawing.Brushes;
using DrawingColor = System.Drawing.Color;
using DrawingFont = System.Drawing.Font;
using DrawingFontFamily = System.Drawing.FontFamily;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingGraphics = System.Drawing.Graphics;
using DrawingGraphicsUnit = System.Drawing.GraphicsUnit;
using DrawingRectangleF = System.Drawing.RectangleF;
using DrawingStringFormat = System.Drawing.StringFormat;
using DrawingStringFormatFlags = System.Drawing.StringFormatFlags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// 文字列のテクスチャ描画
/// </summary>
public static class TextRenderer
{
    /// <summary>
    /// 文字列を描画してテクスチャを作成する。
    /// </summary>
    private static readonly DrawingStringFormat CenteredStringFormat = new()
    {
        Alignment = System.Drawing.StringAlignment.Center,
        LineAlignment = System.Drawing.StringAlignment.Center,
        FormatFlags = DrawingStringFormatFlags.NoWrap
    };

    /// <summary>
    /// 文字列を描画してテクスチャを作成する。
    /// </summary>
    private static readonly DrawingStringFormat LeftAlignedStringFormat = new()
    {
        Alignment = System.Drawing.StringAlignment.Near,
        LineAlignment = System.Drawing.StringAlignment.Center,
        FormatFlags = DrawingStringFormatFlags.NoWrap
    };

    /// <summary>
    /// 文字列を描画してテクスチャを作成する。
    /// </summary>
    private static readonly DrawingStringFormat StringFormatNoWrap = new()
    {
        FormatFlags = DrawingStringFormatFlags.NoWrap
    };

    /// <summary>
    /// UI用の文字テクスチャを生成する。
    /// </summary>
    /// <param name="graphicsDevice">描画先のグラフィックスデバイス</param>
    /// <param name="text">描画文字列</param>
    /// <param name="size">フォントサイズ</param>
    /// <param name="bold">太字かどうか</param>
    /// <returns>文字テクスチャ</returns>
    public static Texture2D CreateUiTextTexture(GraphicsDevice graphicsDevice, string text, float size, bool bold)
    {
        var renderedText = string.IsNullOrEmpty(text) ? " " : text;
        using var font = CreateJapaneseFont(size, bold);
        using var measureBitmap = new DrawingBitmap(1, 1);
        using var measureGraphics = DrawingGraphics.FromImage(measureBitmap);
        var measured = measureGraphics.MeasureString(renderedText, font, 1024, StringFormatNoWrap);
        var width = Math.Clamp((int)Math.Ceiling(measured.Width) + 4, 8, 1024);
        var height = Math.Clamp((int)Math.Ceiling(measured.Height) + 4, 8, 64);
        using var bitmap = new DrawingBitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using var graphics = DrawingGraphics.FromImage(bitmap);
        graphics.Clear(DrawingColor.Transparent);
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        graphics.DrawString(renderedText, font, DrawingBrushes.White, new DrawingRectangleF(0, 0, width, height), LeftAlignedStringFormat);

        return CreateTexture(graphicsDevice, bitmap, width, height);
    }

    /// <summary>
    /// ラベル用の文字テクスチャを生成する。
    /// </summary>
    /// <param name="graphicsDevice">描画先のグラフィックスデバイス</param>
    /// <param name="label">描画するラベル</param>
    /// <param name="editing">編集中かどうか</param>
    /// <returns>文字テクスチャ</returns>
    public static Texture2D CreateLabelTexture(GraphicsDevice graphicsDevice, string label, bool editing)
    {
        var text = string.IsNullOrEmpty(label) ? " " : label;
        using var font = CreateJapaneseFont(22, true);
        using var measureBitmap = new DrawingBitmap(1, 1);
        using var measureGraphics = DrawingGraphics.FromImage(measureBitmap);
        var measured = measureGraphics.MeasureString(text, font, 512, StringFormatNoWrap);
        var width = Math.Clamp((int)Math.Ceiling(measured.Width) + 18, 48, 220);
        var height = Math.Clamp((int)Math.Ceiling(measured.Height) + 10, 30, 72);
        using var bitmap = new DrawingBitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using var graphics = DrawingGraphics.FromImage(bitmap);
        graphics.Clear(DrawingColor.Transparent);
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        if (editing)
        {
            using var editBrush = new System.Drawing.SolidBrush(DrawingColor.FromArgb(170, 20, 24, 30));
            graphics.FillRectangle(editBrush, 0, 0, width, height);
        }

        graphics.DrawString(text, font, DrawingBrushes.White, new DrawingRectangleF(0, 0, width, height), CenteredStringFormat);
        return CreateTexture(graphicsDevice, bitmap, width, height);
    }

    /// <summary>
    /// 日本語を描画しやすいフォントを作成する。
    /// </summary>
    /// <param name="size">フォントサイズ</param>
    /// <param name="bold">太字かどうか</param>
    /// <returns>フォント</returns>
    public static DrawingFont CreateJapaneseFont(float size, bool bold)
    {
        var candidates = new[] { "Yu Gothic UI", "Meiryo", "MS Gothic", "Noto Sans CJK JP", "Arial Unicode MS" };
        foreach (var candidate in candidates)
        {
            if (DrawingFontFamily.Families.Any(f => string.Equals(f.Name, candidate, StringComparison.OrdinalIgnoreCase)))
            {
                return new DrawingFont(candidate, size, bold ? DrawingFontStyle.Bold : DrawingFontStyle.Regular, DrawingGraphicsUnit.Pixel);
            }
        }

        return new DrawingFont(DrawingFontFamily.GenericSansSerif, size, bold ? DrawingFontStyle.Bold : DrawingFontStyle.Regular, DrawingGraphicsUnit.Pixel);
    }

    /// <summary>
    /// ビットマップからテクスチャを作成する。
    /// </summary>
    /// <param name="graphicsDevice">描画先のグラフィックスデバイス</param>
    /// <param name="bitmap">ビットマップ</param>
    /// <param name="width">幅</param>
    /// <param name="height">高さ</param>
    /// <returns>テクスチャ</returns>
    private static Texture2D CreateTexture(GraphicsDevice graphicsDevice, DrawingBitmap bitmap, int width, int height)
    {
        var colors = new Color[width * height];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                colors[y * width + x] = new Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }
        }

        var texture = new Texture2D(graphicsDevice, width, height);
        texture.SetData(colors);
        return texture;
    }
}

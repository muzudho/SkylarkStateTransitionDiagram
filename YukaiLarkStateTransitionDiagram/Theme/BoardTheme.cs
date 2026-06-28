namespace YukaiLarkStateTransitionDiagram.Theme;

using System;
using Microsoft.Xna.Framework;

public sealed record BoardTheme(
    Color BackgroundColor,
    Color GridColor,
    Color ExportBackdropColor,
    Color PhotoPaperColor,
    Color PhotoEdgeColor,
    Color PinColor,
    Color TransitionLineColor,
    Color TransitionLabelColor,
    Color SelectedTransitionLineColor,
    Color SelectedTransitionLabelColor,
    Color TransitionHandleColor,
    Color TransitionControlHandleColor,
    Color TransitionGuideColor,
    Color[]? NormalNodePaletteOverride = null)
{
    public const int NormalNodePaletteLength = 6;

    public BoardTheme WithNormalNodePaletteOverride(Color[]? palette)
        => new(
            BackgroundColor,
            GridColor,
            ExportBackdropColor,
            PhotoPaperColor,
            PhotoEdgeColor,
            PinColor,
            TransitionLineColor,
            TransitionLabelColor,
            SelectedTransitionLineColor,
            SelectedTransitionLabelColor,
            TransitionHandleColor,
            TransitionControlHandleColor,
            TransitionGuideColor,
            palette);

    public Color[] NormalNodePalette { get; } = NormalizeNormalNodePalette(
        NormalNodePaletteOverride,
        CreateNormalNodePalette(
            BackgroundColor,
            PhotoPaperColor,
            PhotoEdgeColor,
            PinColor,
            TransitionLineColor,
            TransitionLabelColor,
            SelectedTransitionLineColor,
            TransitionControlHandleColor,
            GridColor));

    private bool IsLightBackground => GetLuminance(BackgroundColor) >= 0.58f;

    public Color HeaderBackgroundColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 238)
        : WithAlpha(Blend(BackgroundColor, Color.Black, 0.18f), 238);
    public Color HeaderBorderColor => WithAlpha(IsLightBackground ? PhotoEdgeColor : GridColor, 220);
    public Color HeaderTitleTextColor => IsLightBackground ? TransitionLabelColor : PhotoPaperColor;
    public Color HeaderStatusTextColor => IsLightBackground
        ? Blend(TransitionLabelColor, BackgroundColor, 0.18f)
        : TransitionLabelColor;

    public Color PanelBackgroundColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 226)
        : WithAlpha(Blend(BackgroundColor, Color.Black, 0.10f), 224);
    public Color PanelTopEdgeColor => WithAlpha(IsLightBackground ? PinColor : GridColor, 210);
    public Color PanelBottomEdgeColor => WithAlpha(Blend(BackgroundColor, Color.Black, 0.36f), 220);
    public Color PanelPrimaryTextColor => HeaderTitleTextColor;
    public Color PanelSecondaryTextColor => HeaderStatusTextColor;
    public Color PanelMutedTextColor => IsLightBackground
        ? Blend(TransitionLabelColor, BackgroundColor, 0.34f)
        : Blend(TransitionLabelColor, BackgroundColor, 0.18f);

    public Color BottomBarBackgroundColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 220)
        : WithAlpha(Blend(BackgroundColor, Color.Black, 0.14f), 218);

    public Color NodeOuterRingColor => IsLightBackground
        ? WithAlpha(Blend(TransitionLabelColor, BackgroundColor, 0.10f), 245)
        : WithAlpha(Blend(PhotoPaperColor, BackgroundColor, 0.32f), 238);
    public Color SelectedNodeOuterRingColor => IsLightBackground
        ? WithAlpha(SelectedTransitionLineColor, 245)
        : WithAlpha(PhotoPaperColor, 245);
    public Color NormalNodeOutlineColor => IsLightBackground
        ? WithAlpha(Blend(TransitionLabelColor, BackgroundColor, 0.04f), 245)
        : WithAlpha(Blend(TransitionLineColor, PhotoPaperColor, 0.18f), 238);
    public Color StartMarkerFillColor => ToneMarkerColor(PinColor);
    public Color EndMarkerFillColor => ToneMarkerColor(SelectedTransitionLineColor);
    public Color StartMarkerOutlineColor => GetMarkerOutlineColor(StartMarkerFillColor);
    public Color EndMarkerOutlineColor => GetMarkerOutlineColor(EndMarkerFillColor);
    public Color MarkerOutlineColor => GetMarkerOutlineColor(Blend(StartMarkerFillColor, EndMarkerFillColor, 0.5f));
    public Color NodeLabelTextColor => IsLightBackground
        ? PhotoPaperColor
        : WithAlpha(PhotoPaperColor, 248);

    public Color NodeGhostHaloColor => WithAlpha(SelectedTransitionLineColor, IsLightBackground ? (byte)88 : (byte)108);
    public Color NodeGhostInnerHaloColor => WithAlpha(PhotoPaperColor, IsLightBackground ? (byte)82 : (byte)98);
    public Color NodeResizeHandleColor => SelectedTransitionLineColor;
    public Color NodeSelectedGlowColor => SelectedTransitionLineColor;
    public Color NodeSelectedInnerGlowColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 232)
        : WithAlpha(SelectedTransitionLabelColor, 232);
    public Color NodeSelectedSweepColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 230)
        : WithAlpha(SelectedTransitionLabelColor, 230);
    public Color LabelEditorBackgroundColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 236)
        : WithAlpha(Blend(BackgroundColor, Color.Black, 0.26f), 226);
    public Color LabelEditorBorderColor => WithAlpha(SelectedTransitionLineColor, 214);
    public Color LabelEditorTextColor => IsLightBackground
        ? TransitionLabelColor
        : WithAlpha(PhotoPaperColor, 248);
    public Color LabelEditorPlaceholderTextColor => IsLightBackground
        ? WithAlpha(Blend(TransitionLabelColor, BackgroundColor, 0.38f), 220)
        : WithAlpha(Blend(PhotoPaperColor, BackgroundColor, 0.34f), 220);
    public Color HandleOutlineColor => IsLightBackground
        ? WithAlpha(Blend(TransitionLabelColor, BackgroundColor, 0.10f), 232)
        : WithAlpha(Blend(BackgroundColor, PhotoPaperColor, 0.18f), 232);
    public Color AssistantBubbleColor => IsLightBackground
        ? WithAlpha(PhotoPaperColor, 238)
        : WithAlpha(Blend(BackgroundColor, GridColor, 0.58f), 238);
    public Color AssistantBubbleBorderColor => WithAlpha(IsLightBackground ? PinColor : TransitionLineColor, 224);
    public Color AssistantCompletedBubbleBorderColor => WithAlpha(IsLightBackground ? PhotoEdgeColor : SelectedTransitionLineColor, 232);
    public Color AssistantTitleTextColor => IsLightBackground ? TransitionLabelColor : SelectedTransitionLabelColor;
    public Color AssistantBodyTextColor => IsLightBackground ? PanelSecondaryTextColor : Blend(SelectedTransitionLabelColor, TransitionLabelColor, 0.28f);
    public Color AssistantHintTextColor => IsLightBackground ? PanelMutedTextColor : Blend(SelectedTransitionLabelColor, BackgroundColor, 0.26f);
    public Color AssistantCutInShadowColor => WithAlpha(Blend(BackgroundColor, Color.Black, IsLightBackground ? 0.28f : 0.46f), 72);
    public Color AssistantCutInBandColor => WithAlpha(Blend(BackgroundColor, GridColor, IsLightBackground ? 0.42f : 0.28f), 166);
    public Color AssistantCutInFrameColor => WithAlpha(Blend(BackgroundColor, PhotoPaperColor, IsLightBackground ? 0.18f : 0.12f), 94);
    public Color AssistantCutInPrimaryTextColor => IsLightBackground ? TransitionLabelColor : SelectedTransitionLabelColor;
    public Color AssistantCutInSecondaryTextColor => IsLightBackground ? PanelSecondaryTextColor : Blend(SelectedTransitionLabelColor, TransitionLabelColor, 0.22f);

    private static float GetLuminance(Color color)
        => ((0.2126f * color.R) + (0.7152f * color.G) + (0.0722f * color.B)) / 255f;

    private static Color WithAlpha(Color color, byte alpha)
        => new(color.R, color.G, color.B, alpha);

    private static Color[] NormalizeNormalNodePalette(Color[]? overridePalette, Color[] generatedPalette)
    {
        if (overridePalette is { Length: NormalNodePaletteLength })
        {
            return overridePalette;
        }

        return generatedPalette;
    }
    private static Color[] CreateNormalNodePalette(
        Color backgroundColor,
        Color photoPaperColor,
        Color photoEdgeColor,
        Color pinColor,
        Color transitionLineColor,
        Color transitionLabelColor,
        Color selectedTransitionLineColor,
        Color transitionControlHandleColor,
        Color gridColor)
    {
        var isLightBackground = GetLuminance(backgroundColor) >= 0.58f;
        return new[]
        {
            ToneNormalNodeColor(transitionLineColor, backgroundColor, photoPaperColor, isLightBackground),
            ToneNormalNodeColor(transitionControlHandleColor, backgroundColor, photoPaperColor, isLightBackground),
            ToneNormalNodeColor(photoEdgeColor, backgroundColor, photoPaperColor, isLightBackground),
            ToneNormalNodeColor(gridColor, backgroundColor, photoPaperColor, isLightBackground),
            ToneNormalNodeColor(Blend(pinColor, transitionLabelColor, 0.34f), backgroundColor, photoPaperColor, isLightBackground),
            ToneNormalNodeColor(Blend(selectedTransitionLineColor, transitionLabelColor, 0.46f), backgroundColor, photoPaperColor, isLightBackground)
        };
    }

    private Color ToneMarkerColor(Color color)
        => IsLightBackground
            ? WithAlpha(Blend(color, Color.Black, 0.48f), 248)
            : WithAlpha(Blend(color, PhotoPaperColor, 0.18f), 248);

    private Color GetMarkerOutlineColor(Color fillColor)
    {
        var preferred = IsLightBackground ? WithAlpha(TransitionLineColor, 245) : WithAlpha(PhotoPaperColor, 245);
        if (MathF.Abs(GetLuminance(fillColor) - GetLuminance(preferred)) >= 0.30f)
        {
            return preferred;
        }

        var fallback = GetLuminance(fillColor) >= 0.50f
            ? Blend(Color.Black, TransitionLabelColor, IsLightBackground ? 0.08f : 0.18f)
            : Blend(Color.White, PhotoPaperColor, IsLightBackground ? 0.10f : 0.22f);
        return WithAlpha(fallback, 245);
    }

    private static Color ToneNormalNodeColor(Color color, Color backgroundColor, Color photoPaperColor, bool isLightBackground)
    {
        var toned = isLightBackground
            ? Blend(color, Color.Black, 0.30f)
            : Blend(color, photoPaperColor, 0.12f);

        var luminance = GetLuminance(toned);
        if (isLightBackground && luminance > 0.48f)
        {
            toned = Blend(toned, Color.Black, 0.24f);
        }
        else if (!isLightBackground && luminance < 0.36f)
        {
            toned = Blend(toned, photoPaperColor, 0.24f);
        }

        return WithAlpha(toned, 255);
    }
    private static Color Blend(Color from, Color to, float amount)
    {
        var clamped = MathHelper.Clamp(amount, 0f, 1f);
        return new Color(
            (byte)MathF.Round(MathHelper.Lerp(from.R, to.R, clamped)),
            (byte)MathF.Round(MathHelper.Lerp(from.G, to.G, clamped)),
            (byte)MathF.Round(MathHelper.Lerp(from.B, to.B, clamped)),
            (byte)MathF.Round(MathHelper.Lerp(from.A, to.A, clamped)));
    }
}

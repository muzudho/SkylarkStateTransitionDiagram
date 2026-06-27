namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class KifuwarabeKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Kifuwarabe";
    public override Color FaceColor => new(207, 194, 234);
    public override Color TopEdgeColor => new(244, 235, 255);
    public override Color BottomEdgeColor => new(124, 70, 153);
    public override Color InnerHighlightColor => new(224, 213, 246);
    public override Color LabelTextColor => new(54, 37, 78);
    public override Color DescriptionTextColor => new(82, 90, 128);
    public override Color SeparatorTextColor => new(149, 126, 168);
    public override int Height => 24;
    public override int MinWidth => 38;
    public override int HorizontalPadding => 10;
    public override float FontSize => 13.5f;
}

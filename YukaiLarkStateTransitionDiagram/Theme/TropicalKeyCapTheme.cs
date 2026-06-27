namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class TropicalKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Tropical";
    public override Color FaceColor => new(42, 196, 154);
    public override Color TopEdgeColor => new(185, 255, 221);
    public override Color BottomEdgeColor => new(0, 104, 115);
    public override Color InnerHighlightColor => new(80, 220, 172);
    public override Color LabelTextColor => new(22, 67, 72);
    public override Color DescriptionTextColor => new(56, 122, 102);
    public override Color SeparatorTextColor => new(224, 154, 64);
}

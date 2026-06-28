namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class HalloweenKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Halloween";
    public override Color FaceColor => new(236, 126, 38);
    public override Color TopEdgeColor => new(255, 192, 86);
    public override Color BottomEdgeColor => new(72, 38, 98);
    public override Color InnerHighlightColor => new(255, 146, 50);
    public override Color LabelTextColor => new(50, 34, 58);
    public override Color DescriptionTextColor => new(118, 82, 108);
    public override Color SeparatorTextColor => new(155, 194, 74);
}

namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class DesertKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Desert";
    public override Color FaceColor => new(225, 172, 86);
    public override Color TopEdgeColor => new(255, 225, 156);
    public override Color BottomEdgeColor => new(130, 75, 37);
    public override Color InnerHighlightColor => new(239, 192, 108);
    public override Color LabelTextColor => new(73, 47, 29);
    public override Color DescriptionTextColor => new(112, 91, 66);
    public override Color SeparatorTextColor => new(188, 132, 66);
}

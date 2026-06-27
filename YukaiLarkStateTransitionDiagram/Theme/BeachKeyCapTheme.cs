namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class BeachKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Beach";
    public override Color FaceColor => new(255, 226, 151);
    public override Color TopEdgeColor => new(255, 246, 206);
    public override Color BottomEdgeColor => new(42, 129, 156);
    public override Color InnerHighlightColor => new(255, 236, 181);
    public override Color LabelTextColor => new(28, 78, 96);
    public override Color DescriptionTextColor => new(62, 123, 139);
    public override Color SeparatorTextColor => new(162, 139, 94);
}

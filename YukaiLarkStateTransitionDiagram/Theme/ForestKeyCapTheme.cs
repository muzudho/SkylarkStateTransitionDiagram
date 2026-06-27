namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class ForestKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Forest";
    public override Color FaceColor => new(78, 126, 74);
    public override Color TopEdgeColor => new(170, 202, 132);
    public override Color BottomEdgeColor => new(31, 61, 44);
    public override Color InnerHighlightColor => new(98, 148, 88);
    public override Color LabelTextColor => new(242, 238, 202);
    public override Color DescriptionTextColor => new(99, 116, 78);
    public override Color SeparatorTextColor => new(143, 112, 68);
}

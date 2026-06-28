namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class PurpleGrapesKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "PurpleGrapes";
    public override Color FaceColor => new(116, 70, 154);
    public override Color TopEdgeColor => new(190, 150, 222);
    public override Color BottomEdgeColor => new(55, 31, 92);
    public override Color InnerHighlightColor => new(142, 88, 178);
    public override Color LabelTextColor => new(255, 242, 226);
    public override Color DescriptionTextColor => new(132, 104, 158);
    public override Color SeparatorTextColor => new(117, 170, 88);
}

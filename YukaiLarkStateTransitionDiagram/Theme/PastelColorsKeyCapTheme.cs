namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class PastelColorsKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "PastelColors";
    public override Color FaceColor => new(250, 214, 232);
    public override Color TopEdgeColor => new(255, 246, 250);
    public override Color BottomEdgeColor => new(166, 132, 188);
    public override Color InnerHighlightColor => new(254, 230, 241);
    public override Color LabelTextColor => new(71, 58, 104);
    public override Color DescriptionTextColor => new(96, 114, 136);
    public override Color SeparatorTextColor => new(166, 145, 182);
}

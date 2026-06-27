namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class NightSkyKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "NightSky";
    public override Color FaceColor => new(38, 50, 103);
    public override Color TopEdgeColor => new(116, 136, 198);
    public override Color BottomEdgeColor => new(10, 16, 44);
    public override Color InnerHighlightColor => new(58, 72, 132);
    public override Color LabelTextColor => new(245, 239, 196);
    public override Color DescriptionTextColor => new(118, 139, 190);
    public override Color SeparatorTextColor => new(224, 188, 92);
}

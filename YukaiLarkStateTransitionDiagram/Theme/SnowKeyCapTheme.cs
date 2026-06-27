namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class SnowKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Snow";
    public override Color FaceColor => new(232, 246, 252);
    public override Color TopEdgeColor => new(255, 255, 255);
    public override Color BottomEdgeColor => new(118, 151, 174);
    public override Color InnerHighlightColor => new(246, 253, 255);
    public override Color LabelTextColor => new(39, 70, 93);
    public override Color DescriptionTextColor => new(74, 105, 126);
    public override Color SeparatorTextColor => new(144, 170, 188);
}

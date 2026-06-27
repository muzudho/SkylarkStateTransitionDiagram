namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class RainyKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Rainy";
    public override Color FaceColor => new(99, 128, 158);
    public override Color TopEdgeColor => new(168, 197, 220);
    public override Color BottomEdgeColor => new(42, 58, 78);
    public override Color InnerHighlightColor => new(119, 149, 178);
    public override Color LabelTextColor => new(235, 244, 250);
    public override Color DescriptionTextColor => new(94, 118, 142);
    public override Color SeparatorTextColor => new(135, 154, 172);
}

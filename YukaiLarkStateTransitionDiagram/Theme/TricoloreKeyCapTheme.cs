namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class TricoloreKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Tricolore";
    public override Color FaceColor => new(244, 244, 238);
    public override Color TopEdgeColor => new(255, 255, 255);
    public override Color BottomEdgeColor => new(48, 84, 158);
    public override Color InnerHighlightColor => new(252, 252, 248);
    public override Color LabelTextColor => new(32, 54, 102);
    public override Color DescriptionTextColor => new(78, 94, 126);
    public override Color SeparatorTextColor => new(190, 56, 62);
}

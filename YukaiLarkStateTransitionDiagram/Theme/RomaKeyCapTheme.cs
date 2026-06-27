namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class RomaKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Roma";
    public override Color FaceColor => new(202, 88, 57);
    public override Color TopEdgeColor => new(239, 178, 128);
    public override Color BottomEdgeColor => new(93, 50, 42);
    public override Color InnerHighlightColor => new(218, 112, 76);
    public override Color LabelTextColor => new(255, 239, 210);
    public override Color DescriptionTextColor => new(86, 89, 66);
    public override Color SeparatorTextColor => new(152, 119, 84);
}

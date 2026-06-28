namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class ChristmasKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Christmas";
    public override Color FaceColor => new(190, 42, 58);
    public override Color TopEdgeColor => new(255, 144, 138);
    public override Color BottomEdgeColor => new(24, 104, 62);
    public override Color InnerHighlightColor => new(214, 64, 72);
    public override Color LabelTextColor => new(255, 248, 224);
    public override Color DescriptionTextColor => new(106, 126, 90);
    public override Color SeparatorTextColor => new(218, 176, 72);
}

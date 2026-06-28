namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class DyeingPoisonDartFrogKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "DyeingPoisonDartFrog";
    public override Color FaceColor => new(255, 212, 42);
    public override Color TopEdgeColor => new(255, 245, 112);
    public override Color BottomEdgeColor => new(22, 64, 110);
    public override Color InnerHighlightColor => new(255, 226, 66);
    public override Color LabelTextColor => new(26, 46, 72);
    public override Color DescriptionTextColor => new(64, 94, 116);
    public override Color SeparatorTextColor => new(28, 152, 170);
}

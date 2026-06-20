namespace SkylarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class YukaiLarkKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "YukaiLark";
    public override Color FaceColor => new(255, 232, 124);
    public override Color TopEdgeColor => new(255, 251, 202);
    public override Color BottomEdgeColor => new(218, 151, 70);
    public override Color InnerHighlightColor => new(255, 244, 160);
    public override Color LabelTextColor => new(71, 60, 46);
    public override Color DescriptionTextColor => new(93, 123, 127);
    public override Color SeparatorTextColor => new(144, 157, 139);
    public override int Height => 24;
    public override int MinWidth => 38;
    public override int HorizontalPadding => 10;
    public override float FontSize => 13.5f;
}

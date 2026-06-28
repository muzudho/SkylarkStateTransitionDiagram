namespace YukaiLarkStateTransitionDiagram.Theme;

using Microsoft.Xna.Framework;

public sealed class OkinawaKeyCapTheme : KeyCapThemeBase
{
    public override string Name => "Okinawa";
    public override Color FaceColor => new(66, 194, 210);
    public override Color TopEdgeColor => new(180, 245, 238);
    public override Color BottomEdgeColor => new(210, 88, 88);
    public override Color InnerHighlightColor => new(96, 216, 220);
    public override Color LabelTextColor => new(32, 74, 88);
    public override Color DescriptionTextColor => new(66, 122, 128);
    public override Color SeparatorTextColor => new(230, 172, 74);
}

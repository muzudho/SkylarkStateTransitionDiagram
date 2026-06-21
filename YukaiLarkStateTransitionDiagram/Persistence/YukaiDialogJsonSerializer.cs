namespace YukaiLarkStateTransitionDiagram.Persistence;

using System.Text.Json;

internal static class YukaiDialogJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        IncludeFields = true
    };
}

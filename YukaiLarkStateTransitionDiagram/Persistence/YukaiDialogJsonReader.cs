namespace YukaiLarkStateTransitionDiagram.Persistence;

using System.IO;
using System.Text;
using System.Text.Json;

public static class YukaiDialogJsonReader
{
    public static DiagramDocument? Read(string path)
    {
        var json = File.ReadAllText(path, Encoding.UTF8);
        return JsonSerializer.Deserialize<DiagramDocument>(json, YukaiDialogJsonSerializer.Options);
    }
}

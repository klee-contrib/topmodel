namespace TopModel.Core.Types;

public class JavaType
{
#nullable disable
    public string Type { get; set; }

    public List<string> Imports { get; set; } = new List<string>();

    public List<string> Annotations { get; set; } = new List<string>();
}
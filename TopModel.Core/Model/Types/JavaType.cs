namespace TopModel.Core.Types;

public class JavaType
{
#nullable disable
    public string Type { get; set; }
#nullable enable

    public List<string>? Imports { get; set; }

    public List<string>? Annotations { get; set; }
}
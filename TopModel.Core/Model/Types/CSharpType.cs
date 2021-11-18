namespace TopModel.Core.Types;

public class CSharpType
{
#nullable disable
    public string Type { get; set; }
#nullable enable

    public IList<string> Annotations { get; set; } = new List<string>();

    public IList<string> Usings { get; set; } = new List<string>();

    public bool UseSqlTypeName { get; set; }
}
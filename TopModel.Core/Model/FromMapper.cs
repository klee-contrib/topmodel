namespace TopModel.Core;

public class FromMapper
{
#nullable enable
    public string? Comment { get; set; }

    public List<ClassMappings> Params { get; } = new();

    public FromMapper? ParentMapper { get; set; }

#nullable disable
    internal LocatedString Reference { get; set; }
}

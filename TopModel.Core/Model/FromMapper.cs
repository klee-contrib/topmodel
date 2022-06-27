namespace TopModel.Core;

public class FromMapper
{
    public List<ClassMappings> Params { get; } = new();

    public FromMapper? ParentMapper { get; set; }

#nullable disable
    internal LocatedString Reference { get; set; }
#nullable enable
}

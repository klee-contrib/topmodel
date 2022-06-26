namespace TopModel.Core;

public class FromMapper
{
    public List<ClassMappings> Params { get; } = new();

#nullable disable
    internal LocatedString Reference { get; set; }
#nullable enable
}

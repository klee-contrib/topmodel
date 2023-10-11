namespace TopModel.Core;

public class FromMapper
{
#nullable enable
    public string? Comment { get; set; }

    public List<ClassMappings> Params { get; } = new();

#nullable disable
    internal LocatedString Reference { get; set; }
}

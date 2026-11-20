using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class FromMapper
{
    public string? Comment { get; internal set; }

    public IList<MapperParam> Params { get; } = [];

    public IEnumerable<ClassMappings> ClassParams =>
        Params.Where(p => p is ClassMappings).Select(p => (ClassMappings)p.Value);

    public IEnumerable<PropertyMapping> PropertyParams =>
        Params.Where(p => p is PropertyMapping).Select(p => (PropertyMapping)p.Value);

    public required Class Class { get; init; }

    internal IList<MapperParam> OwnParams { get; } = [];

    internal IEnumerable<PropertyMapping> OwnPropertyParams =>
        OwnParams.Where(p => p is PropertyMapping).Select(p => (PropertyMapping)p.Value);

    internal LocatedString Reference { get; set; } = null!;
}

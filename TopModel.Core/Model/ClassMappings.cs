using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class ClassMappings
{
    public bool To { get; init; }

    public LocatedString Name { get; internal set; } = null!;

    public Class Class { get; internal set; } = null!;

    public ClassReference ClassReference { get; internal set; } = null!;

    public bool Required { get; internal set; } = true;

    public string? Comment { get; internal set; }

    public IDictionary<IProperty, IProperty> Mappings { get; } = new Dictionary<IProperty, IProperty>();

    public IDictionary<Reference, Reference> MappingReferences { get; internal set; } =
        new Dictionary<Reference, Reference>();

    public IEnumerable<IProperty> MissingRequiredProperties =>
        Class
            .Properties.Where(p =>
                p.Required
                && (p is ({ Composition: not null } or { DefaultValue: null }) and { AssociationMultiple: false })
                && !(
                    p.Class.IsPersistent && p.PrimaryKey && p.Class.PrimaryKey.Count() == 1 && p.GeneratedValue != null
                )
            )
            .Except(Mappings.Select(m => m.Value));
}

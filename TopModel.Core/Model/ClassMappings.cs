using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class ClassMappings
{
#nullable disable
    public bool To { get; set; }

    public LocatedString Name { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public bool Required { get; set; } = true;

#nullable enable
    public string? Comment { get; set; }

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

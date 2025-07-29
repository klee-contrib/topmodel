using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class CompositionProperty : IProperty
{
#nullable disable
    public Class Composition { get; set; }

    public string Name { get; set; }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToPascalCase(strictIfUppercase: true);

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToCamelCase(strictIfUppercase: true);

    public string NameByClassPascal => NamePascal;

    public string NameByClassCamel => NameCamel;

    public Domain Domain { get; set; }

    public Dictionary<string, string> DomainParameters { get; set; } = [];

    public string Comment { get; set; }

    public bool Readonly { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

    public string Label { get; set; }

    public bool IsMultipart => Composition.Properties.Any(cpp => cpp.Domain?.IsMultipart ?? false);

    public bool PrimaryKey => false;

    public bool Required { get; set; } = true;

#nullable enable

    public string DefaultValue => throw new NotImplementedException();

    public LocatedString? Trigram { get; set; }

    public IList<AnnotationInstance> Annotations { get; private set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; set; } = [];

    public Dictionary<string, string> CustomProperties { get; private set; } = [];

    public IProperty? CompositionPrimaryKey
    {
        get
        {
            var cpPks = Composition.ExtendedProperties.Where(p => p.PrimaryKey);
            if (!cpPks.Any())
            {
                cpPks = Composition.ExtendedProperties.OfType<AliasProperty>().Where(p => p.AliasedPrimaryKey);
            }

            return cpPks.Count() == 1 ? cpPks.Single() : null;
        }
    }

    public bool UseLegacyRoleName { get; init; }

    public Decorator? SourceDecorator { get; set; }

    public DomainReference? DomainReference { get; set; }

#nullable disable
    public ClassReference Reference { get; set; }

    internal Reference Location { get; set; }
#nullable enable
#pragma warning disable KTA1600

    /// <inheritdoc cref="IProperty.CloneForDecorator" />
    public IProperty CloneForDecorator(Class? classe = null, Endpoint? endpoint = null, Decorator? decorator = null)
    {
        return new CompositionProperty
        {
            SourceDecorator = Decorator,
            Class = classe,
            Comment = Comment,
            Composition = Composition,
            Decorator = decorator,
            Domain = Domain,
            DomainParameters = DomainParameters,
            Endpoint = endpoint,
            Location = Location,
            Name = Name,
            Required = Required,
            CustomProperties = CustomProperties,
            Readonly = Readonly,
            Trigram = Trigram,
            UseLegacyRoleName = UseLegacyRoleName,
            Annotations = Annotations
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
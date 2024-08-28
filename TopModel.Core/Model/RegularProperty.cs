using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class RegularProperty : IProperty
{
#nullable disable
    public string Name { get; set; }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToPascalCase();

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToCamelCase();

    public string NameByClassPascal => NamePascal;

    public string NameByClassCamel => NameCamel;

#nullable enable

    public string? Label { get; set; }

    public bool PrimaryKey { get; set; }

    public bool Required { get; set; }

    public bool Readonly { get; set; }

    public LocatedString? Trigram { get; set; }

#nullable disable
    public Domain Domain { get; set; }

    public string[] DomainParameters { get; set; } = [];

    public string Comment { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

    public DomainReference DomainReference { get; set; }

#nullable enable

    public string? DefaultValue { get; set; }

    public bool UseLegacyRoleName { get; init; }

#nullable disable
    internal Reference Location { get; set; }
#nullable enable
#pragma warning disable KTA1600

    /// <inheritdoc cref="IProperty.CloneWithClassOrEndpoint" />
    public IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null)
    {
        return new RegularProperty
        {
            Class = classe,
            Comment = Comment,
            Decorator = Decorator,
            DefaultValue = DefaultValue,
            Domain = Domain,
            Endpoint = endpoint,
            Label = Label,
            Location = Location,
            Name = Name,
            PrimaryKey = PrimaryKey,
            Required = Required
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
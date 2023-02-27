using System.Text;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class AssociationProperty : IFieldProperty
{
    private IFieldProperty? _property;

    public LocatedString? Trigram { get; set; }

#nullable disable
    public Class Association { get; set; }

    public IFieldProperty Property
    {
        get => _property ?? Association?.PrimaryKey.Single();
        set => _property = value;
    }

#nullable enable

    public string? Label { get; set; }

#nullable disable
    public string Comment { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }
#nullable enable

    public string? Role { get; set; }

    public AssociationType Type { get; set; }

    public bool Required { get; set; }

    public bool Readonly { get; set; }

    public string? DefaultValue { get; set; }

    public string Name
    {
        get
        {
            if (Association == null)
            {
                return string.Empty;
            }

            var name = new StringBuilder();

            if (Type == AssociationType.OneToMany || Type == AssociationType.ManyToMany)
            {
                name.Append(Association.PluralName);
            }
            else if (Association.Extends == null)
            {
                name.Append(Association.Name);
            }

            if (Type == AssociationType.ManyToOne || Type == AssociationType.OneToOne)
            {
                name.Append(Property?.Name);
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                name.Append(Role?.Replace(" ", string.Empty));
            }

            return name.ToString();
        }
    }

    public string NameCamel
    {
        get
        {
            if (((IProperty)this).Parent.PreservePropertyCasing)
            {
                return Name;
            }

            if (Association == null)
            {
                return string.Empty;
            }

            var name = new StringBuilder();

            if (Type == AssociationType.OneToMany || Type == AssociationType.ManyToMany)
            {
                name.Append(Association.PluralNameCamel);
            }
            else if (Association.Extends == null)
            {
                name.Append(Association.NameCamel);
            }

            if (Type == AssociationType.ManyToOne || Type == AssociationType.OneToOne)
            {
                if (Association.Extends == null)
                {
                    name.Append(Property?.NameCamel.ToFirstUpper());
                }
                else
                {
                    name.Append(Property?.NameCamel);
                }
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                name.Append(Role?.Replace(" ", string.Empty).ToPascalCase());
            }

            return name.ToString();
        }
    }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing ? Name : NameCamel.ToFirstUpper();

    public Domain Domain => Type == AssociationType.ManyToMany || Type == AssociationType.OneToMany ? Property?.Domain.ListDomain! : Property?.Domain!;

    public bool PrimaryKey { get; set; }

    public Reference? PropertyReference { get; set; }

#nullable disable
    public ClassReference Reference { get; set; }

    public bool UseLegacyRoleName { get; init; }

    internal Reference Location { get; set; }
#nullable enable

    public IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null)
    {
        return new AssociationProperty
        {
            Association = Association,
            Class = classe,
            Comment = Comment,
            Decorator = Decorator,
            DefaultValue = DefaultValue,
            Endpoint = endpoint,
            Label = Label,
            Location = Location,
            Required = Required,
            Role = Role,
            Type = Type
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
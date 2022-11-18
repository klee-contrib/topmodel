using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class AssociationProperty : IFieldProperty
{
    private IFieldProperty? _property;

    public LocatedString? Trigram { get; set; }

#nullable disable
    public Class Association { get; set; }

    public IFieldProperty Property
    {
        get => _property ?? Association?.PrimaryKey!;
        set => _property = value;
    }

#nullable enable

    public string Label { get; set; }

#nullable disable
    public string Comment { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }
#nullable enable

    public string? Role { get; set; }

    public AssociationType Type { get; set; }

    public bool Required { get; set; }

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
            if (Association.Extends == null)
            {
                if (Type == AssociationType.OneToMany || Type == AssociationType.ManyToMany)
                {
                    name.Append(Association.PluralName);
                }
                else
                {
                    name.Append(Association.Name);
                }
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

    public Domain Domain => Property?.Domain!;

    public bool PrimaryKey => false;

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
}
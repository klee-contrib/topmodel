using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class AssociationProperty : IFieldProperty
{
#nullable disable
    public Class Association { get; set; }
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
            if (Association.Extends == null && !AsAlias)
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
                name.Append(Association?.Properties.Single(p => p.PrimaryKey).Name);
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                name.Append(Role?.Replace(" ", string.Empty));
            }

            return name.ToString();
        }
    }

    public Domain Domain => Association.Properties.OfType<IFieldProperty>().Single(p => p.PrimaryKey).Domain;

    public bool PrimaryKey => false;

    public bool AsAlias { get; set; }

#nullable disable
    public ClassReference Reference { get; set; }

    internal Reference Location { get; set; }

    public IProperty CloneWithClass(Class classe)
    {
        return new AssociationProperty
        {
            AsAlias = AsAlias,
            Association = Association,
            Class = classe,
            Comment = Comment,
            Decorator = Decorator,
            DefaultValue = DefaultValue,
            Label = Label,
            Location = Location,
            Required = Required,
            Role = Role,
            Type = Type
        };
    }
}
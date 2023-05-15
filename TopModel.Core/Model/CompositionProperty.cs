using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class CompositionProperty : IProperty
{
#nullable disable
    public Class Composition { get; set; }

    public string Name { get; set; }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToPascalCase();

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToCamelCase();

    public string NameByClassPascal => NamePascal;

    public string NameByClassCamel => NameCamel;

    public Domain Domain { get; set; }

    public string Comment { get; set; }

    public bool Readonly { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public string Label => Name;

    public bool PrimaryKey => false;

    public ClassReference Reference { get; set; }

    internal Reference Location { get; set; }

#nullable enable
    internal DomainReference? DomainReference { get; set; }

    /// <inheritdoc cref="IProperty.CloneWithClassOrEndpoint" />
    public IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null)
    {
        return new CompositionProperty
        {
            Class = classe,
            Comment = Comment,
            Composition = Composition,
            Decorator = Decorator,
            Domain = Domain,
            Endpoint = endpoint,
            Location = Location,
            Name = Name
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
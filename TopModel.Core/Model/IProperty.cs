namespace TopModel.Core;

public interface IProperty
{
    string Name { get; }

    string NamePascal { get; }

    string NameCamel { get; }

    string NameByClassPascal { get; }

    string NameByClassCamel { get; }

    string? Label { get; }

    bool PrimaryKey { get; }

    bool Required { get; }

    Domain Domain { get; }

    string[] DomainParameters { get; }

    string Comment { get; }

    bool Readonly { get; set; }

    Class Class { get; set; }

    Endpoint Endpoint { get; set; }

    Decorator Decorator { get; set; }

    PropertyMapping PropertyMapping { get; set; }

    IPropertyContainer Parent => Class ?? (IPropertyContainer)Endpoint ?? (IPropertyContainer)Decorator ?? PropertyMapping;

    IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null);
}
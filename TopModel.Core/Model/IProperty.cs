namespace TopModel.Core;

public interface IProperty
{
    string Name { get; }

    string? Label { get; }

    bool PrimaryKey { get; }

    string Comment { get; }

    bool Readonly { get; set; }

    Class Class { get; set; }

    Endpoint Endpoint { get; set; }

    Decorator Decorator { get; set; }

    IPropertyContainer Parent => Class ?? (IPropertyContainer)Endpoint ?? Decorator;

    IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null);
}
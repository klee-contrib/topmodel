namespace TopModel.Core;

public interface IProperty
{
    string Name { get; }

    string? Label { get; }

    bool PrimaryKey { get; }

    string Comment { get; }

    Class Class { get; set; }

    Endpoint Endpoint { get; set; }

    Decorator Decorator { get; set; }

    IProperty CloneWithClass(Class classe);
}
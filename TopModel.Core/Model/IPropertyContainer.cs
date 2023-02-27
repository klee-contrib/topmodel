using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IPropertyContainer
{
    ModelFile ModelFile { get; }

    LocatedString Name { get; }

    string NamePascal { get; }

    string NameCamel { get; }

    Namespace Namespace { get; }

    IList<IProperty> Properties { get; }

    bool PreservePropertyCasing { get; }
}

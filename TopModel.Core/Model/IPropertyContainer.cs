using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IPropertyContainer
{
    ModelFile ModelFile { get; }

    LocatedString Name { get; }

    Namespace Namespace { get; }

    IList<IProperty> Properties { get; }
}

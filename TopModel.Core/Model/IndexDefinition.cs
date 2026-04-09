using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class IndexDefinition
{
    public IList<IProperty> Properties { get; } = [];

    public bool Unique { get; init; }

    public IList<Reference> PropertyReferences { get; } = [];
}

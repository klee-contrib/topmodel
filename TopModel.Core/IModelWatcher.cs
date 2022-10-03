using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IModelWatcher
{
    string Name { get; }

    int Number { get; init; }

    string FullName => $"{Name.PadRight(18, '.')}@{Number}";

    IEnumerable<string>? GeneratedFiles { get; }

    void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors);

    void OnFilesChanged(IEnumerable<ModelFile> files, ModelStoreConfig? storeConfig = null);
}
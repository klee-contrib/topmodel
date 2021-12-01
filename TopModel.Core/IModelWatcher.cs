using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IModelWatcher
{
    string Name { get; }

    int Number { get; set; }

    string FullName => $"{Name}@{Number}";

    void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors);

    void OnFilesChanged(IEnumerable<ModelFile> files);
}
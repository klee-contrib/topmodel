using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IModelReporter
{
    void RegisterChange();

    void RegisterErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors);

    void Report(bool refresh = false);
}

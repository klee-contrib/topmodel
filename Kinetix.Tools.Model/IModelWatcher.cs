using System.Collections.Generic;
using TopModel.Core.FileModel;

namespace TopModel.Core
{
    public interface IModelWatcher
    {
        string Name { get; }

        void OnFilesChanged(IEnumerable<ModelFile> files);
    }
}

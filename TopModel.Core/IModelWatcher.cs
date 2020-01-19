using System.Collections.Generic;
using TopModel.Core.FileModel;

namespace TopModel.Core
{
    public interface IModelWatcher
    {
        void OnFilesChanged(IEnumerable<ModelFile> files);
    }
}

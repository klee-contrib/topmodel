using System.Collections.Generic;

namespace TopModel.Core.FileModel
{
    public delegate void ModelFileChangeEventHandler(object sender, IEnumerable<ModelFile> modelFile);
}
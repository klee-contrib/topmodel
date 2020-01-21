using System.Collections.Generic;
using TopModel.Core.FileModel;

namespace TopModel.UI
{
    public delegate void ModelChangeEventHandler(object sender, IDictionary<FileName, ModelFile> files);
}
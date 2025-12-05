using TopModel.Core.FileModel;
using YamlDotNet.Core;

namespace TopModel.Core.Loaders;

internal interface ILoader
{
    void Load(Parser parser, ModelFile modelFile, Reference location);
}

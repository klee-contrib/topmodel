using System.Collections.Generic;
using System.Linq;
using TopModel.Core.FileModel;

namespace TopModel.Generator
{
    public abstract class GeneratorBase : IModelWatcher
    {
        private readonly GeneratorConfigBase _config;

        protected GeneratorBase(GeneratorConfigBase config)
        {
            _config = config;
        }

        public abstract string Name { get; }

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            HandleFiles(files.Where(file => _config.Kinds.Contains(file.Descriptor.Kind)));
        }

        protected abstract void HandleFiles(IEnumerable<ModelFile> files);
    }
}

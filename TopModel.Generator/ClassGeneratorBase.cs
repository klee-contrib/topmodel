using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator;

public abstract class ClassGeneratorBase : GeneratorBase
{
    private readonly GeneratorConfigBase _config;

    public ClassGeneratorBase(ILogger<ClassGeneratorBase> logger, GeneratorConfigBase config)
        : base(logger, config)
    {
        _config = config;
    }

    public override IEnumerable<string> GeneratedFiles => Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
        .SelectMany(c => _config.Tags.Intersect(GetClassTags(c)).Select(tag => GetFileName(c, tag)))
        .Distinct();

    protected virtual bool FilterClass(Class classe)
    {
        return true;
    }

    protected abstract string GetFileName(Class classe, string tag);

    protected abstract void HandleClass(string fileName, Class classe, string tag);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            foreach (var classe in file.Classes.Where(FilterClass))
            {
                foreach (var (tag, fileName) in _config.Tags.Intersect(file.Tags)
                     .Select(tag => (tag, fileName: GetFileName(classe, tag)))
                     .DistinctBy(t => t.fileName))
                {
                    HandleClass(fileName, classe, tag);
                }
            }
        }
    }
}
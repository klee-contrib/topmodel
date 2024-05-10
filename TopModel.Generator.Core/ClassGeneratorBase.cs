using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class ClassGeneratorBase<T> : GeneratorBase<T>
    where T : GeneratorConfigBase
{
    public ClassGeneratorBase(ILogger<ClassGeneratorBase<T>> logger)
        : base(logger)
    {
    }

    public override IEnumerable<string> GeneratedFiles => Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
        .SelectMany(c => Config.Tags.Intersect(c.Tags).Select(tag => GetFileName(c, tag)))
        .Distinct();

    protected virtual bool FilterClass(Class classe)
    {
        return true;
    }

    protected abstract string GetFileName(Class classe, string tag);

    protected abstract void HandleClass(string fileName, Class classe, string tag);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        Parallel.ForEach(files, file =>
            Parallel.ForEach(file.Classes.Where(FilterClass), classe =>
                Parallel.ForEach(
                    Config.Tags.Intersect(classe.Tags)
                         .Select(tag => (tag, fileName: GetFileName(classe, tag)))
                         .DistinctBy(t => t.fileName),
                    l => HandleClass(l.fileName, classe, l.tag))));
    }
}
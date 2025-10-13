using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class ClassGeneratorBase<T>(ILogger<ClassGeneratorBase<T>> logger, IFileWriterProvider writerProvider)
    : GeneratorBase<T>(logger, writerProvider)
    where T : GeneratorConfigBase
{
    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass).Concat(Config.GetExtraClasses(f)))
            .SelectMany(c => Config.Tags.Intersect(c.Tags).Select(tag => GetFileName(c, tag)).Distinct());

    protected virtual bool FilterClass(Class classe)
    {
        return true;
    }

    protected abstract string GetFileName(Class classe, string tag);

    protected abstract void HandleClass(string fileName, Class classe, string tag);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        Parallel.ForEach(
            files,
            file =>
                Parallel.ForEach(
                    file.Classes.Where(FilterClass).Concat(Config.GetExtraClasses(file)),
                    classe =>
                        Parallel.ForEach(
                            Config
                                .Tags.Intersect(classe.Tags)
                                .Select(tag => (tag, fileName: GetFileName(classe, tag)))
                                .DistinctBy(t => t.fileName),
                            l => HandleClass(l.fileName, classe, l.tag)
                        )
                )
        );
    }
}

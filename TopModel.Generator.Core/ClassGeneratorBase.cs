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
            .Files.Values.SelectMany(f => f.Classes.Where(c => FilterClass(c) && Config.FilterClass(c)))
            .SelectMany(c => Config.Tags.Intersect(c.Tags).Select(tag => GetFileName(c, tag)).Distinct());

    protected virtual bool FilterClass(Class classe)
    {
        return true;
    }

    protected abstract string GetFileName(Class classe, string tag);

    protected abstract Task HandleClass(string fileName, Class classe, string tag, CancellationToken ct = default);

    protected override async Task HandleFiles(IEnumerable<ModelFile> files, CancellationToken ct = default)
    {
        await Parallel.ForEachAsync(files, ct, async (file, ct) =>
            await Parallel.ForEachAsync(
                file.Classes.Where(c => FilterClass(c) && Config.FilterClass(c)),
                ct,
                async (classe, ct) =>
                    await Parallel.ForEachAsync(
                        Config
                            .Tags.Intersect(classe.Tags)
                            .Select(tag => (tag, fileName: GetFileName(classe, tag)))
                            .DistinctBy(t => t.fileName),
                        ct,
                        async (l, ct) =>
                        {
                            await HandleClass(l.fileName, classe, l.tag, ct);
                        }
                    )
            )
        );
    }
}

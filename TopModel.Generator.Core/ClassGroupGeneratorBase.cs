using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class ClassGroupGeneratorBase<T> : GeneratorBase<T>
    where T : GeneratorConfigBase
{
    public ClassGroupGeneratorBase(ILogger<ClassGroupGeneratorBase<T>> logger)
        : base(logger)
    {
    }

    public override List<string> GeneratedFiles => Classes
        .SelectMany(c => Config.Tags.Intersect(c.Tags).SelectMany(tag => GetFileNames(c, tag)))
        .Select(f => f.FileName)
        .Distinct()
        .ToList();

    protected abstract IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag);

    protected abstract void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in Classes
            .SelectMany(classe => Config.Tags.Intersect(classe.Tags)
                .SelectMany(tag => GetFileNames(classe, tag)
                    .Select(f => (key: (f.FileType, f.FileName), tag, classe))))
            .GroupBy(f => f.key))
        {
            HandleFile(file.Key.FileType, file.Key.FileName, file.First().tag, file.Select(f => f.classe).Distinct());
        }
    }
}
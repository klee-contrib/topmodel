using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class MapperGeneratorBase<T> : GeneratorBase<T>
    where T : GeneratorConfigBase
{
    public MapperGeneratorBase(ILogger<MapperGeneratorBase<T>> logger)
        : base(logger)
    {
    }

    public override IEnumerable<string> GeneratedFiles => Mappers
        .SelectMany(m => Config.Tags.Intersect(GetClassTags(m.Classe)).Select(tag => GetFileName(m.Classe, m.IsPersistant, tag)))
        .Distinct();

    protected IEnumerable<(Class Classe, FromMapper Mapper, bool IsPersistant)> FromMappers => Classes
        .SelectMany(classe => classe.FromMappers.Select(mapper => (classe, mapper)))
        .Where(mapper => mapper.mapper.Params.All(p => Classes.Contains(p.Class)))
        .Select(c => (c.classe, c.mapper, c.classe.IsPersistent || c.mapper.Params.Any(p => p.Class.IsPersistent)));

    protected IEnumerable<(Class Classe, ClassMappings Mapper, bool IsPersistant)> ToMappers => Classes
        .SelectMany(classe => classe.ToMappers.Select(mapper => (classe, mapper))
        .Where(mapper => Classes.Contains(mapper.mapper.Class)))
        .Select(c => (c.classe, c.mapper, c.classe.IsPersistent || c.mapper.Class.IsPersistent));

    private IEnumerable<(Class Classe, bool IsPersistant)> Mappers =>
        FromMappers.Select(c => (c.Classe, c.IsPersistant))
            .Concat(ToMappers.Select(c => (c.Classe, c.IsPersistant)));

    protected abstract string GetFileName(Class classe, bool isPersistant, string tag);

    protected abstract void HandleFile(bool? isPersistant, string fileName, string tag, IEnumerable<Class> classes);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in Mappers
            .SelectMany(m => Config.Tags.Intersect(GetClassTags(m.Classe))
                .Select(tag => (FileName: GetFileName(m.Classe, m.IsPersistant, tag), Tag: tag, m.Classe, m.IsPersistant)))
            .GroupBy(f => f.FileName))
        {
            HandleFile(file.All(f => f.IsPersistant) ? true : file.All(f => !f.IsPersistant) ? false : null, file.Key, file.First().Tag, file.Select(f => f.Classe));
        }
    }
}
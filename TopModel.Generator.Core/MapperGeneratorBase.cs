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
        .SelectMany(m => Config.Tags.Intersect(GetClassTags(m.Classe)).Select(tag => GetFileName(m.Classe, m.IsPersistent, tag)))
        .Distinct();

    protected IEnumerable<(Class Classe, FromMapper Mapper, bool IsPersistent)> FromMappers => Classes
        .SelectMany(classe => classe.FromMappers.Select(mapper => (classe, mapper)))
        .Where(mapper => mapper.mapper.Params.All(p => Classes.Contains(p.Class)))
        .Select(c => (c.classe, c.mapper, IsPersistent(c.classe) || c.mapper.Params.Any(p => IsPersistent(p.Class))));

    protected IEnumerable<(Class Classe, ClassMappings Mapper, bool IsPersistent)> ToMappers => Classes
        .SelectMany(classe => classe.ToMappers.Select(mapper => (classe, mapper))
        .Where(mapper => Classes.Contains(mapper.mapper.Class)))
        .Select(c => (c.classe, c.mapper, IsPersistent(c.classe) || IsPersistent(c.mapper.Class)));

    private IEnumerable<(Class Classe, bool IsPersistent)> Mappers =>
        FromMappers.Select(c => (c.Classe, c.IsPersistent))
            .Concat(ToMappers.Select(c => (c.Classe, c.IsPersistent)));

    protected abstract string GetFileName(Class classe, bool isPersistent, string tag);

    protected abstract void HandleFile(bool? isPersistent, string fileName, string tag, IEnumerable<Class> classes);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in Mappers
            .SelectMany(m => Config.Tags.Intersect(GetClassTags(m.Classe))
                .Select(tag => (FileName: GetFileName(m.Classe, m.IsPersistent, tag), Tag: tag, m.Classe, m.IsPersistent)))
            .GroupBy(f => f.FileName))
        {
            HandleFile(file.All(f => f.IsPersistent) ? true : file.All(f => !f.IsPersistent) ? false : null, file.Key, file.First().Tag, file.Select(f => f.Classe));
        }
    }

    protected virtual bool IsPersistent(Class classe)
    {
        return classe.IsPersistent;
    }
}
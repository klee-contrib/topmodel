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

    public override IEnumerable<string> GeneratedFiles =>
        FromMappers.SelectMany(m => Config.Tags.Intersect(GetMapperTags(m)).Select(tag => GetFileName(m, tag)))
        .Concat(ToMappers.SelectMany(m => Config.Tags.Intersect(GetMapperTags(m)).Select(tag => GetFileName(m, tag))))
        .Distinct();

    protected IEnumerable<(Class Classe, FromMapper Mapper)> FromMappers => Classes
        .SelectMany(classe => classe.FromMappers.Select(mapper => (classe, mapper)))
        .Where(mapper => mapper.mapper.Params.All(p => Classes.Contains(p.Class)))
        .Select(c => (c.classe, c.mapper));

    protected IEnumerable<(Class Classe, ClassMappings Mapper)> ToMappers => Classes
        .SelectMany(classe => classe.ToMappers.Select(mapper => (classe, mapper))
        .Where(mapper => Classes.Contains(mapper.mapper.Class)))
        .Select(c => (c.classe, c.mapper));

    protected abstract string GetFileName((Class Classe, FromMapper Mapper) mapper, string tag);

    protected abstract string GetFileName((Class Classe, ClassMappings Mapper) mapper, string tag);

    protected abstract void HandleFile(string fileName, string tag, IList<(Class Classe, FromMapper Mapper)> fromMappers, IList<(Class Classe, ClassMappings Mapper)> toMappers);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var fromMappers = FromMappers.SelectMany(mapper => Config.Tags.Intersect(GetMapperTags(mapper))
            .Select(tag => (FileName: GetFileName(mapper, tag), Mapper: mapper, Tag: tag)))
            .GroupBy(f => f.FileName)
            .ToDictionary(f => f.Key, f =>
            {
                var tags = f.Select(m => m.Tag);
                return (
                    Mappers: f.Select(m => m.Mapper)
                        .Distinct()
                        .OrderBy(m => $"{m.Classe.NamePascal} {string.Join(',', m.Mapper.Params.Select(p => p.Name))}", StringComparer.Ordinal)
                        .ToArray(),
                    Tags: tags);
            });

        var toMappers = ToMappers.SelectMany(mapper => Config.Tags.Intersect(GetMapperTags(mapper))
            .Select(tag => (FileName: GetFileName(mapper, tag), Mapper: mapper, Tag: tag)))
            .GroupBy(f => f.FileName)
            .ToDictionary(f => f.Key, f => (
                Mappers: f.Select(m => m.Mapper)
                    .Distinct()
                    .OrderBy(m => $"{m.Mapper.Name} {m.Classe.NamePascal}", StringComparer.Ordinal)
                    .ToArray(),
                Tags: f.Select(m => m.Tag)));

        foreach (var fileName in fromMappers.Keys.Concat(toMappers.Keys).Distinct())
        {
            var (fileFromMappers, fromTags) = fromMappers.ContainsKey(fileName) ? fromMappers[fileName] : (Array.Empty<(Class, FromMapper)>(), Array.Empty<string>());
            var (fileToMappers, toTags) = toMappers.ContainsKey(fileName) ? toMappers[fileName] : (Mappers: Array.Empty<(Class, ClassMappings)>(), Tags: Array.Empty<string>());
            HandleFile(fileName, fromTags.Concat(toTags).First(), fileFromMappers, fileToMappers);
        }
    }

    protected virtual bool IsPersistent(Class classe)
    {
        return classe.IsPersistent;
    }

    private IEnumerable<string> GetMapperTags((Class Classe, FromMapper Mapper) mapper)
    {
        if (IsPersistent(mapper.Classe))
        {
            return GetClassTags(mapper.Classe);
        }

        var persistentParam = mapper.Mapper.Params.FirstOrDefault(p => IsPersistent(p.Class));
        if (persistentParam != null)
        {
            return GetClassTags(persistentParam.Class);
        }

        return GetClassTags(mapper.Classe);
    }

    private IEnumerable<string> GetMapperTags((Class Classe, ClassMappings Mapper) mapper)
    {
        if (IsPersistent(mapper.Classe))
        {
            return GetClassTags(mapper.Classe);
        }

        if (IsPersistent(mapper.Mapper.Class))
        {
            return GetClassTags(mapper.Mapper.Class);
        }

        return GetClassTags(mapper.Classe);
    }
}
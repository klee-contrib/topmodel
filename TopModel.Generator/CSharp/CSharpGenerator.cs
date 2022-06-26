using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.CSharp;

/// <summary>
/// Générateur de code C#.
/// </summary>
public class CSharpGenerator : GeneratorBase
{
    private readonly CSharpConfig _config;
    private readonly ILogger<CSharpGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    private readonly CSharpClassGenerator _classGenerator;
    private readonly DbContextGenerator _dbContextGenerator;
    private readonly MapperGenerator _mapperGenerator;
    private readonly ReferenceAccessorGenerator _referenceAccessorGenerator;

    public CSharpGenerator(ILogger<CSharpGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;

        _classGenerator = new CSharpClassGenerator(_config, _logger);
        _dbContextGenerator = new DbContextGenerator(_config, _logger);
        _mapperGenerator = new MapperGenerator(_config, _logger);
        _referenceAccessorGenerator = new ReferenceAccessorGenerator(_config, _logger);
    }

    public override string Name => "CSharpGen";

    public override IEnumerable<string> GeneratedFiles =>
        new[] { AppName != null ? _config.GetDbContextFilePath(AppName) : null }
        .Concat(_files.Values.SelectMany(f => f.Classes).Select(c => _config.GetClassFileName(c)))
        .Concat(GetMapperModules(_files.Values).Select(module => _config.GetMapperFilePath(module)))
        .Concat(GetReferenceModules(_files.Values).SelectMany(module => new[] { _config.GetReferenceInterfaceFilePath(module), _config.GetReferenceImplementationFilePath(module) }))
        .Where(f => f != null)!;

    private string? AppName => _files.FirstOrDefault().Value?.Classes.FirstOrDefault()?.Namespace.App;

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            GenerateClasses(file);
        }

        if (files.SelectMany(f => f.Classes).Any(c => c.IsPersistent))
        {
            GenerateDbContext();
        }

        var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

        foreach (var classes in GetMapperModules(files))
        {
            _mapperGenerator.Generate(classes, _files.Values.SelectMany(c => c.Classes).ToList());
        }

        foreach (var classes in GetReferenceModules(files))
        {
            _referenceAccessorGenerator.Generate(classes);
        }
    }

    private void GenerateDbContext()
    {
        if (_config.DbContextPath != null && AppName != null)
        {
            _dbContextGenerator.Generate(_files.Values.SelectMany(f => f.Classes).Where(c => c.IsPersistent), AppName);
        }
    }

    private void GenerateClasses(ModelFile file)
    {
        foreach (var classe in file.Classes)
        {
            _classGenerator.Generate(classe);
        }
    }

    private IEnumerable<IEnumerable<Class>> GetMapperModules(IEnumerable<ModelFile> files)
    {
        return files
            .SelectMany(f => f.Classes.Select(c => c.Namespace.Module))
            .Distinct()
            .Select(
                module => _files.Values
                    .SelectMany(f => f.Classes)
                    .Distinct()
                    .Where(c =>
                        c.Namespace.Module == module
                        && c.FromMappers.SelectMany(m => m.Params).Concat(c.ToMappers)
                            .Any(m => _files.SelectMany(f => f.Value.Classes).Contains(c))));
    }

    private IEnumerable<IEnumerable<Class>> GetReferenceModules(IEnumerable<ModelFile> files)
    {
        return files
            .SelectMany(f => f.Classes.Select(c => c.Namespace.Module))
            .Distinct()
            .Select(
                module => _files.Values
                    .SelectMany(f => f.Classes)
                    .Distinct()
                    .Where(c => c.Reference && c.Namespace.Module == module));
    }
}
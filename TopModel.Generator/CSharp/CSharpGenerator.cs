using Microsoft.Extensions.Logging;
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
    private readonly ReferenceAccessorGenerator _referenceAccessorGenerator;

    public CSharpGenerator(ILogger<CSharpGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;

        _classGenerator = new CSharpClassGenerator(_config, _logger);
        _dbContextGenerator = new DbContextGenerator(_config, _logger);
        _referenceAccessorGenerator = new ReferenceAccessorGenerator(_config, _logger);
    }

    public override string Name => "CSharpGen";

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
        foreach (var module in modules)
        {
            GenerateReferences(module);
        }
    }

    private void GenerateDbContext()
    {
        if (_config.DbContextPath != null)
        {
            _dbContextGenerator.Generate(_files.Values.SelectMany(f => f.Classes).Where(c => c.IsPersistent));
        }
    }

    private void GenerateClasses(ModelFile file)
    {
        if (_config.OutputDirectory == null)
        {
            return;
        }

        foreach (var classe in file.Classes)
        {
            _classGenerator.Generate(classe);
        }
    }

    private void GenerateReferences(string module)
    {
        _referenceAccessorGenerator.Generate(
            _files.Values
                .SelectMany(f => f.Classes)
                .Distinct()
                .Where(c => c.Reference && c.Namespace.Module == module));
    }
}
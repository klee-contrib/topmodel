using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

    /// <summary>
    /// Générateur de code C#.
    /// </summary>
    public class CSharpGenerator : IModelWatcher
    {
        private readonly CSharpConfig _config;
        private readonly ILogger<CSharpGenerator> _logger;
        private readonly IDictionary<FileName, ModelFile> _files = new Dictionary<FileName, ModelFile>();

        private readonly CSharpClassGenerator _classGenerator;
        private readonly DbContextGenerator _dbContextGenerator;
        private readonly ReferenceAccessorGenerator _referenceAccessorGenerator;

        public CSharpGenerator(ILogger<CSharpGenerator> logger, CSharpConfig? config = null)
        {
            _config = config!;
            _logger = logger;

            _classGenerator = new CSharpClassGenerator(_config);
            _dbContextGenerator = new DbContextGenerator(_config, _logger);
            _referenceAccessorGenerator = new ReferenceAccessorGenerator(_config, _logger);
        }

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            if (_config == null)
            {
                return;
            }

            if (files.SelectMany(f => f.Classes).Any(c => c.Trigram != null))
            {
                GenerateDbContext();
            }

            foreach (var file in files)
            {
                _files[file.Name] = file;
                GenerateClasses(file);
            }

            var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace)).Distinct();
            foreach (var module in modules)
            {
                GenerateReferences(module);
            }
        }

        private void GenerateDbContext()
        {
            if (_config.DbContextProjectPath != null)
            {
                _dbContextGenerator.Generate(_files.Values.SelectMany(f => f.Classes));
            }
        }

        private void GenerateClasses(ModelFile file)
        {
            if (_config.OutputDirectory == null)
            {
                return;
            }

            _logger.LogInformation($"Génération des classes pour le fichier {file}...");

            var currentDirectory = GetDirectoryForModelClass(
                _config.LegacyProjectPaths,
                _config.OutputDirectory,
                file.Descriptor.Kind == Kind.Data,
                file.Descriptor.App,
                file.Descriptor.Namespace.CSharpName);

            Directory.CreateDirectory(currentDirectory);
            foreach (var classe in file.Classes)
            {
                _classGenerator.Generate(classe);
            }

            _logger.LogInformation($"{file.Classes.Count()} classes générées.");
        }

        private void GenerateReferences(Namespace ns)
        {
            _referenceAccessorGenerator.Generate(
                _files.Values.SelectMany(f => f.Classes)
                    .Where(c => c.Stereotype != null && c.Namespace.Equals(ns)));
        }
    }
}

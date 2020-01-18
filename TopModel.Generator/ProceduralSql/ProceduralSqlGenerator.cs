using System.Linq;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.ProceduralSql
{
    public class ProceduralSqlGenerator : IGenerator
    {
        private readonly ProceduralSqlConfig _config;
        private readonly ModelStore _modelStore;
        private readonly ILogger<ProceduralSqlGenerator> _logger;

        private readonly AbstractSchemaGenerator? _schemaGenerator;

        public ProceduralSqlGenerator(ModelStore modelStore, ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig? config = null)
        {
            _config = config!;
            _logger = logger;
            _modelStore = modelStore;

            var rootNamespace = _modelStore.RootNamespace;

            if (_config != null)
            {
                _schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
                    ? new PostgreSchemaGenerator(rootNamespace, _config, _logger)
                    : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(rootNamespace, _config, _logger);
            }
        }

        public bool CanGenerate => _config != null;

        public string Name => "des scripts de création SQL";

        public void GenerateAll()
        {
            _schemaGenerator?.GenerateSchemaScript(_modelStore.Classes);
            GenerateListInitScript(Stereotype.Statique);
            GenerateListInitScript(Stereotype.Reference);
        }

        public void GenerateFromFile(ModelFile file)
        {
            // Pas de génération unitaire.
        }

        private void GenerateListInitScript(Stereotype stereotype)
        {
            var classes = _modelStore.Classes.Where(c => c.Stereotype == stereotype && c.ReferenceValues != null);
            if (classes.Any())
            {
                _schemaGenerator?.GenerateListInitScript(classes, stereotype == Stereotype.Statique);
            }
        }
    }
}

using TopModel.Core.Config;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.ProceduralSql
{
    public class ProceduralSqlGenerator : IGenerator
    {
        private readonly ProceduralSqlConfig? _config;
        private readonly ModelStore _modelStore;
        private readonly ILogger<ProceduralSqlGenerator> _logger;

        public ProceduralSqlGenerator(ModelStore modelStore, ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig? config = null)
        {
            _config = config;
            _logger = logger;
            _modelStore = modelStore;
        }

        public bool CanGenerate => _config != null;

        public string Name => "des scripts de création SQL";

        public void Generate()
        {
            if (_config == null)
            {
                return;
            }

            var rootNamespace = _modelStore.RootNamespace;

            var schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
                ? new PostgreSchemaGenerator(rootNamespace, _config, _logger)
                : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(rootNamespace, _config, _logger);

            schemaGenerator.GenerateSchemaScript(_modelStore.Classes);
            schemaGenerator.GenerateListInitScript(_modelStore.StaticListsMap, isStatic: true);
            schemaGenerator.GenerateListInitScript(_modelStore.ReferenceListsMap, isStatic: false);
        }
    }
}

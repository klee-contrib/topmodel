using TopModel.Core;
using TopModel.Core.Config;

namespace TopModel.Generator.ProceduralSql
{
    class ProceduralSqlGenerator : IGenerator
    {
        private readonly ProceduralSqlConfig? _config;
        private readonly ModelStore _modelStore;

        public ProceduralSqlGenerator(ModelStore modelStore, ProceduralSqlConfig? config = null)
        {
            _config = config;
            _modelStore = modelStore;
        }

        public void Generate()
        {
            if (_config == null)
            {
                return;
            }

            var rootNamespace = _modelStore.RootNamespace;

            var schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
                ? new PostgreSchemaGenerator(rootNamespace, _config)
                : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(rootNamespace, _config);

            schemaGenerator.GenerateSchemaScript(_modelStore.Classes);
            schemaGenerator.GenerateListInitScript(_modelStore.StaticListsMap, isStatic: true);
            schemaGenerator.GenerateListInitScript(_modelStore.ReferenceListsMap, isStatic: false);
        }
    }
}

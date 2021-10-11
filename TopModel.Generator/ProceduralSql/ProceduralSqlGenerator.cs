using System.Collections.Generic;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.ProceduralSql
{
    public class ProceduralSqlGenerator : GeneratorBase
    {
        private readonly ProceduralSqlConfig _config;
        private readonly ILogger<ProceduralSqlGenerator> _logger;
        private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

        private readonly AbstractSchemaGenerator? _schemaGenerator;

        public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig config)
            : base(logger, config)
        {
            _config = config;
            _logger = logger;
            _schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
                ? new PostgreSchemaGenerator(_config, _logger)
                : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(_config, _logger);
        }

        public override string Name => "ProceduralSqlGen";

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
            }

            _schemaGenerator?.GenerateSchemaScript(_files.Values.SelectMany(f => f.Classes).Distinct());
            GenerateListInitScript();
        }

        private void GenerateListInitScript()
        {
            var classes = _files.Values
                .SelectMany(f => f.Classes)
                .Distinct()
                .Where(c => c.ReferenceValues != null);

            if (classes.Any())
            {
                _schemaGenerator?.GenerateListInitScript(classes);
            }
        }
    }
}

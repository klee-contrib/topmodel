using System.Collections.Generic;
using System.Linq;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.ProceduralSql
{
    public class ProceduralSqlGenerator : IModelWatcher
    {
        private readonly ProceduralSqlConfig _config;
        private readonly ILogger<ProceduralSqlGenerator> _logger;
        private readonly IDictionary<FileName, ModelFile> _files = new Dictionary<FileName, ModelFile>();

        private readonly AbstractSchemaGenerator? _schemaGenerator;

        public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig? config = null)
        {
            _config = config!;
            _logger = logger;

            if (_config != null)
            {
                _schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
                    ? new PostgreSchemaGenerator(_config, _logger)
                    : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(_config, _logger);
            }
        }

        public bool CanGenerate => _config != null;

        public string Name => "des scripts de création SQL";

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
            }

            _schemaGenerator?.GenerateSchemaScript(_files.Values.SelectMany(f => f.Classes));
            GenerateListInitScript(Stereotype.Statique);
            GenerateListInitScript(Stereotype.Reference);
        }

        private void GenerateListInitScript(Stereotype stereotype)
        {
            var classes = _files.Values.SelectMany(f => f.Classes).Where(c => c.Stereotype == stereotype && c.ReferenceValues != null);
            if (classes.Any())
            {
                _schemaGenerator?.GenerateListInitScript(classes, stereotype == Stereotype.Statique);
            }
        }
    }
}

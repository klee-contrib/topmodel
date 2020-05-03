using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using TopModel.Generator.Ssdt.Scripter;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    public class SsdtGenerator : GeneratorBase
    {
        private readonly SsdtConfig _config;
        private readonly ILogger<SsdtGenerator> _logger;
        private readonly IDictionary<FileName, ModelFile> _files = new Dictionary<FileName, ModelFile>();

        private readonly ISqlScripter<Class> _tableScripter = new SqlTableScripter();
        private readonly ISqlScripter<Class> _tableTypeScripter = new SqlTableTypeScripter();
        private readonly ISqlScripter<Class> _initReferenceListScript;
        private readonly ISqlScripter<IEnumerable<Class>> _initReferenceListMainScripter;

        public SsdtGenerator(ILogger<SsdtGenerator> logger, SsdtConfig config)
            : base(config)
        {
            _config = config;
            _logger = logger;

            _initReferenceListScript = new InitReferenceListScripter();
            _initReferenceListMainScripter = new InitReferenceListMainScripter(_config);
        }

        public override string Name => nameof(SsdtGenerator);

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
                GenerateClasses(file);
            }

            GenerateListInitScript();
        }

        private void GenerateClasses(ModelFile file)
        {
            if (_config.TableScriptFolder != null && _config.TableTypeScriptFolder != null)
            {
                var tableCount = 0;
                var tableTypeCount = 0;
                foreach (var classe in file.Classes)
                {
                    tableCount++;
                    _tableScripter.Write(classe, _config.TableScriptFolder, _logger);

                    if (classe.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName))
                    {
                        tableTypeCount++;
                        _tableTypeScripter.Write(classe, _config.TableTypeScriptFolder, _logger);
                    }
                }
            }
        }

        private void GenerateListInitScript()
        {
            var classes = _files.Values.SelectMany(f => f.Classes).Where(c => c.ReferenceValues != null);

            if (!classes.Any() || _config.InitListMainScriptName == null || _config.InitListScriptFolder == null)
            {
                return;
            }

            Directory.CreateDirectory(_config.InitListScriptFolder);

            // Construit la liste des Reference Class ordonnée.
            var orderList = ModelUtils.Sort(classes.OrderBy(c => c.Name), c => c.Properties
                .OfType<AssociationProperty>()
                .Select(a => a.Association)
                .Where(a => a.ReferenceValues != null));

            // Script un fichier par classe.
            foreach (var referenceClass in orderList)
            {
                _initReferenceListScript.Write(referenceClass, _config.InitListScriptFolder, _logger);
            }

            // Script le fichier appelant les fichiers dans le bon ordre.
            _initReferenceListMainScripter.Write(orderList, _config.InitListScriptFolder, _logger);
        }
    }
}

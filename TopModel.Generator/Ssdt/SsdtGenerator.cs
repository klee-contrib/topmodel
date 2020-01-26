using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using TopModel.Generator.Ssdt.Dto;
using TopModel.Generator.Ssdt.Scripter;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    public class SsdtGenerator : IModelWatcher
    {
        private readonly SsdtConfig _config;
        private readonly ILogger<SsdtGenerator> _logger;
        private readonly IDictionary<FileName, ModelFile> _files = new Dictionary<FileName, ModelFile>();

        private readonly ISqlScripter<Class> _tableScripter = new SqlTableScripter();
        private readonly ISqlScripter<Class> _tableTypeScripter = new SqlTableTypeScripter();
        private readonly ISqlScripter<ReferenceClass> _initReferenceListScript;
        private readonly ISqlScripter<ReferenceClassSet> _initReferenceListMainScripter = new InitReferenceListMainScripter();

        public SsdtGenerator(ILogger<SsdtGenerator> logger, SsdtConfig config)
        {
            _config = config;
            _logger = logger;

            _initReferenceListScript = new InitReferenceListScripter(_config);
        }

        public string Name => nameof(SsdtGenerator);

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
                GenerateClasses(file);
            }

            if (files.SelectMany(f => f.Classes).Any(c => c.Stereotype == Stereotype.Statique))
            {
                GenerateListInitScript(Stereotype.Statique);
            }

            if (files.SelectMany(f => f.Classes).Any(c => c.Stereotype == Stereotype.Reference))
            {
                GenerateListInitScript(Stereotype.Reference);
            }
        }

        private void GenerateClasses(ModelFile file)
        {
            if (file.Descriptor.Kind == Kind.Data && _config.TableScriptFolder != null && _config.TableTypeScriptFolder != null)
            {
                var tableCount = 0;
                var tableTypeCount = 0;
                foreach (var classe in file.Classes)
                {
                    if (classe.Trigram != null)
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
        }

        private void GenerateListInitScript(Stereotype stereotype)
        {
            var classes = _files.Values.SelectMany(f => f.Classes).Where(c => c.Stereotype == stereotype && c.ReferenceValues != null);
            var insertScriptFolderPath = stereotype == Stereotype.Statique
                ? _config.InitStaticListScriptFolder
                : _config.InitReferenceListScriptFolder;
            var insertMainScriptName = stereotype == Stereotype.Statique
                ? _config.InitStaticListMainScriptName
                : _config.InitReferenceListMainScriptName;

            if (!classes.Any() || insertMainScriptName == null || insertScriptFolderPath == null)
            {
                return;
            }

            Directory.CreateDirectory(insertScriptFolderPath);

            // Construit la liste des Reference Class ordonnée.
            var orderList = ModelUtils.Sort(classes.OrderBy(c => c.Name), c => c.Properties
                .OfType<AssociationProperty>()
                .Select(a => a.Association)
                .Where(a => a.Stereotype == c.Stereotype));

            var referenceClassList =
                orderList.Select(x => new ReferenceClass
                {
                    Class = x,
                    Values = x.ReferenceValues,
                    IsStatic = stereotype == Stereotype.Statique
                }).ToList();
            var referenceClassSet = new ReferenceClassSet
            {
                ClassList = orderList.ToList(),
                ScriptName = insertMainScriptName
            };

            // Script un fichier par classe.
            foreach (var referenceClass in referenceClassList)
            {
                _initReferenceListScript.Write(referenceClass, insertScriptFolderPath, _logger);
            }

            // Script le fichier appelant les fichiers dans le bon ordre.
            _initReferenceListMainScripter.Write(referenceClassSet, insertScriptFolderPath, _logger);
        }
    }
}

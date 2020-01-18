using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using TopModel.Generator.Ssdt.Dto;
using TopModel.Generator.Ssdt.Scripter;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    public class SsdtGenerator : IGenerator
    {
        private readonly SsdtConfig _config;
        private readonly ILogger<SsdtGenerator> _logger;
        private readonly ModelStore _modelStore;

        private readonly ISqlScripter<Class> _tableScripter = new SqlTableScripter();
        private readonly ISqlScripter<Class> _tableTypeScripter = new SqlTableTypeScripter();
        private readonly ISqlScripter<ReferenceClass> _initReferenceListScript;
        private readonly ISqlScripter<ReferenceClassSet> _initReferenceListMainScripter = new InitReferenceListMainScripter();

        public SsdtGenerator(ModelStore modelStore, ILogger<SsdtGenerator> logger, SsdtConfig? config = null)
        {
            _config = config!;
            _logger = logger;
            _modelStore = modelStore;

            _initReferenceListScript = new InitReferenceListScripter(_config);
        }

        public bool CanGenerate => _config != null;

        public string Name => "du modèle SSDT";

        public void GenerateAll()
        {
            foreach (var file in _modelStore.Files)
            {
                GenerateFromFile(file);
            }

            GenerateListInitScript(Stereotype.Statique);
            GenerateListInitScript(Stereotype.Reference);
        }

        public void GenerateFromFile(ModelFile file)
        {
            if (file.Descriptor.Kind == Kind.Data && _config.TableScriptFolder != null && _config.TableTypeScriptFolder != null)
            {
                _logger.LogInformation($"Génération des scripts de tables du fichier {file}...");

                var tableCount = 0;
                var tableTypeCount = 0;
                foreach (var classe in file.Classes)
                {
                    if (classe.Trigram != null)
                    {
                        tableCount++;
                        _tableScripter.Write(classe, _config.TableScriptFolder);

                        if (classe.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName))
                        {
                            tableTypeCount++;
                            _tableTypeScripter.Write(classe, _config.TableTypeScriptFolder);
                        }
                    }
                }

                _logger.LogInformation($"{tableCount} scripts de tables et {tableTypeCount} scripts de types de tables générés.");
            }
        }

        private void GenerateListInitScript(Stereotype stereotype)
        {
            var classes = _modelStore.Classes.Where(c => c.Stereotype == stereotype && c.ReferenceValues != null);
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

            _logger.LogInformation($"Génération des scripts d'initialisation du répertoire {insertScriptFolderPath.Split("\\").Last()}...");
            Directory.CreateDirectory(insertScriptFolderPath);

            // Construit la liste des Reference Class ordonnée.
            var orderList = ModelUtils.Sort(classes, c => c.Properties
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
                _initReferenceListScript.Write(referenceClass, insertScriptFolderPath);
            }

            // Script le fichier appelant les fichiers dans le bon ordre.
            _initReferenceListMainScripter.Write(referenceClassSet, insertScriptFolderPath);

            _logger.LogInformation($"{referenceClassList.Count} scripts d'initialisation générés.");
        }
    }
}

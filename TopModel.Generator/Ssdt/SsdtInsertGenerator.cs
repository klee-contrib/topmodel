using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using TopModel.Generator.Ssdt.Dto;
using TopModel.Generator.Ssdt.Scripter;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    using static SqlScriptEngine;

    /// <summary>
    /// Générateur de Transact-SQL (insertions de données) visant une structure de fichiers SSDT.
    /// </summary>
    public static class SsdtInsertGenerator
    {
        /// <summary>
        /// Génère le script SQL d'initialisation des listes reference.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="classes">Classes avec des initilisations de listes statiques.</param>
        /// <param name="insertScriptFolderPath">Chemin du dossier contenant les scripts.</param>        
        /// <param name="insertMainScriptName">Nom du script principal.</param>
        /// <param name="isStatic">True if generation for static list.</param>
        public static void GenerateListInitScript(SsdtConfig config, ILogger<SsdtGenerator> logger, IEnumerable<Class> classes, string insertScriptFolderPath, string insertMainScriptName, bool isStatic)
        {
            if (!classes.Any())
            {
                return;
            }

            logger.LogInformation($"Génération des scripts d'initialisation du répertoire {insertScriptFolderPath.Split("\\").Last()}...");
            Directory.CreateDirectory(insertScriptFolderPath);

            // Construit la liste des Reference Class ordonnée.
            var orderList = ModelUtils.Sort(classes, c => c.Properties
                .OfType<AssociationProperty>()
                .Select(a => a.Association)
                .Where(a  => a.Stereotype == c.Stereotype));

            var referenceClassList =
                orderList.Select(x => new ReferenceClass
                {
                    Class = x,
                    Values = x.ReferenceValues,
                    IsStatic = isStatic
                }).ToList();
            var referenceClassSet = new ReferenceClassSet
            {
                ClassList = orderList.ToList(),
                ScriptName = insertMainScriptName
            };

            // Script un fichier par classe.            
            Write(new InitReferenceListScripter(config), referenceClassList, insertScriptFolderPath);

            // Script le fichier appelant les fichiers dans le bon ordre.
            Write(new InitReferenceListMainScripter(), referenceClassSet, insertScriptFolderPath);

            logger.LogInformation($"{referenceClassList.Count} scripts d'initialisation générés.");
        }       
    }
}

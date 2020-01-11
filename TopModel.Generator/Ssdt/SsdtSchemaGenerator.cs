using System.Collections.Generic;
using System.Linq;
using TopModel.Generator.Ssdt.Scripter;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    using static SqlScriptEngine;

    /// <summary>
    /// Générateur de Transact-SQL (structure) visant une structure de fichiers SSDT.
    /// </summary>
    public static class SsdtSchemaGenerator
    {
        /// <summary>
        /// Génère le script SQL.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="classes">Classes.</param>
        /// <param name="tableScriptFolder">Dossier contenant les fichiers de script des tables.</param>
        /// <param name="tableTypeScriptFolder">Dossier contenant les fichiers de script des types de table.</param>
        public static void GenerateSchemaScript(ILogger<SsdtGenerator> logger, IEnumerable<Class> classes, string tableScriptFolder, string tableTypeScriptFolder)
        {
            // Charge le modèle issu de l'OOM.
            var tableList = classes.Where(x => x.Trigram != null).ToList();
            var tableTypeList = tableList.Where(item => item.Properties.Where(p => p.Name == ScriptUtils.InsertKeyName).Any()).ToList();

            // Script de table.
            logger.LogInformation("Génération des scripts de tables...");
            Write(new SqlTableScripter(), tableList, tableScriptFolder);
            logger.LogInformation($"{tableList.Count} scripts de tables générés.");

            // Script de type table.
            logger.LogInformation("Génération des scripts de types de tables...");
            Write(new SqlTableTypeScripter(), tableTypeList, tableTypeScriptFolder);
            logger.LogInformation($"{tableTypeList.Count} scripts de types de tables générés.");
        }
    }
}

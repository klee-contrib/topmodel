using System;
using System.Collections.Generic;
using System.Linq;
using Kinetix.NewGenerator.Config;
using Kinetix.NewGenerator.Model;
using Kinetix.NewGenerator.Ssdt.Contract;
using Kinetix.NewGenerator.Ssdt.Scripter;

namespace Kinetix.NewGenerator.Ssdt
{
    /// <summary>
    /// Générateur de Transact-SQL (structure) visant une structure de fichiers SSDT.
    /// </summary>
    public class SqlServerSsdtSchemaGenerator : ISqlServerSsdtSchemaGenerator
    {
        private readonly ISqlScriptEngine _engine;

        public SqlServerSsdtSchemaGenerator()
        {
            _engine = new SqlScriptEngine();
        }

        /// <summary>
        /// Génère le script SQL.
        /// </summary>
        /// <param name="classes">Classes.</param>
        /// <param name="tableScriptFolder">Dossier contenant les fichiers de script des tables.</param>
        /// <param name="tableTypeScriptFolder">Dossier contenant les fichiers de script des types de table.</param>
        public void GenerateSchemaScript(IEnumerable<Class> classes, string tableScriptFolder, string tableTypeScriptFolder)
        {
            Console.Out.WriteLine("Generating schema script");

            // Charge le modèle issu de l'OOM.
            var tableList = new List<Class>();
            InitCollection(classes, tableList);

            // Script de table.
            _engine.Write(new SqlTableScripter(), tableList, tableScriptFolder);

            // Script de type table.
            _engine.Write(new SqlTableTypeScripter(), tableList, tableTypeScriptFolder);
        }

        /// <summary>
        /// Initialise les collections.
        /// </summary>
        /// <param name="classes">Classes.</param>
        /// <param name="tableList">Listes de tables.</param>
        private static void InitCollection(IEnumerable<Class> classes, List<Class> tableList)
        {
            // Sélection des classes avec persistance.
            tableList.AddRange(classes.Where(x => x.Trigram != null));
        }
    }
}

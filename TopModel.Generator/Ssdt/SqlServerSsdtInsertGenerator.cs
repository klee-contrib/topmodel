using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core;
using TopModel.Core.Config;
using TopModel.Generator.Ssdt.Contract;
using TopModel.Generator.Ssdt.Dto;
using TopModel.Generator.Ssdt.Scripter;

namespace TopModel.Generator.Ssdt
{
    /// <summary>
    /// Générateur de Transact-SQL (insertions de données) visant une structure de fichiers SSDT.
    /// </summary>
    public class SqlServerSsdtInsertGenerator : ISqlServerSsdtInsertGenerator
    {
        private readonly ISqlScriptEngine _engine;
        private readonly SsdtConfig _config;

        public SqlServerSsdtInsertGenerator(SsdtConfig config)
        {
            _config = config;
            _engine = new SqlScriptEngine();
        }

        /// <summary>
        /// Génère le script SQL d'initialisation des listes reference.
        /// </summary>
        /// <param name="initDictionary">Dictionnaire des initialisations.</param>
        /// <param name="insertScriptFolderPath">Chemin du dossier contenant les scripts.</param>        
        /// <param name="insertMainScriptName">Nom du script principal.</param>
        /// <param name="outputDeltaFileName">Nom du fichier de delta généré.</param>
        /// <param name="isStatic">True if generation for static list.</param>
        public void GenerateListInitScript(IDictionary<Class, IEnumerable<ReferenceValue>> initDictionary, string insertScriptFolderPath, string insertMainScriptName, string outputDeltaFileName, bool isStatic)
        {
            if (!initDictionary.Any())
            {
                return;
            }

            Console.Out.WriteLine("Generating init script " + insertScriptFolderPath);
            Directory.CreateDirectory(insertScriptFolderPath);

            // Construit la liste des Reference Class ordonnée.
            var orderList = OrderStaticTableList(initDictionary).ToArray();
            var referenceClassList =
                orderList.Select(x => new ReferenceClass
                {
                    Class = x,
                    Values = initDictionary[x],
                    IsStatic = isStatic
                }).ToList();
            var referenceClassSet = new ReferenceClassSet
            {
                ClassList = orderList.ToList(),
                ScriptName = insertMainScriptName
            };

            // Script un fichier par classe.            
            _engine.Write(new InitReferenceListScripter(_config), referenceClassList, insertScriptFolderPath);

            // Script le fichier appelant les fichiers dans le bon ordre.
            _engine.Write(new InitReferenceListMainScripter(), referenceClassSet, insertScriptFolderPath);

            // TODO : delta ?
        }

        /// <summary>
        /// Retourne un tableau ordonné des ModelClass pour gérer les FK entre les listes statiques.
        /// </summary>
        /// <param name="dictionnary">Dictionnaire des couples (ModelClass, StaticTableInit) correspondant aux tables de listes statiques. </param>
        /// <returns>ModelClass[] ordonné.</returns>
        private static Class[] OrderStaticTableList(IDictionary<Class, IEnumerable<ReferenceValue>> dictionnary)
        {
            var nbTable = dictionnary.Count;
            var orderedList = new Class[nbTable];
            dictionnary.Keys.CopyTo(orderedList, 0);

            var i = 0;
            while (i < nbTable)
            {
                var canIterate = true;
                var currentModelClass = orderedList[i];

                // On récupère les ModelClass des tables pointées par la table
                // On récupère les ModelClass des tables pointées par la table
                var pointedTableSet = new HashSet<Class>(
                    currentModelClass.Properties.OfType<AssociationProperty>()
                        .Select(p => p.Association));

                for (var j = i + 1; j < nbTable; j++)
                {
                    if (pointedTableSet.Contains(orderedList[j]))
                    {
                        var sauvegarde = orderedList[i];
                        orderedList[i] = orderedList[j];
                        orderedList[j] = sauvegarde;
                        canIterate = false;
                        break;
                    }
                }

                if (canIterate)
                {
                    i++;
                }
            }

            return orderedList;
        }
    }
}

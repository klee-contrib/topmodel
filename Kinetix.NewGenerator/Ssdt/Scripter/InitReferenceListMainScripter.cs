using System;
using System.IO;
using Kinetix.NewGenerator.Ssdt.Contract;
using Kinetix.NewGenerator.Ssdt.Dto;

namespace Kinetix.NewGenerator.Ssdt.Scripter
{
    /// <summary>
    /// Scripter écrivant un script qui ordonnance l'appel aux scripts d'insertions de valeurs de listes de références.
    /// </summary>
    public class InitReferenceListMainScripter : ISqlScripter<ReferenceClassSet>
    {
        /// <summary>
        /// Calcule le nom du script pour l'item.
        /// </summary>
        /// <param name="item">Item à scripter.</param>
        /// <returns>Nom du fichier de script.</returns>
        public string GetScriptName(ReferenceClassSet item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.ScriptName;
        }

        /// <summary>
        /// Indique si l'item doit générer un script.
        /// </summary>
        /// <param name="item">Item candidat.</param>
        /// <returns><code>True</code> si un script doit être généré.</returns>
        public bool IsScriptGenerated(ReferenceClassSet item)
        {
            return true;
        }

        /// <summary>
        /// Ecrit dans un flux le script pour l'item.
        /// </summary>
        /// <param name="writer">Flux d'écriture.</param>
        /// <param name="item">Table à scripter.</param>
        public void WriteItemScript(TextWriter writer, ReferenceClassSet item)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            // Entête du fichier.
            WriteHeader(writer);

            // Appel des scripts d'insertion.
            WriteScriptCalls(writer, item);
        }

        /// <summary>
        /// Ecrit l'entête du fichier.
        /// </summary>
        /// <param name="writer">Flux.</param>
        private static void WriteHeader(TextWriter writer)
        {
            writer.WriteLine("-- ===========================================================================================");
            writer.WriteLine("--   Description		:	Insertion des valeurs de listes statiques.");
            writer.WriteLine("-- ===========================================================================================");
            writer.WriteLine();
        }

        /// <summary>
        /// Ecrit les appels de scripts.
        /// </summary>
        /// <param name="writer">Flux.</param>
        /// <param name="classSet">Ensemble des listes de référence.</param>
        private static void WriteScriptCalls(TextWriter writer, ReferenceClassSet classSet)
        {
            foreach (var classe in classSet.ClassList)
            {
                var subscriptName = classe.SqlName + ".insert.sql";
                writer.WriteLine("/* Insertion dans la table " + classe.SqlName + ". */");
                writer.WriteLine(":r .\\" + subscriptName);
                writer.WriteLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Kinetix.NewGenerator.Ssdt.Contract;

namespace Kinetix.NewGenerator.Ssdt
{
    /// <summary>
    /// Moteur permettant d'écrire une série de scripts SQL dans un dossier à partir d'une liste d'items et d'un scripter.
    /// </summary>
    public class SqlScriptEngine : ISqlScriptEngine
    {
        /// <summary>
        /// Ecrit les fichiers pour une liste d'items dans un dossier donné à l'aide du scripter.
        /// </summary>
        /// <param name="scripter">Scripter indiquant l'implémentation de script.</param>
        /// <param name="itemList">Liste des items.</param>
        /// <param name="folderPath">Dossier cible pour les scripts.</param>
        /// <typeparam name="T">Type de l'item à scripter.</typeparam>
        public void Write<T>(ISqlScripter<T> scripter, IList<T> itemList, string folderPath)
        {
            if (scripter == null)
            {
                throw new ArgumentNullException("scripter");
            }

            if (itemList == null)
            {
                throw new ArgumentNullException("itemList");
            }

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentNullException("folderPath");
            }

            foreach (var item in itemList)
            {
                WriteCore(scripter, item, folderPath);
            }
        }

        /// <summary>
        /// Ecrit un fichier pour un item dans un dossier.
        /// </summary>
        /// <param name="scripter">Scripter indiquant l'implémentation de script.</param>
        /// <param name="item">Item.</param>
        /// <param name="folderPath">Dossier cible pour le script.</param>
        /// <typeparam name="T">Type de l'item à scripter.</typeparam>
        public void Write<T>(ISqlScripter<T> scripter, T item, string folderPath)
        {
            if (scripter == null)
            {
                throw new ArgumentNullException("scripter");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentNullException("folderPath");
            }

            WriteCore(scripter, item, folderPath);
        }

        /// <summary>
        /// Ecrit un fichier pour un item.
        /// </summary>
        /// <param name="scripter">Scripter indiquant l'implémentation de script.</param>
        /// <param name="item">Item.</param>
        /// <param name="folderPath">Dossier cible pour le script.</param>
        /// <typeparam name="T">Type de l'item à scripter.</typeparam>
        private void WriteCore<T>(ISqlScripter<T> scripter, T item, string folderPath)
        {
            // Filtrage des items à scripter.
            if (!scripter.IsScriptGenerated(item))
            {
                return;
            }

            // Génére le nom du fichier.
            var scriptName = scripter.GetScriptName(item);

            // Chemin complet du fichier.
            var scriptPath = Path.Combine(folderPath, scriptName);

            // Utilisation du flux spécial qui ne checkout le fichier que s'il est modifié.
            using var tw = new SqlFileWriter(scriptPath);

            /*  Génére le script de l'item */
            scripter.WriteItemScript(tw, item);
        }
    }
}

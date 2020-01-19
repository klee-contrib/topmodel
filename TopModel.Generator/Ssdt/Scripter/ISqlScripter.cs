using System.IO;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt.Scripter
{
    /// <summary>
    /// Interface des implémentations spécifiques qui scriptent de fichiers SQL.
    /// </summary>
    /// <typeparam name="T">Type e l'item scripé.</typeparam>
    public interface ISqlScripter<T>
    {
        /// <summary>
        /// Calcule le nom du script pour l'item.
        /// </summary>
        /// <param name="item">Item à scripter.</param>
        /// <returns>Nom du fichier de script.</returns>
        string GetScriptName(T item);

        /// <summary>
        /// Ecrit dans un flux le script pour l'item.
        /// </summary>
        /// <param name="writer">Flux d'écriture.</param>
        /// <param name="item">Table à scripter.</param>
        void WriteItemScript(TextWriter writer, T item);

        void Write(T item, string folderPath, ILogger logger)
        {
            // Génére le nom du fichier.
            var scriptName = GetScriptName(item);

            // Chemin complet du fichier.
            var scriptPath = Path.Combine(folderPath, scriptName);

            // Utilisation du flux spécial qui ne checkout le fichier que s'il est modifié.
            using var tw = new SqlFileWriter(scriptPath, logger);

            /*  Génére le script de l'item */
            WriteItemScript(tw, item);
        }
    }
}

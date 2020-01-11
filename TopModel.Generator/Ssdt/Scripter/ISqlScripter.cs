using System.IO;

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
    }
}

using System.IO;

namespace TopModel.Generator.Ssdt.Contract
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
        /// Indique si l'item doit générer un script.
        /// </summary>
        /// <param name="item">Item candidat.</param>
        /// <returns><code>True</code> si un script doit être généré.</returns>
        bool IsScriptGenerated(T item);

        /// <summary>
        /// Ecrit dans un flux le script pour l'item.
        /// </summary>
        /// <param name="writer">Flux d'écriture.</param>
        /// <param name="item">Table à scripter.</param>
        void WriteItemScript(TextWriter writer, T item);
    }
}

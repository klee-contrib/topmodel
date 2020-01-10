using System.Collections.Generic;

namespace Kinetix.NewGenerator.Ssdt.Contract
{
    /// <summary>
    /// Contrat du moteur permettant d'écrire une série de scripts SQL dans un dossier à partir d'une liste d'items et d'un scripter.
    /// </summary>
    public interface ISqlScriptEngine
    {
        /// <summary>
        /// Ecrit les fichiers pour une liste d'items dans un dossier donné à l'aide du scripter.
        /// </summary>
        /// <param name="scripter">Scripter indiquant l'implémentation de script.</param>
        /// <param name="itemList">Liste des items.</param>
        /// <param name="folderPath">Dossier cible pour les scripts.</param>
        /// <typeparam name="T">Type de l'item à scripter.</typeparam>
        void Write<T>(ISqlScripter<T> scripter, IList<T> itemList, string folderPath);

        /// <summary>
        /// Ecrit un fichier pour un item dans un dossier.
        /// </summary>
        /// <param name="scripter">Scripter indiquant l'implémentation de script.</param>
        /// <param name="item">Item.</param>
        /// <param name="folderPath">Dossier cible pour le script.</param>
        /// <typeparam name="T">Type de l'item à scripter.</typeparam>
        void Write<T>(ISqlScripter<T> scripter, T item, string folderPath);
    }
}

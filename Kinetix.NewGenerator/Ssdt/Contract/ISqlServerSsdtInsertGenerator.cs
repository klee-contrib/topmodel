using System.Collections.Generic;
using Kinetix.NewGenerator.Model;

namespace Kinetix.NewGenerator.Ssdt.Contract
{
    /// <summary>
    /// Contrat du générateur de Transact-SQL (insertions de données) visant une structure de fichiers SSDT.
    /// </summary>
    public interface ISqlServerSsdtInsertGenerator
    {
        /// <summary>
        /// Génère le script SQL d'initialisation des listes reference.
        /// </summary>
        /// <param name="initDictionary">Dictionnaire des initialisations.</param>
        /// <param name="insertScriptFolderPath">Chemin du dossier contenant les scripts.</param>
        /// <param name="insertMainScriptName">Nom du script principal.</param>
        /// <param name="outputDeltaFileName">Nom du fichier de delta généré.</param>
        /// <param name="isStatic">True if generation for static list.</param>
        void GenerateListInitScript(IDictionary<Class, IEnumerable<ReferenceValue>> initDictionary, string insertScriptFolderPath, string insertMainScriptName, string outputDeltaFileName, bool isStatic);
    }
}

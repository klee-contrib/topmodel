namespace TopModel.Generator.Ssdt
{
    /// <summary>
    /// Paramètres pour la génération SSDT.
    /// </summary>
    public class SsdtConfig
    {
        /// <summary>
        /// Dossier du projet pour les scripts de déclaration de table.
        /// </summary>
        public string? TableScriptFolder { get; set; }

        /// <summary>
        /// Dossier du projet pour les scripts de déclaration de type table.
        /// </summary>
        public string? TableTypeScriptFolder { get; set; }

        /// <summary>
        /// Dossier du projet pour les scripts d'initialisation des listes statiques.
        /// </summary>
        public string? InitListScriptFolder { get; set; }

        /// <summary>
        /// Fichier du projet référençant les scripts d'initialisation des listes statiques.
        /// </summary>
        public string? InitListMainScriptName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de la table où stocker l'historique de passage des scripts.
        /// </summary>
        public string LogScriptTableName { get; set; } = "SCRIPT_HISTORIQUE";

        /// <summary>
        /// Obtient ou définit le nom du champ où stocker le nom des scripts exécutés.
        /// </summary>
        public string LogScriptVersionField { get; set; } = "SHI_VERSION";

        /// <summary>
        /// Obtient ou définit le nom du champ où stocker la date d'exécution des scripts.
        /// </summary>
        public string LogScriptDateField { get; set; } = "SHI_DATE";
    }
}

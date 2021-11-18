namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Paramètres pour la génération du Javascript.
    /// </summary>
    public class JavascriptConfig : GeneratorConfigBase
    {
        /// <summary>
        /// Dossier de sortie pour le modèle.
        /// </summary>
        public string? ModelOutputDirectory { get; set; }

        /// <summary>
        /// Dossier de sortie pour les ressources.
        /// </summary>
        public string? ResourceOutputDirectory { get; set; }

        /// <summary>
        /// Dossier de sortie pour le client d'API.
        /// </summary>
        public string? ApiClientOutputDirectory { get; set; }

        /// <summary>
        /// Chemin vers un "fetch" personnalisé.
        /// </summary>
        public string? FetchImportPath { get; set; }

        /// <summary>
        /// Génère des modèles pour Focus (par défaut : true).
        /// </summary>
        public bool Focus { get; set; } = true;

        /// <summary>
        /// Mode de génération (JS ou JSON Schema).
        /// </summary>
        public ResourceMode ResourceMode { get; set; }
    }
}

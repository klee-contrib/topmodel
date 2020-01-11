namespace TopModel.Core.Config
{
    /// <summary>
    /// Paramètres pour la génération du Javascript.
    /// </summary>
    public class JavascriptConfig
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
        /// If should generate entities in JS.
        /// </summary>
        public bool IsGenerateEntities { get; set; } = true;
    }
}

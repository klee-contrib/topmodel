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
    }
}

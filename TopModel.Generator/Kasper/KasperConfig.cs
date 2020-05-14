#nullable disable
namespace TopModel.Generator.Kasper
{
    public class KasperConfig : GeneratorConfigBase
    {
        /// <summary>
        /// Racine du répertoire où sont situées les sources Java.
        /// </summary>
        public string SourcesDirectory { get; set; }

        /// <summary>
        /// Précise le nom du package dans lequel générer les classes.
        /// </summary>
        public string PackageName { get; set; }
    }
}
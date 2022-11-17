#nullable disable

namespace TopModel.Core;

public class LangConfig
{
    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public List<string> Langs { get; set; } = new();

    /// <summary>
    /// Chemin de la génération des fichiers langues manquantes
    /// </summary>
    public string RootPath { get; set; } = "{lang}";
}

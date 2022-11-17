#nullable disable

namespace TopModel.Generator.Translation;

public class TranslationConfig : GeneratorConfigBase
{
    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public Dictionary<string, string> Langs { get; set; }
}
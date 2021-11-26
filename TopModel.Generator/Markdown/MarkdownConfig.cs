#nullable disable

namespace TopModel.Generator.Markdown;

public class MarkdownConfig : GeneratorConfigBase
{
    /// <summary>
    /// Dossier de sortie pour le modèle.
    /// </summary>
    public string DocOutputDirectory { get; set; }

    /// <summary>
    /// Generate not persisted objects
    /// </summary>
    public bool GenerateNotPersisted { get; set; }
}
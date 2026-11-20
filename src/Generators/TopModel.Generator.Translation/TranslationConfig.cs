using TopModel.Generator.Core;

namespace TopModel.Generator.Translation;

public class TranslationConfig : GeneratorConfigBase
{
    /// <summary>
    /// Langues cibles pour lesquelles les traductions manquantes doivent être générées.
    /// </summary>
    public virtual IList<string> Langs { get; set; } = [];

    /// <summary>
    /// Chemin des dossiers de traductions sortants (manquants).
    /// </summary>
    public virtual string RootPath { get; set; } = "{lang}";

    public override string[] PropertiesWithTagVariableSupport =>
        [.. base.PropertiesWithTagVariableSupport, nameof(RootPath)];

    public override string[] PropertiesWithLangVariableSupport =>
        [.. base.PropertiesWithLangVariableSupport, nameof(RootPath)];
}

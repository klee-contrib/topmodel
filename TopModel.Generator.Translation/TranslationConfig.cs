using TopModel.Generator.Core;

namespace TopModel.Generator.Translation;

public class TranslationConfig : GeneratorConfigBase
{
    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public List<string> Langs { get; set; } = [];

    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public string RootPath { get; set; } = "{lang}";

    public override string[] PropertiesWithTagVariableSupport => [nameof(RootPath)];

    public override string[] PropertiesWithLangVariableSupport => [nameof(RootPath)];

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        throw new NotImplementedException();
    }
}
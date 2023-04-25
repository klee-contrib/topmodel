#nullable disable

using TopModel.Generator.Core;

namespace TopModel.Generator.Translation;

public class TranslationConfig : GeneratorConfigBase
{
    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public List<string> Langs { get; set; } = new();

    /// <summary>
    /// liste des langues de l'application
    /// </summary>
    public string RootPath { get; set; } = "{lang}";

    public override string[] PropertiesWithTagVariableSupport => new[]
    {
        nameof(RootPath)
    };

    public override string[] PropertiesWithLangVariableSupport => new[]
    {
        nameof(RootPath)
    };

    public override string GetEnumType(string className, string propName, bool asList = false, bool isPrimaryKeyDef = false)
    {
        throw new NotImplementedException();
    }

    public override string GetListType(string name, bool useIterable = true)
    {
        throw new NotImplementedException();
    }
}
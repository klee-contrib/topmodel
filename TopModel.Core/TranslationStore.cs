namespace TopModel.Core;

public class TranslationStore
{
    public Dictionary<string, Dictionary<string, string>> Translations { get; } = new();

    public string GetTranslation(IFieldProperty property, string lang)
    {
        return Translations.TryGetValue(lang, out var dict)
            && dict.TryGetValue(property.ResourceKey, out var translatedValue)
            ? translatedValue : property.Label ?? string.Empty;
    }

    public string GetTranslation(ClassValue refValue, string lang)
    {
        return Translations.TryGetValue(lang, out var dict) && dict.TryGetValue(refValue.ResourceKey, out var translatedValue) ? translatedValue : refValue.Value[refValue.Class.DefaultProperty!];
    }
}

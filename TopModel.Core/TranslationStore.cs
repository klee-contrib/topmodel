using TopModel.Core.Model;

namespace TopModel.Core;

public class TranslationStore
{
    public IDictionary<string, IDictionary<string, string>> Translations { get; } =
        new Dictionary<string, IDictionary<string, string>>();

    public string GetTranslation(IProperty property, string lang)
    {
        return
            Translations.TryGetValue(lang, out var dict)
            && dict.TryGetValue(property.ResourceKey, out var translatedValue)
            ? translatedValue
            : property.Label ?? string.Empty;
    }

    public string GetTranslation(ClassValue refValue, string lang)
    {
        return
            Translations.TryGetValue(lang, out var dict)
            && dict.TryGetValue(refValue.ResourceKey, out var translatedValue)
            ? translatedValue
            : refValue.Value[refValue.Class.DefaultProperty!];
    }
}

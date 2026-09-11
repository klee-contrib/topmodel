using TopModel.Core.Model;

namespace TopModel.Core;

public class TranslationStore(ModelConfig config)
{
    private readonly object _lock = new();

    public IDictionary<string, Dictionary<string, string>> Translations { get; } =
        new Dictionary<string, Dictionary<string, string>>();

    public bool AllowPropertyLabelFallback => config.I18n.AllowPropertyLabelFallback;

    public string? GetTranslation(IProperty property, string lang)
    {
        var label =
            Translations.TryGetValue(lang, out var dict)
            && dict.TryGetValue(property.ResourceKey, out var translatedValue)
                ? translatedValue
                : property.Label;

        if (label == null && config.I18n.AllowPropertyLabelFallback)
        {
            return property.Name;
        }

        return label;
    }

    public string GetTranslation(ClassValue refValue, string lang)
    {
        return
            Translations.TryGetValue(lang, out var dict)
            && dict.TryGetValue(refValue.ResourceKey, out var translatedValue)
            ? translatedValue
            : refValue.Value[refValue.Class.DefaultProperty!];
    }

    internal void AddTranslation(string lang, string resourceKey, string value)
    {
        lock (_lock)
        {
            Translations[lang][resourceKey] = value;
        }
    }
}

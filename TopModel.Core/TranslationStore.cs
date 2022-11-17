namespace TopModel.Core;

public class TranslationStore
{
    public Dictionary<string, Dictionary<string, string>> Translations { get; } = new();
}

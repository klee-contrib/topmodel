using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class References : List<(Reference Reference, ModelFile File)>
{
    public References(object? objet, IEnumerable<(Reference Reference, ModelFile File)> enumerable)
        : base(enumerable)
    {
        Objet = objet ?? new();
    }

    public object Objet { get; }
}

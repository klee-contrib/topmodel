using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class Locations(object? objet, IEnumerable<(Location Location, ModelFile File)> enumerable)
    : List<(Location Location, ModelFile File)>(enumerable)
{
    public object Objet { get; } = objet ?? new();
}

using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

public static class OmnisharpExtensions
{
    public static OmniSharp.Extensions.LanguageServer.Protocol.Models.Range? ToRange(this Reference? loc)
    {
        return loc == null
            ? null
            : new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                loc.Start.Line - 1,
                loc.Start.Column - 1,
                loc.End.Line - 1,
                loc.End.Column - 1);
    }

    public static string GetFilePath(this ILanguageServerFacade facade, ModelFile file)
    {
        return facade.Workspace.ClientSettings.RootPath + file.Path[1..];
    }


    public static IEnumerable<(Reference Reference, ModelFile File)>? GetReferencesForPositionInFile(this ModelStore modelStore, Position position, ModelFile file)
    {
        object? objet = null;

        var matchedReference = file.References.Keys.SingleOrDefault(reference =>
            reference.Start.Line - 1 <= position.Line && position.Line <= reference.End.Line - 1
            && reference.Start.Column - 1 <= position.Character && position.Character <= reference.End.Column - 1);

        if (matchedReference != null)
        {
            objet = file.References[matchedReference];
        }
        else
        {
            objet =
                (object?)file.Classes.SingleOrDefault(c => c.Name.GetLocation()!.Start.Line - 1 == position.Line || c.GetLocation()!.Start.Line - 1 == position.Line)
                ?? (object?)file.Domains.SingleOrDefault(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line || d.GetLocation()!.Start.Line - 1 == position.Line)
                ?? file.Decorators.SingleOrDefault(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line || d.GetLocation()!.Start.Line - 1 == position.Line);
        }

        return objet switch
        {
            Class classe => new[] { (Reference: classe.Name.GetLocation()!, File: classe.GetFile()!) }
                .Concat(modelStore.GetClassReferences(classe).Select(c => (Reference: (Reference)c.Reference, c.File))),
            Domain domain => new[] { (Reference: domain.Name.GetLocation()!, File: domain.GetFile()!) }
                .Concat(modelStore.GetDomainReferences(domain).Select(d => (Reference: (Reference)d.Reference, d.File))),
            Decorator decorator => new[] { (Reference: decorator.Name.GetLocation()!, File: decorator.GetFile()!) }
                .Concat(modelStore.GetDecoratorReferences(decorator).Select(d => (Reference: (Reference)d.Reference, d.File))),
            _ => null
        };
    }

    public static bool ShouldMatch(this string word, string otherword)
    {
        if (string.IsNullOrWhiteSpace(otherword))
        {
            return true;
        }

        var currentIndex = 0;

        foreach (var character in otherword.ToLower())
        {
            if ((currentIndex = word.ToLower().IndexOf(character, currentIndex)) == -1)
            {
                return false;
            }
        }

        return true;
    }
}

using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public static class OmnisharpExtensions
{
    public static string GetFilePath(this ILanguageServerFacade facade, ModelFile file)
    {
        return facade.Workspace.ClientSettings.RootPath + file.Path[1..].Replace('/', Path.DirectorySeparatorChar);
    }

    public static string? GetName(this object objet)
    {
        return objet switch
        {
            Class classe => classe.Name,
            Domain domain => domain.Name,
            Decorator decorator => decorator.Name,
            DataFlow dataFlow => dataFlow.Name,
            AliasProperty property => property.OriginalProperty?.Name ?? property.Name,
            IProperty property => property.Name,
            _ => null
        };
    }

    public static References? GetReferencesForPositionInFile(this ModelStore modelStore, Position position, ModelFile file, bool includeTransitive = false)
    {
        object? referencedObject = null;

        var matchedReference = file.References.Keys.SingleOrDefault(reference =>
            reference.Start.Line - 1 <= position.Line && position.Line <= reference.End.Line - 1
            && reference.Start.Column - 1 <= position.Character && position.Character <= reference.End.Column - 1);

        if (matchedReference != null)
        {
            referencedObject = file.References[matchedReference];
        }

        var definedObjects = file.Classes.Where(c => c.Name.GetLocation()!.Start.Line - 1 == position.Line || c.GetLocation()!.Start.Line - 1 == position.Line).Cast<object>()
            .Concat(file.Domains.Where(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line || d.GetLocation()!.Start.Line - 1 == position.Line).Cast<object>())
            .Concat(file.Decorators.Where(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line || d.GetLocation()!.Start.Line - 1 == position.Line).Cast<object>())
            .Concat(file.DataFlows.Where(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line || d.GetLocation()!.Start.Line - 1 == position.Line).Cast<object>())
            .Concat(file.Properties.Where(p => p.GetLocation()!.Start.Line - 1 == position.Line));

        var definedObject = definedObjects.Count() == 1 ? definedObjects.Single() : null;

        return new References(definedObject ?? referencedObject, new[] { definedObject!, referencedObject! }
            .Where(o => o != null)
            .SelectMany(objet => objet switch
            {
                Class classe => new[] { (Reference: classe.Name.GetLocation()!, File: classe.GetFile()!) }
                    .Concat(modelStore.GetClassReferences(classe).Select(c => (Reference: (Reference)c.Reference, c.File))),
                Domain domain => new[] { (Reference: domain.Name.GetLocation()!, File: domain.GetFile()!) }
                    .Concat(modelStore.GetDomainReferences(domain).Select(d => (Reference: (Reference)d.Reference, d.File))),
                Decorator decorator => new[] { (Reference: decorator.Name.GetLocation()!, File: decorator.GetFile()!) }
                    .Concat(modelStore.GetDecoratorReferences(decorator).Select(d => (Reference: (Reference)d.Reference, d.File))),
                DataFlow dataFlow => new[] { (Reference: dataFlow.Name.GetLocation()!, File: dataFlow.GetFile()!) }
                    .Concat(modelStore.GetDataFlowReferences(dataFlow).Select(d => (Reference: (Reference)d.Reference, d.File))),
                IProperty property => new[] { (Reference: property.GetLocation()!, File: property.GetFile()!) }
                    .Concat(modelStore.GetPropertyReferences(property, includeTransitive).Select(d => (d.Reference, d.File))),
                _ => null!
            })
            .Distinct());
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
}

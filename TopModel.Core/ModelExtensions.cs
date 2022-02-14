using TopModel.Core.FileModel;

namespace TopModel.Core;

public static class ModelExtensions
{
    public static ModelFile GetFile(this object? objet)
    {
        return objet switch
        {
            ModelFile file => file,
            Class classe => classe.ModelFile,
            Endpoint endpoint => endpoint.ModelFile,
            IProperty { Class: Class classe } => classe.ModelFile,
            IProperty { Endpoint: Endpoint endpoint } => endpoint.ModelFile,
            Alias alias => alias.ModelFile,
            Domain domain => domain.ModelFile,
            _ => throw new ArgumentException("Type d'objet non supporté.")
        };
    }

    public static Reference? GetLocation(this object? objet)
    {
        return objet switch
        {
            Class c => c.Location,
            Endpoint e => e.Location,
            RegularProperty p => p.Location,
            AssociationProperty p => p.Location,
            CompositionProperty p => p.Location,
            AliasProperty p => p.Location,
            Alias a => a.Location,
            Domain d => d.Location,
            LocatedString l => l.Location,
            _ => null
        };
    }

    public static IEnumerable<(ClassReference Reference, ModelFile File)> GetClassReferences(this ModelStore modelStore, Class classe)
    {
        return modelStore.Classes.SelectMany(c => c.Properties)
            .Concat(modelStore.Endpoints.SelectMany(e => e.Params.Concat(e.Returns != null ? new[] { e.Returns } : Array.Empty<IProperty>())))
            .Where(p =>
                p is AliasProperty alp && alp.OriginalProperty.Class == classe
                || p is AssociationProperty ap && ap.Association == classe
                || p is CompositionProperty cp && cp.Composition == classe)
            .Select(p =>
            {
                return (Reference: p switch
                {
                    AssociationProperty ap => ap.Reference,
                    CompositionProperty cp => cp.Reference,
                    AliasProperty alp => alp.ClassReference!,
                    _ => null! // Impossible
                }, File: p.GetFile());
            })
            .Concat(modelStore.Classes.Where(c => c.Extends == classe)
                .Select(c => (Reference: c.ExtendsReference!, File: c.GetFile())))
            .Concat(modelStore.Files.SelectMany(f =>
                f.Aliases.SelectMany(a => a.Classes
                    .Where(c => f.ResolvedAliases.OfType<Class>().Any(ra => ra.Name == c.ReferenceName && ra == classe))
                    .Select(c => (Reference: c, File: f)))))
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(DomainReference Reference, ModelFile File)> GetDomainReferences(this ModelStore modelStore, Domain domain)
    {
        return modelStore.Classes.SelectMany(c => c.Properties)
            .Concat(modelStore.Endpoints.SelectMany(e => e.Params.Concat(e.Returns != null ? new[] { e.Returns } : Array.Empty<IProperty>())))
            .Where(p =>
                p is RegularProperty rp && rp.Domain == domain
                || p is CompositionProperty cp && cp.DomainKind == domain)
            .Select(p =>
            {
                return (Reference: p switch
                {
                    RegularProperty rp => rp.DomainReference,
                    CompositionProperty cp => cp.DomainKindReference!,
                    _ => null! // Impossible
                }, File: p.GetFile());
            })
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }
}

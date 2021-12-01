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
            _ => null
        };
    }
}

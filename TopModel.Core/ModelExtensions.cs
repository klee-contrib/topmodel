using TopModel.Core.FileModel;

namespace TopModel.Core;

public static class ModelExtensions
{
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

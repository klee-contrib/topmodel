#pragma warning disable MA0048

namespace TopModel.Core.Model;

public record PropertyMappingType(Domain Domain, Domain? ItemDomain);

public record ClassMappingType(Class Class, Domain? Domain, IProperty? Property);

public record ClassCollectionMappingType(Class Class, Domain Domain, IProperty? Property);

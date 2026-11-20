using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public interface IProperty : IAnnotationContainer
{
    IList<NameSegment> NameSegments { get; }

    IList<NameSegment> PropertyNameSegments { get; }

    string SqlName { get; }

    string? Label { get; }

    bool PrimaryKey { get; }

    bool Required { get; }

    DomainInstance Domain { get; }

    DomainReference? DomainReference { get; }

    string Comment { get; }

    string? DefaultValue { get; }

    bool Readonly { get; }

    LocatedString? Trigram { get; }

    ParamLocation? ParamLocation { get; }

    string? FinalTrigram { get; }

    IDictionary<string, string> CustomProperties { get; }

    IList<string>? Tags { get; }

    Class Class { get; internal set; }

    Endpoint? Endpoint { get; internal set; }

    Decorator? Decorator { get; internal set; }

    IProperty? SourceProperty { get; }

    PropertyMapping? PropertyMapping { get; internal set; }

    IEnumerable<(Reference Reference, object Target)> References { get; }

    internal ParamLocation? OwnLocation { get; }

    IProperty CloneDefinition();

    IProperty CloneForContainer(IPropertyContainer container);
}

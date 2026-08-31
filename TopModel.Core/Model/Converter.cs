using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;

namespace TopModel.Core.Model;

public class Converter(Reference location) : IVariableContainer
{
    /// <summary>
    /// Domains sources du convertisseur
    /// </summary>
    public IList<DomainReference> DomainsFromReferences { get; internal set; } = [];

    /// <summary>
    /// Domains cibles du convertisseur
    /// </summary>
    public IList<DomainReference> DomainsToReferences { get; internal set; } = [];

    /// <summary>
    /// Domains sources du convertisseur
    /// </summary>
    public IList<Domain> From { get; } = [];

    /// <summary>
    /// Domains cibles du convertisseur
    /// </summary>
    public IList<Domain> To { get; } = [];

    public IEnumerable<(Domain From, Domain To)> Conversions => From.SelectMany(f => To.Select(t => (f, t))).Distinct();

    public IDictionary<string, ConverterImplementation> Implementations { get; internal set; } =
        new Dictionary<string, ConverterImplementation>();

    public IEnumerable<ParameterReference> VariableReferences =>
        Implementations.Values.SelectMany(i => i.Text.Variables);

    public IDictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

    public IEnumerable<TransformReference> TransformReferences =>
        Implementations.Values.SelectMany(i => i.Text.Transforms).Where(pr => pr.ReferenceName.IsValidTransform());

    public required ModelFile ModelFile { get; init; }

    internal Reference Location { get; } = location;
}

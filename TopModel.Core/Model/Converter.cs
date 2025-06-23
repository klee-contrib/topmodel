using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;

namespace TopModel.Core;

public class Converter
{
#nullable disable
    /// <summary>
    /// Domains sources du convertisseur
    /// </summary>
    public IList<DomainReference> DomainsFromReferences { get; set; } = new List<DomainReference>();

    /// <summary>
    /// Domains cibles du convertisseur
    /// </summary>
    public IList<DomainReference> DomainsToReferences { get; set; } = new List<DomainReference>();

    /// <summary>
    /// Domains sources du convertisseur
    /// </summary>
    public IList<Domain> From { get; set; } = [];

    /// <summary>
    /// Domains cibles du convertisseur
    /// </summary>
    public IList<Domain> To { get; set; } = [];

    public IEnumerable<(Domain From, Domain To)> Conversions => From.SelectMany(f => To.Select(t => (f, t))).Distinct();

#nullable enable

    public Dictionary<string, ConverterImplementation> Implementations { get; set; } = [];

    public IEnumerable<ParameterReference> VariableReferences => Implementations.Values
        .SelectMany(i => i.TextWithVariables.Variables)
        .Where(pr => pr.ReferenceName.IsValidConverterVariable());

    public IEnumerable<TransformReference> TransformReferences => Implementations.Values
         .SelectMany(i => i.TextWithVariables.Transforms)
         .Where(pr => pr.ReferenceName.IsValidTransform());

#nullable disable
    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}
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
    public IList<Domain> From { get; set; } = new List<Domain>();

    /// <summary>
    /// Domains cibles du convertisseur
    /// </summary>
    public IList<Domain> To { get; set; } = new List<Domain>();

    public IEnumerable<(Domain From, Domain To)> Conversions => From.SelectMany(f => To.Select(t => (f, t))).Distinct();

#nullable enable

    /// <summary>
    /// Description du convertisseur C# dans le cas où la conversion n'est pas implicite
    /// </summary>
    public ConverterImplementation? CSharp { get; set; }

    /// <summary>
    /// Description du convertisseur java dans le cas où la conversion n'est pas implicite
    /// </summary>
    public ConverterImplementation? Java { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}
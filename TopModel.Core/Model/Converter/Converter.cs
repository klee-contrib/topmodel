using TopModel.Core.FileModel;

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

#nullable enable

    /// <summary>
    /// Description du convertisseur C# dans le cas où la conversion n'est pas implicite
    /// </summary>
    public CSharpConverter? CSharp { get; set; }

    /// <summary>
    /// Description du convertisseur java dans le cas où la conversion n'est pas implicite
    /// </summary>
    public JavaConverter? Java { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}
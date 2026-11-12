////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Région.
/// </summary>
public partial record RegionValues
{
    /// <summary>
    /// Valeurs possibles de la liste de référence Region.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Île de France.
        /// </summary>
        IDF
    }

    /// <summary>
    /// Code de la région.
    /// </summary>
    public RegionValues.Codes? Code { get; set; }

    /// <summary>
    /// Libellé de la région.
    /// </summary>
    public string? Libelle { get; set; }

    /// <summary>
    /// Nom du responsable de la région.
    /// </summary>
    public string? NomResponsable { get; set; }

}

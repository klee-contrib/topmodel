////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Département.
/// </summary>
public partial record DepartementValues
{
    /// <summary>
    /// Hauts de Seine.
    /// </summary>
    public const string HautsDeSeineCode = "92";

    /// <summary>
    /// Paris.
    /// </summary>
    public const string ParisCode = "75";

    /// <summary>
    /// Seine et Marne.
    /// </summary>
    public const string SeineEtMarneCode = "94";

    /// <summary>
    /// Seine Saint Denis.
    /// </summary>
    public const string SeineSaintDenisCode = "93";

    /// <summary>
    /// Hauts de Seine.
    /// </summary>
    public static DepartementValues HautsDeSeine { get; } = new() { Code = HautsDeSeineCode, Libelle = "restaurant.departement.values.HautsDeSeine", RegionCode = RegionValues.Codes.IDF };

    /// <summary>
    /// Paris.
    /// </summary>
    public static DepartementValues Paris { get; } = new() { Code = ParisCode, Libelle = "restaurant.departement.values.Paris", RegionCode = RegionValues.Codes.IDF };

    /// <summary>
    /// Seine et Marne.
    /// </summary>
    public static DepartementValues SeineEtMarne { get; } = new() { Code = SeineEtMarneCode, Libelle = "restaurant.departement.values.SeineEtMarne", RegionCode = RegionValues.Codes.IDF };

    /// <summary>
    /// Seine Saint Denis.
    /// </summary>
    public static DepartementValues SeineSaintDenis { get; } = new() { Code = SeineSaintDenisCode, Libelle = "restaurant.departement.values.SeineSaintDenis", RegionCode = RegionValues.Codes.IDF };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<DepartementValues> Values { get; } = [HautsDeSeine, Paris, SeineEtMarne, SeineSaintDenis];

    /// <summary>
    /// Code du département.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Libellé du département.
    /// </summary>
    public string? Libelle { get; init; }

    /// <summary>
    /// Région associée.
    /// </summary>
    public RegionValues.Codes? RegionCode { get; init; }


    /// <summary>
    /// Récupère l'instance correspondante à la clé primaire demandée.
    /// </summary>
    /// <param name="code">Code du département.</param>
    /// <returns>L'instance correspondante.</returns>
    public static DepartementValues GetValue(string code)
    {
        return code switch
        {
            ParisCode => Paris,
            HautsDeSeineCode => HautsDeSeine,
            SeineSaintDenisCode => SeineSaintDenis,
            SeineEtMarneCode => SeineEtMarne,
            _ => throw new InvalidOperationException()
        };
    }
}

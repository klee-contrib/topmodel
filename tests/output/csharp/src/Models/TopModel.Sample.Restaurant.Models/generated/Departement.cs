////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Département.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("departement")]
public partial record Departement
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
    public static Departement HautsDeSeine { get; } = new() { Code = HautsDeSeineCode, Libelle = "restaurant.departement.values.HautsDeSeine", RegionCode = Region.Codes.IDF };

    /// <summary>
    /// Paris.
    /// </summary>
    public static Departement Paris { get; } = new() { Code = ParisCode, Libelle = "restaurant.departement.values.Paris", RegionCode = Region.Codes.IDF };

    /// <summary>
    /// Seine et Marne.
    /// </summary>
    public static Departement SeineEtMarne { get; } = new() { Code = SeineEtMarneCode, Libelle = "restaurant.departement.values.SeineEtMarne", RegionCode = Region.Codes.IDF };

    /// <summary>
    /// Seine Saint Denis.
    /// </summary>
    public static Departement SeineSaintDenis { get; } = new() { Code = SeineSaintDenisCode, Libelle = "restaurant.departement.values.SeineSaintDenis", RegionCode = Region.Codes.IDF };

    /// <summary>
    /// Code du département.
    /// </summary>
    [Column("dep_code")]
    [Domain(Domains.Code)]
    [StringLength(10)]
    [Key]
    public string? Code { get; init; }

    /// <summary>
    /// Libellé du département.
    /// </summary>
    [Column("dep_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; init; }

    /// <summary>
    /// Région associée.
    /// </summary>
    [Column("reg_code")]
    [Required]
    [ReferencedType(typeof(Region))]
    [Domain(Domains.Code)]
    public Region.Codes? RegionCode { get; init; }

    /// <summary>
    /// Récupère l'instance correspondante à la clé primaire demandée.
    /// </summary>
    /// <param name="code">Code du département.</param>
    public static Departement GetValue(string code)
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

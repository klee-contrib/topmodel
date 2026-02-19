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
    public const string HautsDeSeine = "92";

    /// <summary>
    /// Paris.
    /// </summary>
    public const string Paris = "75";

    /// <summary>
    /// Seine et Marne.
    /// </summary>
    public const string SeineEtMarne = "94";

    /// <summary>
    /// Seine Saint Denis.
    /// </summary>
    public const string SeineSaintDenis = "93";

    /// <summary>
    /// Code du département.
    /// </summary>
    [Column("dep_code")]
    [Domain(Domains.Code)]
    [StringLength(10)]
    [Key]
    public string? Code { get; set; }

    /// <summary>
    /// Libellé du département.
    /// </summary>
    [Column("dep_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Région.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("region")]
public partial record Region
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
    [Column("reg_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé de la région.
    /// </summary>
    [Column("reg_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Nom du responsable de la région.
    /// </summary>
    [Column("reg_nom_responsable")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? NomResponsable { get; set; }
}

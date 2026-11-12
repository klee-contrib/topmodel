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
[Reference]
[DefaultProperty(nameof(Libelle))]
[Table("region")]
public partial record Region
{
    /// <summary>
    /// Code de la région.
    /// </summary>
    [Column("reg_code")]
    [Key]
    public RegionValues.Codes? Code { get; set; }

    /// <summary>
    /// Libellé de la région.
    /// </summary>
    [Column("reg_libelle")]
    [Required]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Nom du responsable de la région.
    /// </summary>
    [Column("reg_nom_responsable")]
    [StringLength(100)]
    public string? NomResponsable { get; set; }

    /// <summary>
    /// Convertit une valeur métier en entité de persistance.
    /// </summary>
    /// <param name="value">Valeur métier.</param>
    /// <returns>Entité de persistance.</returns>
    public static Region FromValue(RegionValues value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new Region
        {
            Code = value.Code,
            Libelle = value.Libelle,
            NomResponsable = value.NomResponsable,
        };
    }
}

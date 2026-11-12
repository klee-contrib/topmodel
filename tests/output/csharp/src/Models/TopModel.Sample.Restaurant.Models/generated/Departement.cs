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
[Reference]
[DefaultProperty(nameof(Libelle))]
[Table("departement")]
public partial record Departement
{
    /// <summary>
    /// Code du département.
    /// </summary>
    [Column("dep_code")]
    [StringLength(10)]
    [Key]
    public string? Code { get; init; }

    /// <summary>
    /// Libellé du département.
    /// </summary>
    [Column("dep_libelle")]
    [Required]
    [StringLength(100)]
    public string? Libelle { get; init; }

    /// <summary>
    /// Région associée.
    /// </summary>
    [Column("reg_code")]
    [Required]
    [ReferencedType(typeof(RegionValues))]
    public RegionValues.Codes? RegionCode { get; init; }

    /// <summary>
    /// Convertit une valeur métier en entité de persistance.
    /// </summary>
    /// <param name="value">Valeur métier.</param>
    /// <returns>Entité de persistance.</returns>
    public static Departement FromValue(DepartementValues value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new Departement
        {
            Code = value.Code,
            Libelle = value.Libelle,
            RegionCode = value.RegionCode,
        };
    }
}

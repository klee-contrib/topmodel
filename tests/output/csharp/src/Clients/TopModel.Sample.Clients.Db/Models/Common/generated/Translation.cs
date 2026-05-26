////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Common;

/// <summary>
/// Table pour stocker les traductions en SQL.
/// </summary>
[Table("translation")]
public partial record Translation
{
    /// <summary>
    /// Clé de traduction.
    /// </summary>
    [Column("tra_resource_key")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? ResourceKey { get; set; }

    /// <summary>
    /// Valeur de la clé de traduction.
    /// </summary>
    [Column("tra_value")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Value { get; set; }

    /// <summary>
    /// Langue de traduction.
    /// </summary>
    [Column("tra_lang")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Lang { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Common;

/// <summary>
/// Classe pour contenir les traductions en base de données.
/// </summary>
[Table("traduction")]
public partial record Traduction
{
    /// <summary>
    /// Clé de traduction.
    /// </summary>
    [Column("trd_resource_key")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    [Key]
    public string ResourceKey { get; set; }

    /// <summary>
    /// Valeur.
    /// </summary>
    [Column("trd_label")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Label { get; set; }
}

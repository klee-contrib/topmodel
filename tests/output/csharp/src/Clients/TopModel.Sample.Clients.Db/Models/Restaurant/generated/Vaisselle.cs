////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Vaisselle de restaurant.
/// </summary>
public abstract partial record Vaisselle
{
    /// <summary>
    /// Id de la vaisselle.
    /// </summary>
    [Column("vsl_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Description de la vaisselle.
    /// </summary>
    [Column("vsl_description")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Description { get; set; }
}

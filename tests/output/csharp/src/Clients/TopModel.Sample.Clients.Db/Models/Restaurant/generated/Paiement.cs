////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Paiement.
/// </summary>
[Table("paiement")]
public partial record Paiement
{
    /// <summary>
    /// Facture associée au paiement.
    /// </summary>
    [Required]
    public Facture? Facture { get; set; }

    /// <summary>
    /// Carte Swile utilisée pour le paiement.
    /// </summary>
    [Column("swi_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? SwileCardId { get; set; }
}

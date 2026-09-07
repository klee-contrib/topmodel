////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Facture.
/// </summary>
[Table("facture")]
public partial record Facture
{
    /// <summary>
    /// Identifiant de la facture.
    /// </summary>
    [Column("id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }
}

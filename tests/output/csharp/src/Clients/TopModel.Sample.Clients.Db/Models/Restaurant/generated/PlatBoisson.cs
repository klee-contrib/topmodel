////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Boisson.
/// </summary>
public partial record PlatBoisson : Plat
{
    /// <summary>
    /// Volume de la boisson.
    /// </summary>
    [Column("volume")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Volume { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un avis en écriture.
/// </summary>
public partial record AvisClientWrite
{
    /// <summary>
    /// Note sur 5.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? Note { get; set; }

    /// <summary>
    /// Commentaire de l'avis.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Commentaire { get; set; }

    /// <summary>
    /// Indique si l'avis est approuvé par le restaurant.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Approuve { get; set; } = false;

    /// <summary>
    /// Client ayant donné l'avis.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? ClientIdClient { get; set; }

    /// <summary>
    /// Restaurant concerné par l'avis.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }
}

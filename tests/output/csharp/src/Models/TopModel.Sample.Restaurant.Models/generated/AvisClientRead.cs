////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un avis en lecture.
/// </summary>
public partial record AvisClientRead
{
    /// <summary>
    /// Identifiant de l'avis.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

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
    /// Date de l'avis.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateAvis { get; set; }

    /// <summary>
    /// Indique si l'avis est approuvé par le restaurant.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Approuve { get; set; } = false;

    /// <summary>
    /// Nombre de vues de l'avis (calculé).
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombreVues { get; set; } = 0;

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

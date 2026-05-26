////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une table en lecture.
/// </summary>
public partial record TableRead
{
    /// <summary>
    /// Identifiant de la table.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Numéro de la table.
    /// </summary>
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string? Numero { get; set; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? Capacite { get; set; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }
}

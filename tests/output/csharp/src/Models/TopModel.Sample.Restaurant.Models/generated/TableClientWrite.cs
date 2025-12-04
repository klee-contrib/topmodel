////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une table en écriture.
/// </summary>
public partial record TableClientWrite
{
    /// <summary>
    /// Numéro de la table.
    /// </summary>
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string Numero { get; set; }

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
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Commande.TableClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Commandes { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.TableClientIdTable.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> ReservationsTable { get; set; }
}

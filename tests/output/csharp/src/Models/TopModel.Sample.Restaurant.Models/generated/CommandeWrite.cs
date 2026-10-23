////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en écriture.
/// </summary>
public partial record CommandeWrite
{
    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    public DateTime? DateCommande { get; set; }

    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    public DateTime? DateLivraison { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Required]
    public decimal? MontantTotal { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    public int? TableId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    public StatutCommande? StatutCommande { get; set; } = Models.StatutCommande.EN_ATT;

    /// <summary>
    /// Avis laissé par le client sur la commande.
    /// </summary>
    public int? AvisClientId { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    [Required]
    public ClientWrite Client { get; set; } = new();

    /// <summary>
    /// Réservation.
    /// </summary>
    public ReservationWrite? Reservation { get; set; }

    /// <summary>
    /// Association réciproque de LigneCommande.Commande.
    /// </summary>
    [Required]
    public ICollection<LigneCommandeWrite> Lignes { get; set; } = [];
}

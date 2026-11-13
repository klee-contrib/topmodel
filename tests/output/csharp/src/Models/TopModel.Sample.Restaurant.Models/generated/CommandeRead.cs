////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en lecture.
/// </summary>
public partial record CommandeRead
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    [Required]
    public int? CommandeId { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    public DateTime? CommandeDateCommande { get; set; }

    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    public DateTime? CommandeDateLivraison { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Required]
    public decimal? CommandeMontantTotal { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    public int? CommandeTableId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    public StatutCommande? CommandeStatutCommande { get; set; } = StatutCommande.EN_ATT;

    /// <summary>
    /// Avis laissé par le client sur la commande.
    /// </summary>
    public int? CommandeAvisClientId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? CommandeDateCreation { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    [Required]
    public ClientRead Client { get; set; } = new();

    /// <summary>
    /// Réservation.
    /// </summary>
    public ReservationRead? Reservation { get; set; }

    /// <summary>
    /// Association réciproque de LigneCommande.Commande.
    /// </summary>
    [Required]
    public ICollection<LigneCommandeRead> Lignes { get; set; } = [];
}

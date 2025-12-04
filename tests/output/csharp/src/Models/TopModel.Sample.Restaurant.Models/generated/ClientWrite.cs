////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un client en écriture.
/// </summary>
public partial record ClientWrite
{
    /// <summary>
    /// Nom du client.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Prénom du client.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Prenom { get; set; }

    /// <summary>
    /// Numéro de téléphone du client.
    /// </summary>
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string Telephone { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Association réciproque de Commande.ClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Commandes { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.ClientIdClient.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> AvisClientsClient { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.ClientIdClient.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> ReservationsClient { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Client avec la liste de ses commandes.
/// </summary>
public partial record ClientAvecCommandes
{
    /// <summary>
    /// Identifiant du client.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

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

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [Domain(Domains.Liste)]
    public ICollection<int> CommandeId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [Domain(Domains.Liste)]
    public ICollection<DateTime> CommandeDateCommande { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<DateTime> CommandeDateLivraison { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [Domain(Domains.Liste)]
    public ICollection<decimal> CommandeMontantTotal { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [Domain(Domains.Liste)]
    public ICollection<int> CommandeClientId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> CommandeTableClientId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    [Domain(Domains.Liste)]
    public ICollection<StatutCommande.Codes> CommandeStatutCommandeCode { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<ICollection<int>> CommandeLigneCommandes { get; set; }
}

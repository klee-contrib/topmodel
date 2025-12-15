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
    /// Identifiant de la personne.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom de la personne.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Prenom { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.ClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> AvisClients { get; set; }

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
    public ICollection<int> CommandeTableId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> CommandeReservationId { get; set; }

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
    public ICollection<ICollection<int>> CommandeLignes { get; set; }
}

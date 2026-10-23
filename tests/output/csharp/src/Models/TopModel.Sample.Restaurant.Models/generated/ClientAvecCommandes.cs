////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

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
    public int? Id { get; set; }

    /// <summary>
    /// Nom de la personne.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Prenom { get; set; }

    /// <summary>
    /// Département de résidence de la personne.
    /// </summary>
    [ReferencedType(typeof(Departement))]
    [StringLength(10)]
    public string? DepartementCode { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Carte Swile du client.
    /// </summary>
    public int? SwileCardId { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.Client.
    /// </summary>
    public ICollection<int>? AvisClients { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    public ICollection<int>? CommandeId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    public ICollection<DateTime>? CommandeDateCommande { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    public ICollection<DateTime>? CommandeDateLivraison { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    public ICollection<decimal>? CommandeMontantTotal { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    public ICollection<int>? CommandeClientId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    public ICollection<int>? CommandeTableId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    public ICollection<int>? CommandeReservationId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    public ICollection<StatutCommande>? CommandeStatutCommande { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    public ICollection<int>? CommandeAvisClientId { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    public ICollection<ICollection<int>>? CommandeLignes { get; set; }

    /// <summary>
    /// Liste des commandes du client.
    /// </summary>
    [Required]
    public ICollection<DateTime>? CommandeDateCreation { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Client du restaurant.
/// </summary>
[Table("client")]
public partial record Client
{
    /// <summary>
    /// Identifiant du client.
    /// </summary>
    [Column("cli_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du client.
    /// </summary>
    [Column("cli_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Prénom du client.
    /// </summary>
    [Column("cli_prenom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Prenom { get; set; }

    /// <summary>
    /// Numéro de téléphone du client.
    /// </summary>
    [Column("cli_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string Telephone { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    [Column("cli_email")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Association réciproque de Commande.ClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> Commandes { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.ClientIdClient.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> AvisClientsClient { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.ClientIdClient.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> ReservationsClient { get; set; }
}

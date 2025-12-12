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
public partial record Client : Personne
{
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
    /// Association réciproque de Reservation.ClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> Reservations { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.ClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> AvisClients { get; set; }
}

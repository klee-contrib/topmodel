////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Carte Swile du client.
    /// </summary>
    [Column("swi_id")]
    public int? SwileCard { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.Client.
    /// </summary>
    public ICollection<AvisClient> AvisClients { get; set; } = [];
}

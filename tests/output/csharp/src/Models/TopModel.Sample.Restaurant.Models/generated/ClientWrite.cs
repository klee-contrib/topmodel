////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un client en écriture.
/// </summary>
public partial record ClientWrite
{
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
    public string? DepartementCode { get; set; } = Departement.ParisCode;

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
}

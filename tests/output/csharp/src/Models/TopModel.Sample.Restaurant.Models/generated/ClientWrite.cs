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
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Prenom { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.Client.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? AvisClients { get; set; }
}

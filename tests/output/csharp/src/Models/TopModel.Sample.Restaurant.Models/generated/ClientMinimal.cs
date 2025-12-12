////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Version minimale d'un client.
/// </summary>
public partial record ClientMinimal
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
    /// Nom complet du client (calculé).
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string NomComplet { get; set; }
}

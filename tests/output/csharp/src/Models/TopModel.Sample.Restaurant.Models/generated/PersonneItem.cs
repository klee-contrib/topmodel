////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une personne en liste.
/// </summary>
public abstract partial record PersonneItem
{
    /// <summary>
    /// Identifiant de l'employé.
    /// </summary>
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom de l'employé.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Prénom de l'employé.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Prenom { get; set; }
}

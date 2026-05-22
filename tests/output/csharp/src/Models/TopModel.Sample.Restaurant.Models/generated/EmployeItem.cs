////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un employé en liste.
/// </summary>
public partial record EmployeItem : PersonneItem
{
    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string? Matricule { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Liste des autres employés.
    /// </summary>
    [Required]
    public ICollection<EmployeItem> AutresEmployes { get; set; } = [];
}

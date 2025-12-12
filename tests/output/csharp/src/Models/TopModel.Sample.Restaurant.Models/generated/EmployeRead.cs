////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un employé en lecture.
/// </summary>
public partial record EmployeRead
{
    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string Matricule { get; set; }

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateEmbauche { get; set; }

    /// <summary>
    /// Salaire de l'employé.
    /// </summary>
    [Domain(Domains.Prix)]
    public decimal? Salaire { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }
}

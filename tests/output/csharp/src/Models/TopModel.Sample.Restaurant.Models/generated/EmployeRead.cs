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
    /// Numéro de téléphone de l'employé.
    /// </summary>
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string Telephone { get; set; }

    /// <summary>
    /// Date de naissance.
    /// </summary>
    [Domain(Domains.DateHeure)]
    public DateTime? DateNaissance { get; set; }

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un employé en écriture.
/// </summary>
public partial record EmployeWrite
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
    /// Numéro de téléphone de l'employé.
    /// </summary>
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Date de naissance.
    /// </summary>
    public DateTime? DateNaissance { get; set; }

    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string? Matricule { get; set; }

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    [Required]
    public DateTime? DateEmbauche { get; set; }

    /// <summary>
    /// Salaire de l'employé.
    /// </summary>
    public decimal? Salaire { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }
}

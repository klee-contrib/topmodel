////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Employé du restaurant.
/// </summary>
[Table("employe")]
public partial record Employe : Personne
{
    /// <summary>
    /// Numéro de téléphone de l'employé.
    /// </summary>
    [Column("emp_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Date de naissance.
    /// </summary>
    [Column("emp_date_naissance")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateNaissance { get; set; }

    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    [Column("emp_matricule")]
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string? Matricule { get; set; }

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    [Column("emp_date_embauche")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateEmbauche { get; set; }

    /// <summary>
    /// Salaire de l'employé.
    /// </summary>
    [Column("emp_salaire")]
    [Domain(Domains.Prix)]
    public decimal? Salaire { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    [Column("res_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Employé du restaurant.
/// </summary>
[Table("employe")]
public partial record Employe : Personne, IEmployeBase
{
    /// <summary>
    /// Michel.
    /// </summary>
    public const string MichelMatricule = "123456";

    /// <summary>
    /// Numéro de téléphone de l'employé.
    /// </summary>
    [Column("emp_telephone")]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Date de naissance.
    /// </summary>
    [Column("emp_date_naissance")]
    public DateTime? DateNaissance { get; set; }

    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    [Column("emp_matricule")]
    [Required]
    [StringLength(10)]
    public string? Matricule { get; set; }

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    [Column("emp_date_embauche")]
    [Required]
    public DateTime? DateEmbauche { get; set; }

    /// <summary>
    /// Salaire de l'employé.
    /// </summary>
    [Column("emp_salaire")]
    public decimal? Salaire { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    [Required]
    public Restaurant? Restaurant { get; set; }
}

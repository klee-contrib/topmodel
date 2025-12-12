////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Classe de base représentant une personne.
/// </summary>
[Table("personne")]
public partial record Personne
{
    /// <summary>
    /// Identifiant de la personne.
    /// </summary>
    [Column("per_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom de la personne.
    /// </summary>
    [Column("per_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    [Column("per_prenom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Prenom { get; set; }
}

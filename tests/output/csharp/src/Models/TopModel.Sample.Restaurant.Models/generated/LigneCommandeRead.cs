////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une ligne de commande en lecture.
/// </summary>
public partial record LigneCommandeRead
{
    /// <summary>
    /// Identifiant de la ligne.
    /// </summary>
    [Required]
    [Domain(Domains.Id2)]
    public int? Id { get; set; }

    /// <summary>
    /// Quantité commandée.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? Quantite { get; set; }

    /// <summary>
    /// Prix unitaire au moment de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Prix total de la ligne.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? PrixTotal { get; set; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    [Required]
    [Domain(Domains.Id2)]
    public int? CommandeId { get; set; }

    /// <summary>
    /// Plat commandé.
    /// </summary>
    [Required]
    [Domain(Domains.SeqId)]
    public int? PlatId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en lecture.
/// </summary>
public partial record PlatRead
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Description du plat.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Description { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    [Required]
    [ReferencedType(typeof(CategoriePlat))]
    [Domain(Domains.Code)]
    public CategoriePlat.Codes? CategoriePlatCodeCategoriePlat { get; set; }

    /// <summary>
    /// Restaurant proposant ce plat.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de LigneCommande.PlatId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> LigneCommandes { get; set; }

    /// <summary>
    /// Association réciproque de MenuPlat.PlatIdPlat.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> MenuPlatsPlat { get; set; }

    /// <summary>
    /// Association réciproque de PromotionPlat.PlatIdPlat.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> PromotionPlatsPlat { get; set; }
}

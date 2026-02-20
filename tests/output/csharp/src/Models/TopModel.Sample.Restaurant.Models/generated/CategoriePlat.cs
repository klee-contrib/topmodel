////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Catégorie de plat.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("categorie_plat")]
public partial record CategoriePlat
{
    /// <summary>
    /// Boisson.
    /// </summary>
    public const int BoissonOrdre = 1;

    /// <summary>
    /// Dessert.
    /// </summary>
    public const int DessertOrdre = 4;

    /// <summary>
    /// Entrée.
    /// </summary>
    public const int EntreeOrdre = 2;

    /// <summary>
    /// Plat principal.
    /// </summary>
    public const int PlatOrdre = 3;

    /// <summary>
    /// Valeurs possibles de la liste de référence CategoriePlat.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Boisson.
        /// </summary>
        BOISSON,

        /// <summary>
        /// Dessert.
        /// </summary>
        DESSERT,

        /// <summary>
        /// Entrée.
        /// </summary>
        ENTREE,

        /// <summary>
        /// Plat principal.
        /// </summary>
        PLAT
    }

    /// <summary>
    /// Boisson.
    /// </summary>
    public static CategoriePlat Boisson { get; } = new() { Code = Codes.BOISSON, Libelle = "restaurant.categoriePlat.values.Boisson", Ordre = BoissonOrdre };

    /// <summary>
    /// Dessert.
    /// </summary>
    public static CategoriePlat Dessert { get; } = new() { Code = Codes.DESSERT, Libelle = "restaurant.categoriePlat.values.Dessert", Ordre = DessertOrdre };

    /// <summary>
    /// Entrée.
    /// </summary>
    public static CategoriePlat Entree { get; } = new() { Code = Codes.ENTREE, Libelle = "restaurant.categoriePlat.values.Entree", Ordre = EntreeOrdre };

    /// <summary>
    /// Plat principal.
    /// </summary>
    public static CategoriePlat Plat { get; } = new() { Code = Codes.PLAT, Libelle = "restaurant.categoriePlat.values.Plat", Ordre = PlatOrdre };

    /// <summary>
    /// Code de la catégorie.
    /// </summary>
    [Column("cat_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; init; }

    /// <summary>
    /// Libellé de la catégorie.
    /// </summary>
    [Column("cat_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; init; }

    /// <summary>
    /// Ordre d'affichage dans le menu.
    /// </summary>
    [Column("cat_ordre")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Ordre { get; init; }

    /// <summary>
    /// Récupère l'instance correspondante à la clé primaire demandée.
    /// </summary>
    /// <param name="code">Code de la catégorie.</param>
    public static CategoriePlat GetValue(Codes code)
    {
        return code switch
        {
            Codes.ENTREE => Entree,
            Codes.PLAT => Plat,
            Codes.DESSERT => Dessert,
            Codes.BOISSON => Boisson,
            _ => throw new InvalidOperationException()
        };
    }
}

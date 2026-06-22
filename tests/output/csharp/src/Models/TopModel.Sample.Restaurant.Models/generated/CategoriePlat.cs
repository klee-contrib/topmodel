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
[Reference]
[DefaultProperty(nameof(Libelle))]
[Table("categorie_plat")]
public partial record CategoriePlat
{
    /// <summary>
    /// Autre.
    /// </summary>
    public const int AutreOrdre = 5;

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
    public const int PrincipalOrdre = 3;

    /// <summary>
    /// Valeurs possibles de la liste de référence CategoriePlat.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Autre.
        /// </summary>
        AUTRE,

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
        PRINCIPAL
    }

    /// <summary>
    /// Autre.
    /// </summary>
    public static CategoriePlat Autre { get; } = new() { Code = Codes.AUTRE, Libelle = "restaurant.categoriePlat.values.Autre", Ordre = AutreOrdre };

    /// <summary>
    /// Boisson.
    /// </summary>
    public static CategoriePlat Boisson { get; } = new() { Code = Codes.BOISSON, Libelle = "restaurant.categoriePlat.values.Boisson", Ordre = BoissonOrdre, PrixMoyen = 2 };

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
    public static CategoriePlat Principal { get; } = new() { Code = Codes.PRINCIPAL, Libelle = "restaurant.categoriePlat.values.Principal", Ordre = PrincipalOrdre, PrixMoyen = 10 };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<CategoriePlat> Values { get; } = [Boisson, Entree, Principal, Dessert, Autre];

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
    /// Prix moyen de la catégorie, à titre indicatif.
    /// </summary>
    [Column("cat_prix_moyen")]
    [Domain(Domains.Prix)]
    public decimal? PrixMoyen { get; init; }

    /// <summary>
    /// Récupère l'instance correspondante à la clé primaire demandée.
    /// </summary>
    /// <param name="code">Code de la catégorie.</param>
    public static CategoriePlat GetValue(Codes code)
    {
        return code switch
        {
            Codes.AUTRE => Autre,
            Codes.ENTREE => Entree,
            Codes.PRINCIPAL => Principal,
            Codes.DESSERT => Dessert,
            Codes.BOISSON => Boisson,
            _ => throw new InvalidOperationException()
        };
    }
}

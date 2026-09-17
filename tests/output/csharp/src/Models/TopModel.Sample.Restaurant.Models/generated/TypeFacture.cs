////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Type de facture.
/// </summary>
public partial record TypeFacture
{
    /// <summary>
    /// Valeurs possibles de la liste de référence TypeFacture.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Facture électronique.
        /// </summary>
        ELE,

        /// <summary>
        /// Facture physique.
        /// </summary>
        PHY
    }

    /// <summary>
    /// Facture électronique.
    /// </summary>
    public static TypeFacture Ele { get; } = new() { Code = Codes.ELE, Libelle = "Facture électronique" };

    /// <summary>
    /// Facture physique.
    /// </summary>
    public static TypeFacture Phy { get; } = new() { Code = Codes.PHY, Libelle = "Facture physique" };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<TypeFacture> Values { get; } = [Ele, Phy];

    /// <summary>
    /// Code du type de facture.
    /// </summary>
    [Domain(Domains.Code)]
    public Codes? Code { get; init; }

    /// <summary>
    /// Libellé du type de facture.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; init; }

    /// <summary>
    /// Récupère l'instance correspondante à la clé primaire demandée.
    /// </summary>
    /// <param name="code">Code du type de facture.</param>
    public static TypeFacture GetValue(Codes code)
    {
        return code switch
        {
            Codes.ELE => Ele,
            Codes.PHY => Phy,
            _ => throw new InvalidOperationException()
        };
    }
}

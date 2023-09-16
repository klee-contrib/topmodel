////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace Models.CSharp.Securite.Profil.Models;

/// <summary>
/// Détail d'un profil en liste.
/// </summary>
public interface IProfilItem
{
    /// <summary>
    /// Id technique.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Libellé du profil.
    /// </summary>
    string Libelle { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Id technique.</param>
    /// <param name="libelle">Libellé du profil.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IProfilItem Create(int? id = null, string libelle = null);
}

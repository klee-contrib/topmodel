////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace Hello World.Users.Models;

/// <summary>
/// Objet de transfert pour la classe Utilisateur, dans le cas de la modification de celui-ci.
/// </summary>
public partial record UtilisateurUpdateDto
{
    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [StringLength(15)]
    public string Nom { get; set; }
}

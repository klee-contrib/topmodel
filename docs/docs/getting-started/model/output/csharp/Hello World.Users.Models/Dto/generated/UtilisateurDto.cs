////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Hello World.Common;
using Kinetix.Modeling.Annotations;
using MyProject.Common.Page;

namespace Hello World.Users.Models;

/// <summary>
/// Objet de transfert pour la classe Utilisateur.
/// </summary>
public partial record UtilisateurDto
{
    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(15)]
    public string NomUtilisateur { get; set; }

    /// <summary>
    /// Adresse de l'utilisateur.
    /// </summary>
    [Required]
    public Page<AdresseDto> Adresse { get; set; }
}

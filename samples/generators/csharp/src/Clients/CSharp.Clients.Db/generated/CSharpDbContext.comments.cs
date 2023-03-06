////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using CSharp.Clients.Db.Models.Securite;
using CSharp.Clients.Db.Models.Utilisateur;
using Microsoft.EntityFrameworkCore;
using Models.CSharp.Securite.Models;
using Models.CSharp.Utilisateur.Models;

namespace CSharp.Clients.Db;

/// <summary>
/// Partial pour ajouter les commentaires EF.
/// </summary>
public partial class CSharpDbContext : DbContext
{
    partial void AddComments(ModelBuilder modelBuilder)
    {
        var droit = modelBuilder.Entity<Droit>();
        droit.ToTable(t => t.HasComment("Droits de l'application"));
        droit.Property(p => p.Code).HasComment("Code du droit");
        droit.Property(p => p.Libelle).HasComment("Libellé du droit");
        droit.Property(p => p.TypeProfilCode).HasComment("Type de profil pouvant faire l'action");

        var profil = modelBuilder.Entity<Profil>();
        profil.ToTable(t => t.HasComment("Profil des utilisateurs"));
        profil.Property(p => p.Id).HasComment("Id technique");
        profil.Property(p => p.TypeProfilCode).HasComment("Type de profil");
        profil.Property(p => p.Droits).HasComment("Liste des droits de l'utilisateur");
        profil.Property(p => p.Secteurs).HasComment("Liste des secteurs de l'utilisateur");

        var secteur = modelBuilder.Entity<Secteur>();
        secteur.ToTable(t => t.HasComment("Secteur d'application du profil"));
        secteur.Property(p => p.Id).HasComment("Id technique");

        var typeProfil = modelBuilder.Entity<TypeProfil>();
        typeProfil.ToTable(t => t.HasComment("Type d'utilisateur"));
        typeProfil.Property(p => p.Code).HasComment("Code du type d'utilisateur");
        typeProfil.Property(p => p.Libelle).HasComment("Libellé du type d'utilisateur");

        var typeUtilisateur = modelBuilder.Entity<TypeUtilisateur>();
        typeUtilisateur.ToTable(t => t.HasComment("Type d'utilisateur"));
        typeUtilisateur.Property(p => p.Code).HasComment("Code du type d'utilisateur");
        typeUtilisateur.Property(p => p.Libelle).HasComment("Libellé du type d'utilisateur");

        var utilisateur = modelBuilder.Entity<Utilisateur>();
        utilisateur.ToTable(t => t.HasComment("Utilisateur de l'application"));
        utilisateur.Property(p => p.Id).HasComment("Id technique");
        utilisateur.Property(p => p.Age).HasComment("Age en années de l'utilisateur");
        utilisateur.Property(p => p.ProfilId).HasComment("Profil de l'utilisateur");
        utilisateur.Property(p => p.Email).HasComment("Email de l'utilisateur");
        utilisateur.Property(p => p.Nom).HasComment("Nom de l'utilisateur");
        utilisateur.Property(p => p.Actif).HasComment("Si l'utilisateur est actif");
        utilisateur.Property(p => p.TypeUtilisateurCode).HasComment("Type d'utilisateur en Many to one");
        utilisateur.Property(p => p.UtilisateurIdParent).HasComment("Utilisateur parent");
        utilisateur.Property(p => p.DateCreation).HasComment("Date de création de l'utilisateur");
        utilisateur.Property(p => p.DateModification).HasComment("Date de modification de l'utilisateur");
    }
}

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
/// DbContext généré pour Entity Framework Core.
/// </summary>
public partial class CSharpDbContext : DbContext
{
    /// <summary>
    /// Constructeur par défaut.
    /// </summary>
    /// <param name="options">Options du DbContext.</param>
    public CSharpDbContext(DbContextOptions<CSharpDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Accès à l'entité Droit.
    /// </summary>
    public DbSet<Droit> Droits { get; set; }

    /// <summary>
    /// Accès à l'entité Profil.
    /// </summary>
    public DbSet<Profil> Profils { get; set; }

    /// <summary>
    /// Accès à l'entité Secteur.
    /// </summary>
    public DbSet<Secteur> Secteurs { get; set; }

    /// <summary>
    /// Accès à l'entité TypeProfil.
    /// </summary>
    public DbSet<TypeProfil> TypeProfils { get; set; }

    /// <summary>
    /// Accès à l'entité TypeUtilisateur.
    /// </summary>
    public DbSet<TypeUtilisateur> TypeUtilisateurs { get; set; }

    /// <summary>
    /// Accès à l'entité Utilisateur.
    /// </summary>
    public DbSet<Utilisateur> Utilisateurs { get; set; }

    /// <summary>
    /// Personalisation du modèle.
    /// </summary>
    /// <param name="modelBuilder">L'objet de construction du modèle.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Droit>().Property(p => p.Code).HasConversion<string>().HasMaxLength(3);
        modelBuilder.Entity<Droit>().Property(p => p.TypeProfilCode).HasConversion<string>().HasMaxLength(3);
        modelBuilder.Entity<Profil>().Property(p => p.TypeProfilCode).HasConversion<string>().HasMaxLength(3);
        modelBuilder.Entity<Profil>().Property(p => p.Droits).HasConversion<string[]>().HasMaxLength(3);
        modelBuilder.Entity<TypeProfil>().Property(p => p.Code).HasConversion<string>().HasMaxLength(3);
        modelBuilder.Entity<TypeUtilisateur>().Property(p => p.Code).HasConversion<string>().HasMaxLength(3);
        modelBuilder.Entity<Utilisateur>().Property(x => x.Age).HasPrecision(20, 9);
        modelBuilder.Entity<Utilisateur>().Property(p => p.TypeUtilisateurCode).HasConversion<string>().HasMaxLength(3);

        modelBuilder.Entity<Droit>().HasOne<TypeProfil>().WithMany().HasForeignKey(p => p.TypeProfilCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Profil>().HasOne<TypeProfil>().WithMany().HasForeignKey(p => p.TypeProfilCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<Profil>().WithMany().HasForeignKey(p => p.ProfilId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<TypeUtilisateur>().WithMany().HasForeignKey(p => p.TypeUtilisateurCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<Utilisateur>().WithOne().HasForeignKey<Utilisateur>(p => p.UtilisateurIdParent).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Utilisateur>().HasIndex(p => new { p.email, p.UtilisateurIdParent }).IsUnique();

        modelBuilder.Entity<Droit>().HasData(
            new Droit { Code = Droit.Codes.CRE, Libelle = "Créer", TypeProfilCode = TypeProfil.Codes.ADM },
            new Droit { Code = Droit.Codes.MOD, Libelle = "Modifier" },
            new Droit { Code = Droit.Codes.SUP, Libelle = "Supprimer" });
        modelBuilder.Entity<TypeProfil>().HasData(
            new TypeProfil { Code = TypeProfil.Codes.ADM, Libelle = "Administrateur" },
            new TypeProfil { Code = TypeProfil.Codes.GES, Libelle = "Gestionnaire" });
        modelBuilder.Entity<TypeUtilisateur>().HasData(
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.ADM, Libelle = "Administrateur" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.GES, Libelle = "Gestionnaire" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.CLI, Libelle = "Client" });

        AddComments(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void AddComments(ModelBuilder modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

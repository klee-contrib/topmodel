////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using CSharp.Clients.Db.Models.Securite.Profil;
using CSharp.Clients.Db.Models.Securite.Utilisateur;
using Microsoft.EntityFrameworkCore;
using Models.CSharp.Securite.Profil.Models;
using Models.CSharp.Securite.Utilisateur.Models;

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
    /// Accès à l'entité TypeDroit.
    /// </summary>
    public DbSet<TypeDroit> TypeDroits { get; set; }

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
        modelBuilder.Entity<Droit>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Droit>().Property(p => p.TypeDroitCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Profil>().Property(p => p.Droits).HasConversion<string[]>().HasMaxLength(10);
        modelBuilder.Entity<TypeDroit>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<TypeUtilisateur>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Utilisateur>().Property(p => p.TypeUtilisateurCode).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<Droit>().HasOne<TypeDroit>().WithMany().HasForeignKey(p => p.TypeDroitCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<Profil>().WithMany().HasForeignKey(p => p.ProfilId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<TypeUtilisateur>().WithMany().HasForeignKey(p => p.TypeUtilisateurCode).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Utilisateur>().HasIndex(p => p.Email).IsUnique();

        modelBuilder.Entity<Droit>().HasData(
            new Droit { Code = Droit.Codes.CREATE, Libelle = "Création", TypeDroitCode = TypeDroit.Codes.WRITE },
            new Droit { Code = Droit.Codes.READ, Libelle = "Lecture", TypeDroitCode = TypeDroit.Codes.READ },
            new Droit { Code = Droit.Codes.UPDATE, Libelle = "Mise à jour", TypeDroitCode = TypeDroit.Codes.WRITE },
            new Droit { Code = Droit.Codes.DELETE, Libelle = "Suppression", TypeDroitCode = TypeDroit.Codes.ADMIN });
        modelBuilder.Entity<TypeDroit>().HasData(
            new TypeDroit { Code = TypeDroit.Codes.READ, Libelle = "Lecture" },
            new TypeDroit { Code = TypeDroit.Codes.WRITE, Libelle = "Ecriture" },
            new TypeDroit { Code = TypeDroit.Codes.ADMIN, Libelle = "Administration" });
        modelBuilder.Entity<TypeUtilisateur>().HasData(
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.ADMIN, Libelle = "Administrateur" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.GEST, Libelle = "Gestionnaire" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.CLIENT, Libelle = "Client" });

        AddComments(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void AddComments(ModelBuilder modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

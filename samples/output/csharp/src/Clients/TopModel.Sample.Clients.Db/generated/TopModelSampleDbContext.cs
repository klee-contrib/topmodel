////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Common;
using TopModel.Sample.Clients.Db.Models.Securite.Profil;
using TopModel.Sample.Clients.Db.Models.Securite.Utilisateur;
using TopModel.Sample.Securite.Models.Profil;
using TopModel.Sample.Securite.Models.Utilisateur;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// DbContext généré pour Entity Framework Core.
/// </summary>
public partial class TopModelSampleDbContext : DbContext
{
    /// <summary>
    /// Constructeur par défaut.
    /// </summary>
    /// <param name="options">Options du DbContext.</param>
    public TopModelSampleDbContext(DbContextOptions<TopModelSampleDbContext> options)
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
    /// Accès à l'entité ProfilDroit.
    /// </summary>
    public DbSet<ProfilDroit> ProfilDroits { get; set; }

    /// <summary>
    /// Accès à l'entité Traduction.
    /// </summary>
    public DbSet<Traduction> Traductions { get; set; }

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
        modelBuilder.Entity<ProfilDroit>().Property(p => p.DroitCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<TypeDroit>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<TypeUtilisateur>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Utilisateur>().Property(p => p.TypeUtilisateurCode).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<ProfilDroit>().HasKey(p => new { p.ProfilId, p.DroitCode });

        modelBuilder.Entity<Droit>().HasOne<TypeDroit>().WithMany().HasForeignKey(p => p.TypeDroitCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProfilDroit>().HasOne<Profil>().WithMany().HasForeignKey(p => p.ProfilId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProfilDroit>().HasOne<Droit>().WithMany().HasForeignKey(p => p.DroitCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<Profil>().WithMany().HasForeignKey(p => p.ProfilId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Utilisateur>().HasOne<TypeUtilisateur>().WithMany().HasForeignKey(p => p.TypeUtilisateurCode).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Utilisateur>().HasIndex(p => p.Email).IsUnique();

        modelBuilder.Entity<Droit>().HasIndex(p => p.Libelle);
        modelBuilder.Entity<TypeDroit>().HasIndex(p => p.Libelle);
        modelBuilder.Entity<TypeUtilisateur>().HasIndex(p => p.Libelle);

        modelBuilder.Entity<Droit>().HasData(
            new Droit { Code = Droit.Codes.CREATE, Libelle = "securite.profil.droit.values.Create", TypeDroitCode = TypeDroit.Codes.WRITE },
            new Droit { Code = Droit.Codes.READ, Libelle = "securite.profil.droit.values.Read", TypeDroitCode = TypeDroit.Codes.READ },
            new Droit { Code = Droit.Codes.UPDATE, Libelle = "securite.profil.droit.values.Update", TypeDroitCode = TypeDroit.Codes.WRITE },
            new Droit { Code = Droit.Codes.DELETE, Libelle = "securite.profil.droit.values.Delete", TypeDroitCode = TypeDroit.Codes.ADMIN });
        modelBuilder.Entity<TypeDroit>().HasData(
            new TypeDroit { Code = TypeDroit.Codes.READ, Libelle = "securite.profil.typeDroit.values.Read" },
            new TypeDroit { Code = TypeDroit.Codes.WRITE, Libelle = "securite.profil.typeDroit.values.Write" },
            new TypeDroit { Code = TypeDroit.Codes.ADMIN, Libelle = "securite.profil.typeDroit.values.Admin" });
        modelBuilder.Entity<TypeUtilisateur>().HasData(
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.ADMIN, Libelle = "securite.utilisateur.typeUtilisateur.values.Admin" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.GEST, Libelle = "securite.utilisateur.typeUtilisateur.values.Gestionnaire" },
            new TypeUtilisateur { Code = TypeUtilisateur.Codes.CLIENT, Libelle = "securite.utilisateur.typeUtilisateur.values.Client" });

        AddComments(modelBuilder);
        AddResources(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void AddComments(ModelBuilder modelBuilder);

    partial void AddResources(ModelBuilder modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

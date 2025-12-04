////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Restaurant;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// DbContext généré pour Entity Framework Core.
/// </summary>
public partial class TopModelSampleDbContext(DbContextOptions<TopModelSampleDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Accès à l'entité AvisClient.
    /// </summary>
    public DbSet<AvisClient> AvisClients { get; set; }

    /// <summary>
    /// Accès à l'entité CategoriePlat.
    /// </summary>
    public DbSet<CategoriePlat> CategoriePlats { get; set; }

    /// <summary>
    /// Accès à l'entité Client.
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Accès à l'entité Commande.
    /// </summary>
    public DbSet<Commande> Commandes { get; set; }

    /// <summary>
    /// Accès à l'entité CommandeExport.
    /// </summary>
    public DbSet<CommandeExport> CommandeExports { get; set; }

    /// <summary>
    /// Accès à l'entité Employe.
    /// </summary>
    public DbSet<Employe> Employes { get; set; }

    /// <summary>
    /// Accès à l'entité LigneCommande.
    /// </summary>
    public DbSet<LigneCommande> LigneCommandes { get; set; }

    /// <summary>
    /// Accès à l'entité Menu.
    /// </summary>
    public DbSet<Menu> Menus { get; set; }

    /// <summary>
    /// Accès à l'entité MenuPlat.
    /// </summary>
    public DbSet<MenuPlat> MenuPlats { get; set; }

    /// <summary>
    /// Accès à l'entité Personne.
    /// </summary>
    public DbSet<Personne> Personnes { get; set; }

    /// <summary>
    /// Accès à l'entité Plat.
    /// </summary>
    public DbSet<Plat> Plats { get; set; }

    /// <summary>
    /// Accès à l'entité Promotion.
    /// </summary>
    public DbSet<Promotion> Promotions { get; set; }

    /// <summary>
    /// Accès à l'entité PromotionPlat.
    /// </summary>
    public DbSet<PromotionPlat> PromotionPlats { get; set; }

    /// <summary>
    /// Accès à l'entité Reservation.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; }

    /// <summary>
    /// Accès à l'entité Restaurant.
    /// </summary>
    public DbSet<Restaurant> Restaurants { get; set; }

    /// <summary>
    /// Accès à l'entité StatutCommande.
    /// </summary>
    public DbSet<StatutCommande> StatutCommandes { get; set; }

    /// <summary>
    /// Accès à l'entité TableClient.
    /// </summary>
    public DbSet<TableClient> TableClients { get; set; }

    /// <summary>
    /// Personalisation du modèle.
    /// </summary>
    /// <param name="modelBuilder">L'objet de construction du modèle.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriePlat>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Commande>().Property(p => p.StatutCommandeCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<CommandeExport>().Property(p => p.StatutCommandeCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Plat>().Property(p => p.CategoriePlatCodeCategoriePlat).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<StatutCommande>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<AvisClient>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientIdClient).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AvisClient>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<TableClient>().WithMany().HasForeignKey(p => p.TableClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<StatutCommande>().WithMany().HasForeignKey(p => p.StatutCommandeCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeExport>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeExport>().HasOne<TableClient>().WithMany().HasForeignKey(p => p.TableClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeExport>().HasOne<StatutCommande>().WithMany().HasForeignKey(p => p.StatutCommandeCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Employe>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne<Commande>().WithMany().HasForeignKey(p => p.CommandeId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Menu>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne<Menu>().WithMany().HasForeignKey(p => p.MenuIdMenu).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatIdPlat).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne<CategoriePlat>().WithMany().HasForeignKey(p => p.CategoriePlatCodeCategoriePlat).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PromotionPlat>().HasOne<Promotion>().WithMany().HasForeignKey(p => p.PromotionIdPromotion).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PromotionPlat>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatIdPlat).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientIdClient).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<TableClient>().WithMany().HasForeignKey(p => p.TableClientIdTable).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TableClient>().HasOne<Restaurant>().WithMany().HasForeignKey(p => p.RestaurantIdRestaurant).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AvisClient>().HasIndex(p => new { p.ClientIdClient, p.RestaurantIdRestaurant, p.DateAvis }).IsUnique();
        modelBuilder.Entity<Employe>().HasIndex(p => p.Matricule).IsUnique();
        modelBuilder.Entity<LigneCommande>().HasIndex(p => new { p.CommandeId, p.PlatId }).IsUnique();
        modelBuilder.Entity<MenuPlat>().HasIndex(p => new { p.MenuIdMenu, p.Ordre }).IsUnique();
        modelBuilder.Entity<MenuPlat>().HasIndex(p => new { p.MenuIdMenu, p.PlatIdPlat }).IsUnique();
        modelBuilder.Entity<PromotionPlat>().HasIndex(p => new { p.PromotionIdPromotion, p.PlatIdPlat }).IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex(p => new { p.TableClientIdTable, p.DateReservation }).IsUnique();
        modelBuilder.Entity<TableClient>().HasIndex(p => new { p.RestaurantIdRestaurant, p.Numero }).IsUnique();

        modelBuilder.Entity<CategoriePlat>().HasData(
            new CategoriePlat { Code = CategoriePlat.Codes.ENTREE, Libelle = "restaurant.categoriePlat.values.Entree" },
            new CategoriePlat { Code = CategoriePlat.Codes.PLAT, Libelle = "restaurant.categoriePlat.values.Plat" },
            new CategoriePlat { Code = CategoriePlat.Codes.DESSERT, Libelle = "restaurant.categoriePlat.values.Dessert" },
            new CategoriePlat { Code = CategoriePlat.Codes.BOISSON, Libelle = "restaurant.categoriePlat.values.Boisson" });
        modelBuilder.Entity<StatutCommande>().HasData(
            new StatutCommande { Code = StatutCommande.Codes.EN_ATT, Libelle = "restaurant.statutCommande.values.EnAttente" },
            new StatutCommande { Code = StatutCommande.Codes.EN_PREP, Libelle = "restaurant.statutCommande.values.EnPreparation" },
            new StatutCommande { Code = StatutCommande.Codes.PRETE, Libelle = "restaurant.statutCommande.values.Prete" },
            new StatutCommande { Code = StatutCommande.Codes.SERVIE, Libelle = "restaurant.statutCommande.values.Servie" },
            new StatutCommande { Code = StatutCommande.Codes.ANNULE, Libelle = "restaurant.statutCommande.values.Annulee" });

        AddComments(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void AddComments(ModelBuilder modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

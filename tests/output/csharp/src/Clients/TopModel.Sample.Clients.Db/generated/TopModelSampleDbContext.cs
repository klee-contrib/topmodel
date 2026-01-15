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
    /// Accès à l'entité CommandeHistorique.
    /// </summary>
    public DbSet<CommandeHistorique> CommandeHistoriques { get; set; }

    /// <summary>
    /// Accès à l'entité Employe.
    /// </summary>
    public DbSet<Employe> Employes { get; set; }

    /// <summary>
    /// Accès à l'entité LigneCommande.
    /// </summary>
    public DbSet<LigneCommande> LigneCommandes { get; set; }

    /// <summary>
    /// Accès à l'entité LigneCommandeHistorique.
    /// </summary>
    public DbSet<LigneCommandeHistorique> LigneCommandeHistoriques { get; set; }

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
    /// Accès à l'entité Reservation.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; }

    /// <summary>
    /// Accès à l'entité Restaurant.
    /// </summary>
    public DbSet<Models.Restaurant.Restaurant> Restaurants { get; set; }

    /// <summary>
    /// Accès à l'entité StatutCommande.
    /// </summary>
    public DbSet<StatutCommande> StatutCommandes { get; set; }

    /// <summary>
    /// Accès à l'entité TableRestaurant.
    /// </summary>
    public DbSet<TableRestaurant> TableRestaurants { get; set; }

    /// <summary>
    /// Personalisation du modèle.
    /// </summary>
    /// <param name="modelBuilder">L'objet de construction du modèle.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriePlat>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Commande>().Property(p => p.StatutCommandeCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<CommandeHistorique>().Property(p => p.StatutCommandeCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Plat>().Property(p => p.CategoriePlatCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<StatutCommande>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<MenuPlat>().HasKey(p => new { p.MenuId, p.PlatId });

        modelBuilder.Entity<AvisClient>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AvisClient>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<Reservation>().WithMany().HasForeignKey(p => p.ReservationId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<StatutCommande>().WithMany().HasForeignKey(p => p.StatutCommandeCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<AvisClient>().WithOne().HasForeignKey<Commande>(p => p.AvisClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<Reservation>().WithMany().HasForeignKey(p => p.ReservationId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<StatutCommande>().WithMany().HasForeignKey(p => p.StatutCommandeCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<AvisClient>().WithOne().HasForeignKey<CommandeHistorique>(p => p.AvisClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Employe>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne<Commande>().WithMany().HasForeignKey(p => p.CommandeId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne<CommandeHistorique>().WithMany().HasForeignKey(p => p.CommandeHistoriqueId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Menu>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne<Menu>().WithMany().HasForeignKey(p => p.MenuId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne<CategoriePlat>().WithMany().HasForeignKey(p => p.CategoriePlatCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne<Plat>().WithOne().HasForeignKey<Promotion>(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TableRestaurant>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AvisClient>().HasIndex(p => new { p.ClientId, p.RestaurantId, p.DateAvis }).IsUnique();
        modelBuilder.Entity<Employe>().HasIndex(p => p.Matricule).IsUnique();
        modelBuilder.Entity<LigneCommande>().HasIndex(p => new { p.CommandeId, p.PlatId }).IsUnique();
        modelBuilder.Entity<MenuPlat>().HasIndex(p => new { p.MenuId, p.Ordre }).IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex(p => new { p.TableId, p.DateReservation }).IsUnique();
        modelBuilder.Entity<TableRestaurant>().HasIndex(p => new { p.RestaurantId, p.Numero }).IsUnique();

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Common;
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
    /// Accès à l'entité CategoriePlatRegion.
    /// </summary>
    public DbSet<CategoriePlatRegion> CategoriePlatRegions { get; set; }

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
    /// Accès à l'entité Departement.
    /// </summary>
    public DbSet<Departement> Departements { get; set; }

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
    /// Accès à l'entité Region.
    /// </summary>
    public DbSet<Region> Regions { get; set; }

    /// <summary>
    /// Accès à l'entité Reservation.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; }

    /// <summary>
    /// Accès à l'entité Restaurant.
    /// </summary>
    public DbSet<Models.Restaurant.Restaurant> Restaurants { get; set; }

    /// <summary>
    /// Accès à l'entité TableRestaurant.
    /// </summary>
    public DbSet<TableRestaurant> TableRestaurants { get; set; }

    /// <summary>
    /// Accès à l'entité Translation.
    /// </summary>
    public DbSet<Translation> Translations { get; set; }

    /// <summary>
    /// Personalisation du modèle.
    /// </summary>
    /// <param name="modelBuilder">L'objet de construction du modèle.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriePlat>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<CategoriePlatRegion>().Property(p => p.RegionCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<CategoriePlatRegion>().Property("CategoriePlatCode").HasMaxLength(10);
        modelBuilder.Entity<Commande>().Property(p => p.StatutCommande).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<CommandeHistorique>().Property(p => p.StatutCommande).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Departement>().Property(p => p.RegionCode).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Plat>().Property("CategoriePlatCode").HasMaxLength(10);
        modelBuilder.Entity<Region>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<AvisClient>().HasOne(p => p.Client).WithMany(p => p.AvisClients).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AvisClient>().HasOne(p => p.Restaurant).WithMany(p => p.AvisClients).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CategoriePlatRegion>().HasOne<Region>().WithMany().HasForeignKey(p => p.RegionCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CategoriePlatRegion>().HasOne(p => p.CategoriePlat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.Reservation).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.AvisClient).WithOne().HasForeignKey<Commande>("AvisClientId").OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<Client>().WithMany().HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<Reservation>().WithMany().HasForeignKey(p => p.ReservationId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne<AvisClient>().WithOne().HasForeignKey<CommandeHistorique>(p => p.AvisClientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Departement>().HasOne<Region>().WithMany().HasForeignKey(p => p.RegionCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Employe>().HasOne(p => p.Restaurant).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne(p => p.Commande).WithMany(p => p.Lignes).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne(p => p.Plat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne<Plat>().WithMany().HasForeignKey(p => p.PlatId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne<CommandeHistorique>().WithMany().HasForeignKey(p => p.CommandeHistoriqueId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Menu>().HasOne(p => p.Restaurant).WithMany(p => p.Menus).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne(p => p.Menu).WithMany(p => p.Plats).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne(p => p.Plat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Personne>().HasOne<Departement>().WithMany().HasForeignKey(p => p.DepartementCode).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne(p => p.CategoriePlat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne(p => p.Restaurant).WithMany(p => p.Plats).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne(p => p.Plat).WithOne(p => p.Promotion).HasForeignKey<Promotion>("PlatId").OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne(p => p.Restaurant).WithMany(p => p.Promotions).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(p => p.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne<TableRestaurant>().WithMany().HasForeignKey(p => p.TableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(p => p.Restaurant).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TableRestaurant>().HasOne<Models.Restaurant.Restaurant>().WithMany().HasForeignKey(p => p.RestaurantId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CategoriePlatRegion>().HasKey("RegionCode", "CategoriePlatCode");
        modelBuilder.Entity<MenuPlat>().HasKey("MenuId", "PlatId");
        modelBuilder.Entity<Promotion>().HasKey("PlatId");

        modelBuilder.Entity<AvisClient>().HasIndex("ClientId", "RestaurantId", "DateAvis").IsUnique();
        modelBuilder.Entity<CategoriePlat>().HasIndex(p => p.Ordre).IsUnique();
        modelBuilder.Entity<Employe>().HasIndex(p => p.Telephone);
        modelBuilder.Entity<Employe>().HasIndex(p => p.Matricule).IsUnique();
        modelBuilder.Entity<LigneCommande>().HasIndex("CommandeId", "PlatId").IsUnique();
        modelBuilder.Entity<MenuPlat>().HasIndex("MenuId", "Ordre").IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex(p => new { p.TableId, p.DateReservation }).IsUnique();
        modelBuilder.Entity<TableRestaurant>().HasIndex(p => new { p.RestaurantId, p.Numero }).IsUnique();

        modelBuilder.Entity<AvisClient>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<AvisClient>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<CategoriePlatRegion>().Property("CategoriePlatCode").HasColumnName("cat_code");
        modelBuilder.Entity<Commande>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<Commande>().Property("ReservationId").HasColumnName("rev_id");
        modelBuilder.Entity<Commande>().Property("StatutCommande").HasColumnName("stc_code");
        modelBuilder.Entity<Commande>().Property("AvisClientId").HasColumnName("avi_id");
        modelBuilder.Entity<Employe>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<LigneCommande>().Property("CommandeId").HasColumnName("com_id");
        modelBuilder.Entity<LigneCommande>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<Menu>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<MenuPlat>().Property("MenuId").HasColumnName("men_id");
        modelBuilder.Entity<MenuPlat>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<Plat>().Property("CategoriePlatCode").HasColumnName("cat_code");
        modelBuilder.Entity<Plat>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<Promotion>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<Promotion>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<Reservation>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<Reservation>().Property("RestaurantId").HasColumnName("res_id");

        modelBuilder.Entity<CategoriePlat>().HasIndex(p => p.Libelle);
        modelBuilder.Entity<Departement>().HasIndex(p => p.Libelle);
        modelBuilder.Entity<Region>().HasIndex(p => p.Libelle);

        modelBuilder.Entity<CategoriePlat>().HasData(CategoriePlat.Values);
        modelBuilder.Entity<CategoriePlatRegion>().HasData(CategoriePlatRegion.Values);
        modelBuilder.Entity<Departement>().HasData(Departement.Values);
        modelBuilder.Entity<Region>().HasData(
            new Region { Code = Region.Codes.IDF, Libelle = "restaurant.region.values.Idf" });

        AddComments(modelBuilder);
        AddResources(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void AddComments(ModelBuilder modelBuilder);

    partial void AddResources(ModelBuilder modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

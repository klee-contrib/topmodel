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
        modelBuilder.Entity<Commande>().Property("StatutCommandeCode").HasMaxLength(10);
        modelBuilder.Entity<CommandeHistorique>().Property("StatutCommandeCode").HasMaxLength(10);
        modelBuilder.Entity<Plat>().Property("CategoriePlatCode").HasMaxLength(10);
        modelBuilder.Entity<StatutCommande>().Property(p => p.Code).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<MenuPlat>().HasKey("MenuId", "PlatId");
        modelBuilder.Entity<Promotion>().HasKey("PlatId");

        modelBuilder.Entity<AvisClient>().HasOne(p => p.Client).WithMany(p => p.AvisClients).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AvisClient>().HasOne(p => p.Restaurant).WithMany(p => p.AvisClients).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.Table).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.Reservation).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.StatutCommande).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Commande>().HasOne(p => p.AvisClient).WithOne().HasForeignKey<Commande>("AvisClientId").OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne(p => p.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne(p => p.Table).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne(p => p.Reservation).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne(p => p.StatutCommande).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CommandeHistorique>().HasOne(p => p.AvisClient).WithOne().HasForeignKey<CommandeHistorique>("AvisClientId").OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Employe>().HasOne(p => p.Restaurant).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne(p => p.Commande).WithMany(p => p.Lignes).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommande>().HasOne(p => p.Plat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne(p => p.Plat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<LigneCommandeHistorique>().HasOne(p => p.CommandeHistorique).WithMany(p => p.Lignes).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Menu>().HasOne(p => p.Restaurant).WithMany(p => p.Menus).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne(p => p.Menu).WithMany(p => p.Plats).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MenuPlat>().HasOne(p => p.Plat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne(p => p.CategoriePlat).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Plat>().HasOne(p => p.Restaurant).WithMany(p => p.Plats).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne(p => p.Plat).WithOne().HasForeignKey<Promotion>("PlatId").OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Promotion>().HasOne(p => p.Restaurant).WithMany(p => p.Promotions).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(p => p.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(p => p.Table).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reservation>().HasOne(p => p.Restaurant).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TableRestaurant>().HasOne(p => p.Restaurant).WithMany(p => p.Tables).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AvisClient>().HasIndex("ClientId", "RestaurantId", "DateAvis").IsUnique();
        modelBuilder.Entity<Employe>().HasIndex(p => p.Matricule).IsUnique();
        modelBuilder.Entity<LigneCommande>().HasIndex("CommandeId", "PlatId").IsUnique();
        modelBuilder.Entity<MenuPlat>().HasIndex("MenuId", "Ordre").IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex("TableId", "DateReservation").IsUnique();
        modelBuilder.Entity<TableRestaurant>().HasIndex("RestaurantId", "Numero").IsUnique();

        modelBuilder.Entity<AvisClient>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<AvisClient>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<Commande>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<Commande>().Property("TableId").HasColumnName("tab_id");
        modelBuilder.Entity<Commande>().Property("ReservationId").HasColumnName("rev_id");
        modelBuilder.Entity<Commande>().Property("StatutCommandeCode").HasColumnName("stc_code");
        modelBuilder.Entity<Commande>().Property("AvisClientId").HasColumnName("avi_id");
        modelBuilder.Entity<CommandeHistorique>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<CommandeHistorique>().Property("TableId").HasColumnName("tab_id");
        modelBuilder.Entity<CommandeHistorique>().Property("ReservationId").HasColumnName("rev_id");
        modelBuilder.Entity<CommandeHistorique>().Property("StatutCommandeCode").HasColumnName("stc_code");
        modelBuilder.Entity<CommandeHistorique>().Property("AvisClientId").HasColumnName("avi_id");
        modelBuilder.Entity<Employe>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<LigneCommande>().Property("CommandeId").HasColumnName("com_id");
        modelBuilder.Entity<LigneCommande>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<LigneCommandeHistorique>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<LigneCommandeHistorique>().Property("CommandeHistoriqueId").HasColumnName("com_id");
        modelBuilder.Entity<Menu>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<MenuPlat>().Property("MenuId").HasColumnName("men_id");
        modelBuilder.Entity<MenuPlat>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<Plat>().Property("CategoriePlatCode").HasColumnName("cat_code");
        modelBuilder.Entity<Plat>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<Promotion>().Property("PlatId").HasColumnName("pla_id");
        modelBuilder.Entity<Promotion>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<Reservation>().Property("ClientId").HasColumnName("per_id");
        modelBuilder.Entity<Reservation>().Property("TableId").HasColumnName("tab_id");
        modelBuilder.Entity<Reservation>().Property("RestaurantId").HasColumnName("res_id");
        modelBuilder.Entity<TableRestaurant>().Property("RestaurantId").HasColumnName("res_id");

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

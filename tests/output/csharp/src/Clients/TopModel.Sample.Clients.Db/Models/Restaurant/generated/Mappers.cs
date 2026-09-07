////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Mappers pour le module 'Restaurant'.
/// </summary>
public static class Mappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'AvisClientRead'.
    /// </summary>
    /// <param name="avisClient">Instance de 'AvisClient'.</param>
    /// <returns>Une nouvelle instance de 'AvisClientRead'.</returns>
    public static AvisClientRead CreateAvisClientRead(AvisClient avisClient)
    {
        ArgumentNullException.ThrowIfNull(avisClient);

        return new AvisClientRead
        {
            Id = avisClient.Id,
            Note = avisClient.Note,
            Commentaire = avisClient.Commentaire,
            DateAvis = avisClient.DateAvis,
            Approuve = avisClient.Approuve,
            ClientId = avisClient.Client?.Id,
            RestaurantId = avisClient.Restaurant?.Id,
            DateCreation = avisClient.DateCreation,
            NombreVues = avisClient.NombreVues
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ClientItem'.
    /// </summary>
    /// <param name="client">Instance de 'Client'.</param>
    /// <returns>Une nouvelle instance de 'ClientItem'.</returns>
    public static ClientItem CreateClientItem(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        return new ClientItem
        {
            Id = client.Id,
            Nom = client.Nom,
            Prenom = client.Prenom
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ClientRead'.
    /// </summary>
    /// <param name="client">Instance de 'Client'.</param>
    /// <returns>Une nouvelle instance de 'ClientRead'.</returns>
    public static ClientRead CreateClientRead(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        return new ClientRead
        {
            Id = client.Id,
            Nom = client.Nom,
            Prenom = client.Prenom,
            DepartementCode = client.DepartementCode,
            DateCreation = client.DateCreation,
            Email = client.Email,
            SwileCardId = client.SwileCard,
            AvisClients = client.AvisClients.Select(p => p.Id!.Value).ToList()
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'CommandeRead'.
    /// </summary>
    /// <param name="commande">Instance de 'Commande'.</param>
    /// <returns>Une nouvelle instance de 'CommandeRead'.</returns>
    public static CommandeRead CreateCommandeRead(Commande commande)
    {
        ArgumentNullException.ThrowIfNull(commande);

        return new CommandeRead
        {
            Id = commande.Id,
            DateCommande = commande.DateCommande,
            DateLivraison = commande.DateLivraison,
            MontantTotal = commande.MontantTotal,
            TableId = commande.TableId,
            StatutCommande = commande.StatutCommande,
            AvisClientId = commande.AvisClient?.Id,
            DateCreation = commande.DateCreation,
            Client = commande.Client != null ? CreateClientRead(commande.Client) : new(),
            Lignes = commande.Lignes.Select(CreateLigneCommandeRead).ToList(),
            Reservation = commande.Reservation != null ? CreateReservationRead(commande.Reservation) : null
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'EmployeRead'.
    /// </summary>
    /// <param name="employe">Instance de 'Employe'.</param>
    /// <returns>Une nouvelle instance de 'EmployeRead'.</returns>
    public static EmployeRead CreateEmployeRead(Employe employe)
    {
        ArgumentNullException.ThrowIfNull(employe);

        return new EmployeRead
        {
            Id = employe.Id,
            Nom = employe.Nom,
            Prenom = employe.Prenom,
            DepartementCode = employe.DepartementCode,
            DateCreation = employe.DateCreation,
            Telephone = employe.Telephone,
            DateNaissance = employe.DateNaissance,
            Matricule = employe.Matricule,
            DateEmbauche = employe.DateEmbauche,
            Salaire = employe.Salaire,
            RestaurantId = employe.Restaurant?.Id
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'LigneCommandeRead'.
    /// </summary>
    /// <param name="ligneCommande">Instance de 'LigneCommande'.</param>
    /// <returns>Une nouvelle instance de 'LigneCommandeRead'.</returns>
    public static LigneCommandeRead CreateLigneCommandeRead(LigneCommande ligneCommande)
    {
        ArgumentNullException.ThrowIfNull(ligneCommande);

        return new LigneCommandeRead
        {
            Id = ligneCommande.Id,
            Quantite = ligneCommande.Quantite,
            PrixUnitaire = ligneCommande.PrixUnitaire,
            PrixTotal = ligneCommande.PrixTotal,
            CommandeId = ligneCommande.Commande?.Id,
            PlatId = ligneCommande.Plat?.Id,
            DateCreation = ligneCommande.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'MenuRead'.
    /// </summary>
    /// <param name="menu">Instance de 'Menu'.</param>
    /// <returns>Une nouvelle instance de 'MenuRead'.</returns>
    public static MenuRead CreateMenuRead(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        return new MenuRead
        {
            Id = menu.Id,
            Nom = menu.Nom,
            Description = menu.Description,
            Prix = menu.Prix,
            Disponible = menu.Disponible,
            DateDebut = menu.DateDebut,
            DateFin = menu.DateFin,
            RestaurantId = menu.Restaurant?.Id,
            DateCreation = menu.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'IPlatItem'.
    /// </summary>
    /// <param name="plat">Instance de 'Plat'.</param>
    /// <returns>Une nouvelle instance de 'IPlatItem'.</returns>
    public static IPlatItem CreatePlatItem<T>(Plat plat)
        where T : IPlatItem, new()
    {
        ArgumentNullException.ThrowIfNull(plat);

        return new T
        {
            Id = plat.Id,
            Nom = plat.Nom,
            Prix = plat.Prix,
            Disponible = plat.Disponible,
            CategoriePlatCode = plat.CategoriePlat?.Code
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'PlatRead'.
    /// </summary>
    /// <param name="plat">Instance de 'Plat'.</param>
    /// <returns>Une nouvelle instance de 'PlatRead'.</returns>
    public static PlatRead CreatePlatRead(Plat plat)
    {
        ArgumentNullException.ThrowIfNull(plat);

        return new PlatRead
        {
            Id = plat.Id,
            Nom = plat.Nom,
            Description = plat.Description,
            Prix = plat.Prix,
            Disponible = plat.Disponible,
            CategoriePlatCode = plat.CategoriePlat?.Code,
            RestaurantId = plat.Restaurant?.Id,
            DateCreation = plat.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'PromotionRead'.
    /// </summary>
    /// <param name="promotion">Instance de 'Promotion'.</param>
    /// <returns>Une nouvelle instance de 'PromotionRead'.</returns>
    public static PromotionRead CreatePromotionRead(Promotion promotion)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        return new PromotionRead
        {
            PlatId = promotion.Plat?.Id,
            Libelle = promotion.Libelle,
            PourcentageReduction = promotion.PourcentageReduction,
            DateDebut = promotion.DateDebut,
            DateFin = promotion.DateFin,
            Active = promotion.Active,
            RestaurantId = promotion.Restaurant?.Id,
            DateCreation = promotion.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ReservationRead'.
    /// </summary>
    /// <param name="reservation">Instance de 'Reservation'.</param>
    /// <returns>Une nouvelle instance de 'ReservationRead'.</returns>
    public static ReservationRead CreateReservationRead(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        return new ReservationRead
        {
            Id = reservation.Id,
            DateReservation = reservation.DateReservation,
            NombrePersonnes = reservation.NombrePersonnes,
            Commentaire = reservation.Commentaire,
            Confirmee = reservation.Confirmee,
            ClientId = reservation.Client?.Id,
            TableId = reservation.TableId,
            RestaurantId = reservation.Restaurant?.Id,
            DateCreation = reservation.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'RestaurantAvecStatistiques'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <param name="tables">Tables.</param>
    /// <param name="nombrePlats">Nombre de plats.</param>
    /// <param name="noteMoyenne">Note moyenne.</param>
    /// <returns>Une nouvelle instance de 'RestaurantAvecStatistiques'.</returns>
    public static RestaurantAvecStatistiques CreateRestaurantAvecStatistiques(Restaurant restaurant, ICollection<Table> tables, int? nombrePlats = null, decimal? noteMoyenne = null)
    {
        ArgumentNullException.ThrowIfNull(restaurant);
        ArgumentNullException.ThrowIfNull(tables);

        return new RestaurantAvecStatistiques
        {
            Id = restaurant.Id,
            Nom = restaurant.Nom,
            Adresse = restaurant.Adresse,
            Telephone = restaurant.Telephone,
            Menus = restaurant.Menus.Select(p => p.Id!.Value).ToList(),
            Plats = restaurant.Plats.Select(p => p.Id!.Value).ToList(),
            Promotions = restaurant.Promotions.Where(p => p.Plat != null).Select(p => p.Plat!.Id!.Value).ToList(),
            AvisClients = restaurant.AvisClients.Select(p => p.Id!.Value).ToList(),
            TableIds = restaurant.TableIds,
            DateCreation = restaurant.DateCreation,
            Tables = tables.Select(CreateTableRead).ToList(),
            NombrePlats = nombrePlats,
            NoteMoyenne = noteMoyenne
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'RestaurantRead'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <returns>Une nouvelle instance de 'RestaurantRead'.</returns>
    public static RestaurantRead CreateRestaurantRead(Restaurant restaurant)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        return new RestaurantRead
        {
            Id = restaurant.Id,
            Nom = restaurant.Nom,
            Adresse = restaurant.Adresse,
            Telephone = restaurant.Telephone,
            Menus = restaurant.Menus.Select(p => p.Id!.Value).ToList(),
            Plats = restaurant.Plats.Select(p => p.Id!.Value).ToList(),
            Promotions = restaurant.Promotions.Where(p => p.Plat != null).Select(p => p.Plat!.Id!.Value).ToList(),
            AvisClients = restaurant.AvisClients.Select(p => p.Id!.Value).ToList(),
            DateCreation = restaurant.DateCreation
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'StatistiquesRestaurant'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <param name="nombreCommandes">Nombre de commandes.</param>
    /// <param name="chiffreAffaires">Chiffre d'affaires.</param>
    /// <param name="nombreClients">Nombre de clients.</param>
    /// <param name="noteMoyenne">Note moyenne.</param>
    /// <returns>Une nouvelle instance de 'StatistiquesRestaurant'.</returns>
    public static StatistiquesRestaurant CreateStatistiquesRestaurant(Restaurant restaurant, int? nombreCommandes = null, decimal? chiffreAffaires = null, int? nombreClients = null, decimal? noteMoyenne = null)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        return new StatistiquesRestaurant
        {
            NombreCommandes = nombreCommandes,
            ChiffreAffaires = chiffreAffaires,
            NombreClients = nombreClients,
            NoteMoyenne = noteMoyenne
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'TableRead'.
    /// </summary>
    /// <param name="table">Instance de 'Table'.</param>
    /// <returns>Une nouvelle instance de 'TableRead'.</returns>
    public static TableRead CreateTableRead(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new TableRead
        {
            Id = table.Id,
            Numero = table.Numero,
            Capacite = table.Capacite,
            Disponible = table.Disponible,
            RestaurantId = table.RestaurantId,
            DateCreation = table.DateCreation
        };
    }

    /// <summary>
    /// Mappe 'AvisClientWrite' vers 'AvisClient'.
    /// </summary>
    /// <param name="source">Instance de 'AvisClientWrite'.</param>
    /// <param name="dateAvis">Date de l'avis.</param>
    /// <param name="client">Client ayant donné l'avis.</param>
    /// <param name="restaurant">Restaurant concerné par l'avis.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'AvisClient'.</returns>
    public static AvisClient ToAvisClient(this AvisClientWrite source, DateTime? dateAvis = null, Client? client = null, Restaurant? restaurant = null, DateTime? dateCreation = null)
    {
        return new AvisClient
        {
            Note = source.Note,
            Commentaire = source.Commentaire,
            Approuve = source.Approuve,
            DateAvis = dateAvis,
            Client = client,
            Restaurant = restaurant,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'AvisClientWrite' vers 'AvisClient'.
    /// </summary>
    /// <param name="source">Instance de 'AvisClientWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'AvisClient'.</param>
    /// <returns>L'instance pré-existante de 'AvisClient'.</returns>
    public static AvisClient ToAvisClient(this AvisClientWrite source, AvisClient dest)
    {
        dest.Note = source.Note;
        dest.Commentaire = source.Commentaire;
        dest.Approuve = source.Approuve;
        return dest;
    }

    /// <summary>
    /// Mappe 'ClientWrite' vers 'Client'.
    /// </summary>
    /// <param name="source">Instance de 'ClientWrite'.</param>
    /// <returns>Une nouvelle instance de 'Client'.</returns>
    public static Client ToClient(this ClientWrite source)
    {
        return new Client
        {
            Nom = source.Nom,
            Prenom = source.Prenom,
            DepartementCode = source.DepartementCode,
            Email = source.Email
        };
    }

    /// <summary>
    /// Mappe 'ClientWrite' vers 'Client'.
    /// </summary>
    /// <param name="source">Instance de 'ClientWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Client'.</param>
    /// <returns>L'instance pré-existante de 'Client'.</returns>
    public static Client ToClient(this ClientWrite source, Client dest)
    {
        dest.Nom = source.Nom;
        dest.Prenom = source.Prenom;
        dest.DepartementCode = source.DepartementCode;
        dest.Email = source.Email;
        return dest;
    }

    /// <summary>
    /// Mappe 'CommandeWrite' vers 'Commande'.
    /// </summary>
    /// <param name="source">Instance de 'CommandeWrite'.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Commande'.</returns>
    public static Commande ToCommande(this CommandeWrite source, DateTime? dateCreation = null)
    {
        return new Commande
        {
            DateCommande = source.DateCommande,
            DateLivraison = source.DateLivraison,
            MontantTotal = source.MontantTotal,
            TableId = source.TableId,
            StatutCommande = source.StatutCommande,
            Client = source.Client?.ToClient(),
            Lignes = source.Lignes.Select(p => p.ToLigneCommande()).ToList(),
            Reservation = source.Reservation?.ToReservation(),
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'CommandeWrite' vers 'Commande'.
    /// </summary>
    /// <param name="source">Instance de 'CommandeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Commande'.</param>
    /// <returns>L'instance pré-existante de 'Commande'.</returns>
    public static Commande ToCommande(this CommandeWrite source, Commande dest)
    {
        dest.DateCommande = source.DateCommande;
        dest.DateLivraison = source.DateLivraison;
        dest.MontantTotal = source.MontantTotal;
        dest.TableId = source.TableId;
        dest.StatutCommande = source.StatutCommande;
        dest.Client = source.Client?.ToClient();
        dest.Lignes = source.Lignes.Select(p => p.ToLigneCommande()).ToList();
        dest.Reservation = source.Reservation?.ToReservation();
        return dest;
    }

    /// <summary>
    /// Mappe 'EmployeWrite' vers 'Employe'.
    /// </summary>
    /// <param name="source">Instance de 'EmployeWrite'.</param>
    /// <param name="restaurant">Restaurant où travaille l'employé.</param>
    /// <returns>Une nouvelle instance de 'Employe'.</returns>
    public static Employe ToEmploye(this EmployeWrite source, Restaurant? restaurant = null)
    {
        return new Employe
        {
            Nom = source.Nom,
            Prenom = source.Prenom,
            DepartementCode = source.DepartementCode,
            Telephone = source.Telephone,
            DateNaissance = source.DateNaissance,
            Matricule = source.Matricule,
            DateEmbauche = source.DateEmbauche,
            Salaire = source.Salaire,
            Restaurant = restaurant
        };
    }

    /// <summary>
    /// Mappe 'EmployeWrite' vers 'Employe'.
    /// </summary>
    /// <param name="source">Instance de 'EmployeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Employe'.</param>
    /// <returns>L'instance pré-existante de 'Employe'.</returns>
    public static Employe ToEmploye(this EmployeWrite source, Employe dest)
    {
        dest.Nom = source.Nom;
        dest.Prenom = source.Prenom;
        dest.DepartementCode = source.DepartementCode;
        dest.Telephone = source.Telephone;
        dest.DateNaissance = source.DateNaissance;
        dest.Matricule = source.Matricule;
        dest.DateEmbauche = source.DateEmbauche;
        dest.Salaire = source.Salaire;
        return dest;
    }

    /// <summary>
    /// Mappe 'LigneCommandeWrite' vers 'LigneCommande'.
    /// </summary>
    /// <param name="source">Instance de 'LigneCommandeWrite'.</param>
    /// <param name="commande">Commande à laquelle appartient la ligne.</param>
    /// <param name="plat">Plat commandé.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'LigneCommande'.</returns>
    public static LigneCommande ToLigneCommande(this LigneCommandeWrite source, Commande? commande = null, Plat? plat = null, DateTime? dateCreation = null)
    {
        return new LigneCommande
        {
            Quantite = source.Quantite,
            PrixUnitaire = source.PrixUnitaire,
            PrixTotal = source.PrixTotal,
            Commande = commande,
            Plat = plat,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'LigneCommandeWrite' vers 'LigneCommande'.
    /// </summary>
    /// <param name="source">Instance de 'LigneCommandeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'LigneCommande'.</param>
    /// <returns>L'instance pré-existante de 'LigneCommande'.</returns>
    public static LigneCommande ToLigneCommande(this LigneCommandeWrite source, LigneCommande dest)
    {
        dest.Quantite = source.Quantite;
        dest.PrixUnitaire = source.PrixUnitaire;
        dest.PrixTotal = source.PrixTotal;
        return dest;
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'Menu'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <param name="restaurant">Restaurant proposant ce menu.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Menu'.</returns>
    public static Menu ToMenu(this MenuWrite source, Restaurant? restaurant = null, DateTime? dateCreation = null)
    {
        return new Menu
        {
            Nom = source.Nom,
            Description = source.Description,
            Prix = source.Prix,
            Disponible = source.Disponible,
            DateDebut = source.DateDebut,
            DateFin = source.DateFin,
            Restaurant = restaurant,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'Menu'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Menu'.</param>
    /// <returns>L'instance pré-existante de 'Menu'.</returns>
    public static Menu ToMenu(this MenuWrite source, Menu dest)
    {
        dest.Nom = source.Nom;
        dest.Description = source.Description;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.DateDebut = source.DateDebut;
        dest.DateFin = source.DateFin;
        return dest;
    }

    /// <summary>
    /// Mappe 'IPlatItem' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'IPlatItem'.</param>
    /// <param name="restaurant">Restaurant proposant ce plat.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Plat'.</returns>
    public static Plat ToPlat(this IPlatItem source, Restaurant? restaurant = null, DateTime? dateCreation = null)
    {
        return new Plat
        {
            Id = source.Id,
            Nom = source.Nom,
            Prix = source.Prix,
            Disponible = source.Disponible,
            CategoriePlat = source.CategoriePlatCode != null ? CategoriePlat.GetValue(source.CategoriePlatCode.Value) : null,
            Restaurant = restaurant,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'IPlatItem' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'IPlatItem'.</param>
    /// <param name="dest">Instance pré-existante de 'Plat'.</param>
    /// <returns>L'instance pré-existante de 'Plat'.</returns>
    public static Plat ToPlat(this IPlatItem source, Plat dest)
    {
        dest.Id = source.Id;
        dest.Nom = source.Nom;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.CategoriePlat = source.CategoriePlatCode != null ? CategoriePlat.GetValue(source.CategoriePlatCode.Value) : null;
        return dest;
    }

    /// <summary>
    /// Mappe 'PlatWrite' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'PlatWrite'.</param>
    /// <param name="restaurant">Restaurant proposant ce plat.</param>
    /// <returns>Une nouvelle instance de 'Plat'.</returns>
    public static Plat ToPlat(this PlatWrite source, Restaurant? restaurant = null)
    {
        return new Plat
        {
            Nom = source.Nom,
            Description = source.Description,
            Prix = source.Prix,
            Disponible = source.Disponible,
            CategoriePlat = source.CategoriePlatCode != null ? CategoriePlat.GetValue(source.CategoriePlatCode.Value) : null,
            DateCreation = source.DateCreation,
            Restaurant = restaurant
        };
    }

    /// <summary>
    /// Mappe 'PlatWrite' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'PlatWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Plat'.</param>
    /// <returns>L'instance pré-existante de 'Plat'.</returns>
    public static Plat ToPlat(this PlatWrite source, Plat dest)
    {
        dest.Nom = source.Nom;
        dest.Description = source.Description;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.CategoriePlat = source.CategoriePlatCode != null ? CategoriePlat.GetValue(source.CategoriePlatCode.Value) : null;
        return dest;
    }

    /// <summary>
    /// Mappe 'PromotionWrite' vers 'Promotion'.
    /// </summary>
    /// <param name="source">Instance de 'PromotionWrite'.</param>
    /// <param name="plat">Plat concerné par la promotion.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Promotion'.</returns>
    public static Promotion ToPromotion(this PromotionWrite source, Plat? plat = null, DateTime? dateCreation = null)
    {
        return new Promotion
        {
            Libelle = source.Libelle,
            PourcentageReduction = source.PourcentageReduction,
            DateDebut = source.DateDebut,
            DateFin = source.DateFin,
            Active = source.Active,
            Plat = plat,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'PromotionWrite' vers 'Promotion'.
    /// </summary>
    /// <param name="source">Instance de 'PromotionWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Promotion'.</param>
    /// <returns>L'instance pré-existante de 'Promotion'.</returns>
    public static Promotion ToPromotion(this PromotionWrite source, Promotion dest)
    {
        dest.Libelle = source.Libelle;
        dest.PourcentageReduction = source.PourcentageReduction;
        dest.DateDebut = source.DateDebut;
        dest.DateFin = source.DateFin;
        dest.Active = source.Active;
        return dest;
    }

    /// <summary>
    /// Mappe 'ReservationWrite' vers 'Reservation'.
    /// </summary>
    /// <param name="source">Instance de 'ReservationWrite'.</param>
    /// <param name="client">Client ayant fait la réservation.</param>
    /// <param name="restaurant">Restaurant concerné par la réservation.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Reservation'.</returns>
    public static Reservation ToReservation(this ReservationWrite source, Client? client = null, Restaurant? restaurant = null, DateTime? dateCreation = null)
    {
        return new Reservation
        {
            DateReservation = source.DateReservation,
            NombrePersonnes = source.NombrePersonnes,
            Commentaire = source.Commentaire,
            Confirmee = source.Confirmee,
            TableId = source.TableId,
            Client = client,
            Restaurant = restaurant,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'ReservationWrite' vers 'Reservation'.
    /// </summary>
    /// <param name="source">Instance de 'ReservationWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Reservation'.</param>
    /// <returns>L'instance pré-existante de 'Reservation'.</returns>
    public static Reservation ToReservation(this ReservationWrite source, Reservation dest)
    {
        dest.DateReservation = source.DateReservation;
        dest.NombrePersonnes = source.NombrePersonnes;
        dest.Commentaire = source.Commentaire;
        dest.Confirmee = source.Confirmee;
        dest.TableId = source.TableId;
        return dest;
    }

    /// <summary>
    /// Mappe 'RestaurantWrite' vers 'Restaurant'.
    /// </summary>
    /// <param name="source">Instance de 'RestaurantWrite'.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Restaurant'.</returns>
    public static Restaurant ToRestaurant(this RestaurantWrite source, DateTime? dateCreation = null)
    {
        return new Restaurant
        {
            Nom = source.Nom,
            Adresse = source.Adresse,
            Telephone = source.Telephone,
            TableIds = source.Tables.Select(p => p.Id!.Value).ToList(),
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'RestaurantWrite' vers 'Restaurant'.
    /// </summary>
    /// <param name="source">Instance de 'RestaurantWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Restaurant'.</param>
    /// <returns>L'instance pré-existante de 'Restaurant'.</returns>
    public static Restaurant ToRestaurant(this RestaurantWrite source, Restaurant dest)
    {
        dest.Nom = source.Nom;
        dest.Adresse = source.Adresse;
        dest.Telephone = source.Telephone;
        dest.TableIds = source.Tables.Select(p => p.Id!.Value).ToList();
        return dest;
    }

    /// <summary>
    /// Mappe 'TableWrite' vers 'Table'.
    /// </summary>
    /// <param name="source">Instance de 'TableWrite'.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'Table'.</returns>
    public static Table ToTable(this TableWrite source, DateTime? dateCreation = null)
    {
        return new Table
        {
            Numero = source.Numero,
            Capacite = source.Capacite,
            Disponible = source.Disponible,
            RestaurantId = source.RestaurantId,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'TableWrite' vers 'Table'.
    /// </summary>
    /// <param name="source">Instance de 'TableWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Table'.</param>
    /// <returns>L'instance pré-existante de 'Table'.</returns>
    public static Table ToTable(this TableWrite source, Table dest)
    {
        dest.Numero = source.Numero;
        dest.Capacite = source.Capacite;
        dest.Disponible = source.Disponible;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }
}

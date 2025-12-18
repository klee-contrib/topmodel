////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export const restaurant = {
    avisClient: {
        approuve: "Approuve",
        clientId: "ClientId",
        commentaire: "Commentaire",
        dateAvis: "DateAvis",
        id: "Id",
        nombreVues: "NombreVues",
        note: "Note",
        restaurantId: "RestaurantId"
    },
    categoriePlat: {
        code: "Code",
        libelle: "Libelle",
        values: {
            Entree: "Entrée",
            Plat: "Plat principal",
            Dessert: "Dessert",
            Boisson: "Boisson"
        }
    },
    client: {
        avisClients: "AvisClients",
        email: "Email"
    },
    clientItem: {
        id: "Informations client",
        nom: "Informations client",
        nomComplet: "NomComplet",
        prenom: "Informations client"
    },
    commande: {
        clientId: "ClientId",
        dateCommande: "DateCommande",
        dateLivraison: "DateLivraison",
        id: "Id",
        lignes: "Lignes",
        montantTotal: "MontantTotal",
        reservationId: "ReservationId",
        statutCommandeCode: "StatutCommandeCode",
        tableId: "TableId"
    },
    commandeRead: {
        reservation: "Reservation"
    },
    employe: {
        dateEmbauche: "DateEmbauche",
        dateNaissance: "DateNaissance",
        matricule: "Matricule",
        restaurantId: "RestaurantId",
        salaire: "Salaire",
        telephone: "Telephone"
    },
    employeItem: {
        id: "Id",
        nom: "Nom",
        prenom: "Prenom"
    },
    ligneCommande: {
        commandeId: "CommandeId",
        id: "Id",
        platId: "PlatId",
        prixTotal: "PrixTotal",
        prixUnitaire: "PrixUnitaire",
        quantite: "Quantite"
    },
    menu: {
        dateDebut: "DateDebut",
        dateFin: "DateFin",
        description: "Description",
        disponible: "Disponible",
        id: "Id",
        nom: "Nom",
        prix: "Prix",
        restaurantId: "RestaurantId"
    },
    menuRead: {
        plats: "Plats"
    },
    personne: {
        id: "Id",
        nom: "Nom",
        prenom: "Prenom"
    },
    plat: {
        categoriePlatCode: "CategoriePlatCode",
        description: "Description",
        disponible: "Disponible",
        id: "Id",
        nom: "Nom",
        prix: "Prix",
        restaurantId: "RestaurantId"
    },
    promotion: {
        active: "Active",
        dateDebut: "DateDebut",
        dateFin: "DateFin",
        id: "Id",
        libelle: "Libelle",
        pourcentageReduction: "PourcentageReduction",
        restaurantId: "RestaurantId"
    },
    reservation: {
        clientId: "ClientId",
        commentaire: "Commentaire",
        confirmee: "Confirmee",
        dateReservation: "DateReservation",
        id: "Id",
        nombrePersonnes: "NombrePersonnes",
        restaurantId: "RestaurantId",
        tableId: "TableId"
    },
    restaurant: {
        adresse: "Adresse",
        avisClients: "AvisClients",
        id: "Id",
        menus: "Menus",
        nom: "Nom",
        plats: "Plats",
        promotions: "Promotions",
        tables: "Tables",
        telephone: "Telephone"
    },
    restaurantAvecStatistiques: {
        nombrePlats: "NombrePlats",
        nombreTables: "NombreTables",
        noteMoyenne: "NoteMoyenne"
    },
    statistiquesRestaurant: {
        chiffreAffaires: "ChiffreAffaires",
        nombreClients: "NombreClients",
        nombreCommandes: "NombreCommandes",
        noteMoyenne: "NoteMoyenne",
        restaurantId: "RestaurantId"
    },
    statutCommande: {
        code: "Code",
        libelle: "Libelle",
        values: {
            EnAttente: "En attente",
            EnPreparation: "En préparation",
            Prete: "Prête",
            Servie: "Servie",
            Annulee: "Annulée"
        }
    },
    tableRestaurant: {
        capacite: "Capacite",
        disponible: "Disponible",
        id: "Id",
        numero: "Numero",
        restaurantId: "RestaurantId"
    }
};

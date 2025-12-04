////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {ClientItem} from "../../model/restaurant/client-item";
import {ClientRead} from "../../model/restaurant/client-read";
import {ClientWrite} from "../../model/restaurant/client-write";
import {CommandeItem} from "../../model/restaurant/commande-item";
import {CommandeRead} from "../../model/restaurant/commande-read";
import {CommandeWrite} from "../../model/restaurant/commande-write";
import {LigneCommandeItem} from "../../model/restaurant/ligne-commande-item";
import {LigneCommandeRead} from "../../model/restaurant/ligne-commande-read";
import {LigneCommandeWrite} from "../../model/restaurant/ligne-commande-write";
import {PlatItem} from "../../model/restaurant/plat-item";
import {PlatRead} from "../../model/restaurant/plat-read";
import {PlatWrite} from "../../model/restaurant/plat-write";
import {CategoriePlat, CategoriePlatCode, StatutCommande, StatutCommandeCode} from "../../model/restaurant/references";
import {RestaurantItem} from "../../model/restaurant/restaurant-item";
import {RestaurantRead} from "../../model/restaurant/restaurant-read";
import {RestaurantWrite} from "../../model/restaurant/restaurant-write";
import {TableClientItem} from "../../model/restaurant/table-client-item";
import {TableClientRead} from "../../model/restaurant/table-client-read";
import {TableClientWrite} from "../../model/restaurant/table-client-write";

/**
 * Ajoute un client
 * @param client Client à créer
 * @param options Options pour 'fetch'.
 * @returns Client créé
 */
export async function addClient(client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients`, {
        ...options,
        method: "POST",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Crée une nouvelle commande
 * @param commande Commande à créer
 * @param options Options pour 'fetch'.
 * @returns Commande créée
 */
export async function addCommande(commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes`, {
        ...options,
        method: "POST",
        body: JSON.stringify(commande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Ajoute une ligne de commande
 * @param ligneCommande Ligne de commande à créer
 * @param options Options pour 'fetch'.
 * @returns Ligne de commande créée
 */
export async function addLigneCommande(ligneCommande: LigneCommandeWrite, options: RequestInit = {}): Promise<LigneCommandeRead> {
    const response = await fetch(`./api/restaurants/ligne-commandes`, {
        ...options,
        method: "POST",
        body: JSON.stringify(ligneCommande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Ajoute un plat
 * @param plat Plat à créer
 * @param options Options pour 'fetch'.
 * @returns Plat créé
 */
export async function addPlat(plat: PlatWrite, options: RequestInit = {}): Promise<PlatRead> {
    const response = await fetch(`./api/restaurants/plats`, {
        ...options,
        method: "POST",
        body: JSON.stringify(plat),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Ajoute un restaurant
 * @param restaurant Restaurant à créer
 * @param options Options pour 'fetch'.
 * @returns Restaurant créé
 */
export async function addRestaurant(restaurant: RestaurantWrite, options: RequestInit = {}): Promise<RestaurantRead> {
    const response = await fetch(`./api/restaurants`, {
        ...options,
        method: "POST",
        body: JSON.stringify(restaurant),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Ajoute une table
 * @param table Table à créer
 * @param options Options pour 'fetch'.
 * @returns Table créée
 */
export async function addTable(table: TableClientWrite, options: RequestInit = {}): Promise<TableClientRead> {
    const response = await fetch(`./api/restaurants/tables`, {
        ...options,
        method: "POST",
        body: JSON.stringify(table),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Supprime un client
 * @param cliId Identifiant du client
 * @param options Options pour 'fetch'.
 */
export async function deleteClient(cliId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/clients/${cliId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime une commande
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 */
export async function deleteCommande(comId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime une ligne de commande
 * @param ligId Identifiant de la ligne
 * @param options Options pour 'fetch'.
 */
export async function deleteLigneCommande(ligId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/ligne-commandes/${ligId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime un plat
 * @param plaId Identifiant du plat
 * @param options Options pour 'fetch'.
 */
export async function deletePlat(plaId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/plats/${plaId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime un restaurant
 * @param resId Identifiant du restaurant
 * @param options Options pour 'fetch'.
 */
export async function deleteRestaurant(resId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/${resId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime une table
 * @param tabId Identifiant de la table
 * @param options Options pour 'fetch'.
 */
export async function deleteTable(tabId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/tables/${tabId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Liste toutes les catégories de plats
 * @param options Options pour 'fetch'.
 * @returns Liste des catégories de plats
 */
export async function getCategoriePlats(options: RequestInit = {}): Promise<CategoriePlat[]> {
    const response = await fetch(`./api/restaurants/categorie-plats`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'un client
 * @param cliId Identifiant du client
 * @param options Options pour 'fetch'.
 * @returns Détail du client
 */
export async function getClient(cliId: number, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${cliId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les commandes d'un client
 * @param cliId Identifiant du client
 * @param options Options pour 'fetch'.
 * @returns Liste des commandes du client
 */
export async function getClientCommandes(cliId: number, options: RequestInit = {}): Promise<CommandeItem[]> {
    const response = await fetch(`./api/restaurants/clients/${cliId}/commandes`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les clients
 * @param nom Nom du client
 * @param email Adresse email du client
 * @param options Options pour 'fetch'.
 * @returns Liste des clients
 */
export async function getClients(nom?: string, email?: string, options: RequestInit = {}): Promise<ClientItem[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (email !== undefined) {
        query.append("email", email)
    }
    const response = await fetch(`./api/restaurants/clients?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'une commande
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 * @returns Détail de la commande
 */
export async function getCommande(comId: number, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les lignes d'une commande
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 * @returns Liste des lignes de la commande
 */
export async function getCommandeLignes(comId: number, options: RequestInit = {}): Promise<LigneCommandeItem[]> {
    const response = await fetch(`./api/restaurants/commandes/${comId}/lignes`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste toutes les commandes
 * @param clientId Client ayant passé la commande
 * @param statutCommandeCode Statut de la commande
 * @param tableClientId Table associée à la commande
 * @param options Options pour 'fetch'.
 * @returns Liste des commandes
 */
export async function getCommandes(clientId?: number, statutCommandeCode: StatutCommandeCode = "EN_ATT", tableClientId?: number, options: RequestInit = {}): Promise<CommandeItem[]> {
    const query = new URLSearchParams();
    if (clientId !== undefined) {
        query.append("clientId", `${clientId}`)
    }
    if (statutCommandeCode !== undefined) {
        query.append("statutCommandeCode", statutCommandeCode)
    }
    if (tableClientId !== undefined) {
        query.append("tableClientId", `${tableClientId}`)
    }
    const response = await fetch(`./api/restaurants/commandes?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère les commandes par date
 * @param dateCommande Date et heure de la commande
 * @param options Options pour 'fetch'.
 * @returns Commandes pour la date spécifiée
 */
export async function getCommandesByDate(dateCommande?: string, options: RequestInit = {}): Promise<CommandeItem[]> {
    const query = new URLSearchParams();
    if (dateCommande !== undefined) {
        query.append("dateCommande", dateCommande)
    }
    const response = await fetch(`./api/restaurants/commandes/by-date?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'une ligne de commande
 * @param ligId Identifiant de la ligne
 * @param options Options pour 'fetch'.
 * @returns Détail de la ligne de commande
 */
export async function getLigneCommande(ligId: number, options: RequestInit = {}): Promise<LigneCommandeRead> {
    const response = await fetch(`./api/restaurants/ligne-commandes/${ligId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste toutes les lignes de commande
 * @param commandeId Commande à laquelle appartient la ligne
 * @param platId Plat commandé
 * @param options Options pour 'fetch'.
 * @returns Liste des lignes de commande
 */
export async function getLigneCommandes(commandeId?: number, platId?: number, options: RequestInit = {}): Promise<LigneCommandeItem[]> {
    const query = new URLSearchParams();
    if (commandeId !== undefined) {
        query.append("commandeId", `${commandeId}`)
    }
    if (platId !== undefined) {
        query.append("platId", `${platId}`)
    }
    const response = await fetch(`./api/restaurants/ligne-commandes?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'un plat
 * @param plaId Identifiant du plat
 * @param options Options pour 'fetch'.
 * @returns Détail du plat
 */
export async function getPlat(plaId: number, options: RequestInit = {}): Promise<PlatRead> {
    const response = await fetch(`./api/restaurants/plats/${plaId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les plats
 * @param disponible Indique si le plat est disponible
 * @param restaurantIdRestaurant Restaurant proposant ce plat
 * @param categoriePlatCodeCategoriePlat Catégorie du plat
 * @param options Options pour 'fetch'.
 * @returns Liste des plats
 */
export async function getPlats(disponible: boolean = true, restaurantIdRestaurant?: number, categoriePlatCodeCategoriePlat?: CategoriePlatCode, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    if (restaurantIdRestaurant !== undefined) {
        query.append("restaurantIdRestaurant", `${restaurantIdRestaurant}`)
    }
    if (categoriePlatCodeCategoriePlat !== undefined) {
        query.append("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat)
    }
    const response = await fetch(`./api/restaurants/plats?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'un restaurant
 * @param resId Identifiant du restaurant
 * @param options Options pour 'fetch'.
 * @returns Détail du restaurant
 */
export async function getRestaurant(resId: number, options: RequestInit = {}): Promise<RestaurantRead> {
    const response = await fetch(`./api/restaurants/${resId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les plats d'un restaurant
 * @param resId Identifiant du restaurant
 * @param disponible Indique si le plat est disponible
 * @param categoriePlatCodeCategoriePlat Catégorie du plat
 * @param options Options pour 'fetch'.
 * @returns Liste des plats du restaurant
 */
export async function getRestaurantPlats(resId: number, disponible: boolean = true, categoriePlatCodeCategoriePlat?: CategoriePlatCode, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    if (categoriePlatCodeCategoriePlat !== undefined) {
        query.append("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat)
    }
    const response = await fetch(`./api/restaurants/${resId}/plats?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les tables d'un restaurant
 * @param resId Identifiant du restaurant
 * @param disponible Indique si la table est disponible
 * @param options Options pour 'fetch'.
 * @returns Liste des tables du restaurant
 */
export async function getRestaurantTables(resId: number, disponible: boolean = true, options: RequestInit = {}): Promise<TableClientItem[]> {
    const query = new URLSearchParams();
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    const response = await fetch(`./api/restaurants/${resId}/tables?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les restaurants
 * @param options Options pour 'fetch'.
 * @returns Liste des restaurants
 */
export async function getRestaurants(options: RequestInit = {}): Promise<RestaurantItem[]> {
    const response = await fetch(`./api/restaurants`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les statuts de commande
 * @param options Options pour 'fetch'.
 * @returns Liste des statuts de commande
 */
export async function getStatutCommandes(options: RequestInit = {}): Promise<StatutCommande[]> {
    const response = await fetch(`./api/restaurants/statuts-commande`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Charge le détail d'une table
 * @param tabId Identifiant de la table
 * @param options Options pour 'fetch'.
 * @returns Détail de la table
 */
export async function getTable(tabId: number, options: RequestInit = {}): Promise<TableClientRead> {
    const response = await fetch(`./api/restaurants/tables/${tabId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste toutes les tables
 * @param restaurantIdRestaurant Restaurant auquel appartient la table
 * @param disponible Indique si la table est disponible
 * @param options Options pour 'fetch'.
 * @returns Liste des tables
 */
export async function getTables(restaurantIdRestaurant?: number, disponible: boolean = true, options: RequestInit = {}): Promise<TableClientItem[]> {
    const query = new URLSearchParams();
    if (restaurantIdRestaurant !== undefined) {
        query.append("restaurantIdRestaurant", `${restaurantIdRestaurant}`)
    }
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    const response = await fetch(`./api/restaurants/tables?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Met à jour partiellement un client
 * @param cliId Identifiant du client
 * @param client Données partielles du client
 * @param options Options pour 'fetch'.
 * @returns Client mis à jour
 */
export async function patchClient(cliId: number, client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${cliId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour partiellement une commande
 * @param comId Identifiant de la commande
 * @param commande Données partielles de la commande
 * @param options Options pour 'fetch'.
 * @returns Commande mise à jour
 */
export async function patchCommande(comId: number, commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(commande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour partiellement un plat
 * @param plaId Identifiant du plat
 * @param plat Données partielles du plat
 * @param options Options pour 'fetch'.
 * @returns Plat mis à jour
 */
export async function patchPlat(plaId: number, plat: PlatWrite, options: RequestInit = {}): Promise<PlatRead> {
    const response = await fetch(`./api/restaurants/plats/${plaId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(plat),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Recherche de plats avec critères multiples
 * @param nom Nom du plat
 * @param restaurantIdRestaurant Restaurant proposant ce plat
 * @param categoriePlatCodeCategoriePlat Catégorie du plat
 * @param disponible Indique si le plat est disponible
 * @param options Options pour 'fetch'.
 * @returns Plats correspondant aux critères de recherche
 */
export async function searchPlats(nom?: string, restaurantIdRestaurant?: number, categoriePlatCodeCategoriePlat?: CategoriePlatCode, disponible: boolean = true, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (restaurantIdRestaurant !== undefined) {
        query.append("restaurantIdRestaurant", `${restaurantIdRestaurant}`)
    }
    if (categoriePlatCodeCategoriePlat !== undefined) {
        query.append("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat)
    }
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    const response = await fetch(`./api/restaurants/plats/search?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Met à jour un client
 * @param cliId Identifiant du client
 * @param client Client à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Client mis à jour
 */
export async function updateClient(cliId: number, client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${cliId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour une commande
 * @param comId Identifiant de la commande
 * @param commande Commande à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Commande mise à jour
 */
export async function updateCommande(comId: number, commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(commande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour uniquement le statut d'une commande
 * @param comId Identifiant de la commande
 * @param statutCommandeCode Statut de la commande
 * @param options Options pour 'fetch'.
 * @returns Commande avec le statut mis à jour
 */
export async function updateCommandeStatut(comId: number, statutCommandeCode: StatutCommandeCode = "EN_ATT", options: RequestInit = {}): Promise<CommandeRead> {
    const query = new URLSearchParams();
    if (statutCommandeCode !== undefined) {
        query.append("statutCommandeCode", statutCommandeCode)
    }
    const response = await fetch(`./api/restaurants/commandes/${comId}/statut?${query}`, {
        ...options,
        method: "PATCH"
    });
    return await response.json();
}

/**
 * Met à jour une ligne de commande
 * @param ligId Identifiant de la ligne
 * @param ligneCommande Ligne de commande à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Ligne de commande mise à jour
 */
export async function updateLigneCommande(ligId: number, ligneCommande: LigneCommandeWrite, options: RequestInit = {}): Promise<LigneCommandeRead> {
    const response = await fetch(`./api/restaurants/ligne-commandes/${ligId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(ligneCommande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour un plat
 * @param plaId Identifiant du plat
 * @param plat Plat à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Plat mis à jour
 */
export async function updatePlat(plaId: number, plat: PlatWrite, options: RequestInit = {}): Promise<PlatRead> {
    const response = await fetch(`./api/restaurants/plats/${plaId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(plat),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour un restaurant
 * @param resId Identifiant du restaurant
 * @param restaurant Restaurant à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Restaurant mis à jour
 */
export async function updateRestaurant(resId: number, restaurant: RestaurantWrite, options: RequestInit = {}): Promise<RestaurantRead> {
    const response = await fetch(`./api/restaurants/${resId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(restaurant),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour une table
 * @param tabId Identifiant de la table
 * @param table Table à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Table mise à jour
 */
export async function updateTable(tabId: number, table: TableClientWrite, options: RequestInit = {}): Promise<TableClientRead> {
    const response = await fetch(`./api/restaurants/tables/${tabId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(table),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

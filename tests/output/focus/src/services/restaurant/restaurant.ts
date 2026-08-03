////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {CategoriePlatCode} from "../../model/restaurant/enums";
import {MenuRead} from "../../model/restaurant/menu-read";
import {PlatItem} from "../../model/restaurant/plat-item";
import {RestaurantAvecStatistiques} from "../../model/restaurant/restaurant-avec-statistiques";
import {RestaurantItem} from "../../model/restaurant/restaurant-item";
import {RestaurantRead} from "../../model/restaurant/restaurant-read";
import {RestaurantWrite} from "../../model/restaurant/restaurant-write";
import {StatistiquesRestaurant} from "../../model/restaurant/statistiques-restaurant";
import {TableItem} from "../../model/restaurant/table-item";
import {TableRead} from "../../model/restaurant/table-read";
import {TableWrite} from "../../model/restaurant/table-write";

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
export async function addTable(table: TableWrite, options: RequestInit = {}): Promise<TableRead> {
    const response = await fetch(`./api/restaurants/tables`, {
        ...options,
        method: "POST",
        body: JSON.stringify(table),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
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
 * Récupère un menu spécifique d'un restaurant
 * @param resId Identifiant du restaurant
 * @param menId Identifiant du menu
 * @param options Options pour 'fetch'.
 * @returns Menu du restaurant
 */
export async function getRestaurantMenu(resId: number, menId: number, options: RequestInit = {}): Promise<MenuRead> {
    const response = await fetch(`./api/restaurants/${resId}/menus/${menId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les plats d'un restaurant
 * @param resId Identifiant du restaurant
 * @param categoriePlatCode Catégorie du plat
 * @param disponible Indique si le plat est disponible
 * @param options Options pour 'fetch'.
 * @returns Liste des plats du restaurant
 */
export async function getRestaurantPlats(resId: number, categoriePlatCode: CategoriePlatCode, disponible: boolean = true, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (categoriePlatCode !== undefined) {
        query.append("categoriePlatCode", categoriePlatCode)
    }
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    const response = await fetch(`./api/restaurants/${resId}/plats?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère les statistiques d'un restaurant
 * @param resId Identifiant du restaurant
 * @param dateDebut Date de début pour le calcul des statistiques
 * @param dateFin Date de fin pour le calcul des statistiques
 * @param options Options pour 'fetch'.
 * @returns Statistiques du restaurant
 */
export async function getRestaurantStatistiques(resId: number, dateDebut: string, dateFin: string, options: RequestInit = {}): Promise<StatistiquesRestaurant> {
    const query = new URLSearchParams();
    if (dateDebut !== undefined) {
        query.append("dateDebut", dateDebut)
    }
    if (dateFin !== undefined) {
        query.append("dateFin", dateFin)
    }
    const response = await fetch(`./api/restaurants/${resId}/statistiques?${query}`, {
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
export async function getRestaurantTables(resId: number, disponible: boolean = true, options: RequestInit = {}): Promise<TableItem[]> {
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
 * Charge le détail d'une table
 * @param tabId Identifiant de la table
 * @param options Options pour 'fetch'.
 * @returns Détail de la table
 */
export async function getTable(tabId: number, options: RequestInit = {}): Promise<TableRead> {
    const response = await fetch(`./api/restaurants/tables/${tabId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste toutes les tables
 * @param restaurantId Restaurant auquel appartient la table
 * @param disponible Indique si la table est disponible
 * @param options Options pour 'fetch'.
 * @returns Liste des tables
 */
export async function getTables(restaurantId: number, disponible: boolean = true, options: RequestInit = {}): Promise<TableItem[]> {
    const query = new URLSearchParams();
    if (restaurantId !== undefined) {
        query.append("restaurantId", `${restaurantId}`)
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
 * Recherche avancée de restaurants
 * @param nom Nom du restaurant (recherche partielle)
 * @param adresse Adresse du restaurant (recherche partielle)
 * @param noteMin Note minimum requise
 * @param options Options pour 'fetch'.
 * @returns Liste des restaurants correspondant aux critères
 */
export async function searchRestaurants(nom: string, adresse?: string, noteMin?: number, options: RequestInit = {}): Promise<RestaurantAvecStatistiques[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (adresse !== undefined) {
        query.append("adresse", adresse)
    }
    if (noteMin !== undefined) {
        query.append("noteMin", `${noteMin}`)
    }
    const response = await fetch(`./api/restaurants/search?${query}`, {
        ...options,
        method: "GET"
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
export async function updateTable(tabId: number, table: TableWrite, options: RequestInit = {}): Promise<TableRead> {
    const response = await fetch(`./api/restaurants/tables/${tabId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(table),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

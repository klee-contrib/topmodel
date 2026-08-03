////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {CategoriePlat, CategoriePlatCode, RegionCode} from "../../model/restaurant/enums";
import {MenuRead} from "../../model/restaurant/menu-read";
import {MenuWrite} from "../../model/restaurant/menu-write";
import {PlatItem} from "../../model/restaurant/plat-item";
import {PlatRead} from "../../model/restaurant/plat-read";
import {PlatWrite} from "../../model/restaurant/plat-write";
import {PromotionRead} from "../../model/restaurant/promotion-read";
import {PromotionWrite} from "../../model/restaurant/promotion-write";

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
 * Crée un menu avec ses plats
 * @param menu Menu à créer
 * @param regCodeOrigine Code de la région.
 * @param options Options pour 'fetch'.
 * @returns Menu créé avec ses plats
 */
export async function createMenu(menu: MenuWrite, regCodeOrigine: RegionCode, options: RequestInit = {}): Promise<MenuRead> {
    const body = new FormData();
    fillFormData(
        {
            ...menu,
            regCodeOrigine
        },
        body
    );
    const response = await fetch(`./api/restaurants/menus`, {
        ...options,
        method: "POST",
        body
    });
    return await response.json();
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
 * @param restaurantId Restaurant proposant ce plat
 * @param categoriePlatCode Catégorie du plat
 * @param disponible Indique si le plat est disponible
 * @param options Options pour 'fetch'.
 * @returns Liste des plats
 */
export async function getPlats(restaurantId: number, categoriePlatCode: CategoriePlatCode, disponible: boolean = true, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (restaurantId !== undefined) {
        query.append("restaurantId", `${restaurantId}`)
    }
    if (categoriePlatCode !== undefined) {
        query.append("categoriePlatCode", categoriePlatCode)
    }
    if (disponible !== undefined) {
        query.append("disponible", `${disponible}`)
    }
    const response = await fetch(`./api/restaurants/plats?${query}`, {
        ...options,
        method: "GET"
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
 * Met à jour partiellement une promotion
 * @param plaId Identifiant du plat
 * @param promotion Données partielles de la promotion
 * @param options Options pour 'fetch'.
 * @returns Promotion mise à jour
 */
export async function patchPromotion(plaId: number, promotion: PromotionWrite, options: RequestInit = {}): Promise<PromotionRead> {
    const response = await fetch(`./api/restaurants/plats/${plaId}/promotion`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(promotion),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Recherche de plats avec critères multiples
 * @param nom Nom du plat
 * @param restaurantId Restaurant proposant ce plat
 * @param categoriePlatCode Catégorie du plat
 * @param disponible Indique si le plat est disponible
 * @param options Options pour 'fetch'.
 * @returns Plats correspondant aux critères de recherche
 */
export async function searchPlats(nom: string, restaurantId: number, categoriePlatCode: CategoriePlatCode, disponible: boolean = true, options: RequestInit = {}): Promise<PlatItem[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (restaurantId !== undefined) {
        query.append("restaurantId", `${restaurantId}`)
    }
    if (categoriePlatCode !== undefined) {
        query.append("categoriePlatCode", categoriePlatCode)
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

function fillFormData(data: any, formData: FormData, prefix = "") {
    if (Array.isArray(data)) {
        for (const [i, item] of data.entries()) {
            fillFormData(item, formData, prefix + (typeof item === "object" && !(item instanceof File) ? `[${i}]` : ""));
        }
    } else if (typeof data === "object" && !(data instanceof File)) {
        for (const key in data) {
            fillFormData(data[key], formData, (prefix ? `${prefix}.` : "") + key);
        }
    } else {
        formData.append(prefix, data);
    }
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {ProfilItem} from "../../../model/securite/profil/profil-item";
import {ProfilRead} from "../../../model/securite/profil/profil-read";
import {ProfilWrite} from "../../../model/securite/profil/profil-write";

/**
 * Ajoute un Profil
 * @param profil Profil à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Profil sauvegardé
 */
export async function addProfil(profil: ProfilWrite, options: RequestInit = {}): Promise<ProfilRead> {
    const response = await fetch(`./api/profils`, {
        ...options,
        method: "POST",
        body: JSON.stringify(profil),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Charge le détail d'un Profil
 * @param proId Id technique
 * @param options Options pour 'fetch'.
 * @returns Le détail du profil
 */
export async function getProfil(proId: number, options: RequestInit = {}): Promise<ProfilRead> {
    const response = await fetch(`./api/profils/${proId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les Profils
 * @param options Options pour 'fetch'.
 * @returns Profils matchant les critères
 */
export async function getProfils(options: RequestInit = {}): Promise<ProfilItem[]> {
    const response = await fetch(`./api/profils`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Sauvegarde un Profil
 * @param proId Id technique
 * @param profil Profil à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Profil sauvegardé
 */
export async function updateProfil(proId: number, profil: ProfilWrite, options: RequestInit = {}): Promise<ProfilRead> {
    const response = await fetch(`./api/profils/${proId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(profil),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

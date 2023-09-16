////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {coreFetch} from "@focus4/core";

import {ProfilItem} from "../../../model/securite/profil/profil-item";
import {ProfilRead} from "../../../model/securite/profil/profil-read";
import {ProfilWrite} from "../../../model/securite/profil/profil-write";

/**
 * Ajoute un Profil
 * @param profil Profil à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Profil sauvegardé
 */
export function addProfil(profil: ProfilWrite, options: RequestInit = {}): Promise<ProfilRead> {
    return coreFetch("POST", `./api/profils`, {body: profil}, options);
}

/**
 * Charge le détail d'un Profil
 * @param proId Id technique
 * @param options Options pour 'fetch'.
 * @returns Le détail de l'Profil
 */
export function getProfil(proId: number, options: RequestInit = {}): Promise<ProfilRead> {
    return coreFetch("GET", `./api/profils/${proId}`, {}, options);
}

/**
 * Liste tous les Profils
 * @param options Options pour 'fetch'.
 * @returns Profils matchant les critères
 */
export function getProfils(options: RequestInit = {}): Promise<ProfilItem[]> {
    return coreFetch("GET", `./api/profils`, {}, options);
}

/**
 * Sauvegarde un Profil
 * @param proId Id technique
 * @param profil Profil à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Profil sauvegardé
 */
export function updateProfil(proId: number, profil: ProfilWrite, options: RequestInit = {}): Promise<ProfilRead> {
    return coreFetch("PUT", `./api/profils/${proId}`, {body: profil}, options);
}

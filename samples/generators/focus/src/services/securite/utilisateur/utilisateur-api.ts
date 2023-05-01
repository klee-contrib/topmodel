////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {coreFetch} from "@focus4/core";

import {Page} from "@/types";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";
import {UtilisateurSearch} from "../../../model/utilisateur/utilisateur-search";

/**
 * Recherche des utilisateurs
 * @param utiId Id technique
 * @param options Options pour 'fetch'.
 */
export function deleteAll(utiId?: number[], options: RequestInit = {}): Promise<void> {
    return coreFetch("DELETE", `./utilisateur/deleteAll`, {query: {utiId}}, options);
}

/**
 * Charge le détail d'un utilisateur
 * @param utiId Id technique
 * @param options Options pour 'fetch'.
 * @returns Le détail de l'utilisateur
 */
export function find(utiId: number, options: RequestInit = {}): Promise<UtilisateurDto> {
    return coreFetch("GET", `./utilisateur/${utiId}`, {}, options);
}

/**
 * Charge une liste d'utilisateurs par leur type
 * @param typeUtilisateurCode Type d'utilisateur en Many to one
 * @param options Options pour 'fetch'.
 * @returns Liste des utilisateurs
 */
export function findAllByType(typeUtilisateurCode: TypeUtilisateurCode = "ADM", options: RequestInit = {}): Promise<UtilisateurSearch[]> {
    return coreFetch("GET", `./utilisateur/list`, {query: {typeUtilisateurCode}}, options);
}

/**
 * Sauvegarde un utilisateur
 * @param utilisateur Utilisateur à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Utilisateur sauvegardé
 */
export function save(utilisateur: UtilisateurDto, options: RequestInit = {}): Promise<UtilisateurDto> {
    return coreFetch("POST", `./utilisateur/save`, {body: utilisateur}, options);
}

/**
 * Recherche des utilisateurs
 * @param utiId Id technique
 * @param age Age en années de l'utilisateur
 * @param profilId Profil de l'utilisateur
 * @param email Email de l'utilisateur
 * @param nom Nom de l'utilisateur
 * @param actif Si l'utilisateur est actif
 * @param typeUtilisateurCode Type d'utilisateur en Many to one
 * @param utilisateursEnfant Utilisateur enfants
 * @param dateCreation Date de création de l'utilisateur
 * @param dateModification Date de modification de l'utilisateur
 * @param options Options pour 'fetch'.
 * @returns Utilisateurs matchant les critères
 */
export function search(utiId?: number, age: number = 6l, profilId?: number, email?: string, nom: string = "Jabx", actif?: bool, typeUtilisateurCode: TypeUtilisateurCode = "ADM", utilisateursEnfant?: number[], dateCreation?: string, dateModification?: string, options: RequestInit = {}): Promise<Page<UtilisateurSearch>> {
    return coreFetch("POST", `./utilisateur/search`, {query: {utiId, age, profilId, email, nom, actif, typeUtilisateurCode, utilisateursEnfant, dateCreation, dateModification}}, options);
}

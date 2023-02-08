////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {coreFetch} from "@focus4/core";

import {Page} from "@/types";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";

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
export function findAllByType(typeUtilisateurCode: TypeUtilisateurCode = "ADM", options: RequestInit = {}): Promise<UtilisateurDto[]> {
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
 * @param typeUtilisateurCode Type d'utilisateur en Many to one
 * @param dateCreation Date de création de l'utilisateur
 * @param dateModification Date de modification de l'utilisateur
 * @param options Options pour 'fetch'.
 * @returns Utilisateurs matchant les critères
 */
export function search(utiId?: number, age: number = 6l, profilId?: number, email?: string, nom: string = "Jabx", typeUtilisateurCode: TypeUtilisateurCode = "ADM", dateCreation?: string, dateModification?: string, options: RequestInit = {}): Promise<Page<UtilisateurDto>> {
    return coreFetch("POST", `./utilisateur/search`, {query: {utiId, age, profilId, email, nom, typeUtilisateurCode, dateCreation, dateModification}}, options);
}

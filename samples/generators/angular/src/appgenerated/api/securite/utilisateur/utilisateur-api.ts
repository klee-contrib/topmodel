////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import {Page} from "@/types";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";
import {UtilisateurSearch} from "../../../model/utilisateur/utilisateur-search";
@Injectable({
    providedIn: 'root'
})
export class UtilisateurApiService {

    constructor(private http: HttpClient) {}

    /**
     * Recherche des utilisateurs
     * @param utiId Id technique
     * @param options Options pour 'fetch'.
     */
    deleteAll(utiId?: number[], queryParams: any = {}): Observable<void> {
        if(utiId) {
            queryParams['utiId'] = utiId
        }

        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }

        return this.http.delete<void>(`/utilisateur/deleteAll`, httpOptions);
    }

    /**
     * Charge le détail d'un utilisateur
     * @param utiId Id technique
     * @param options Options pour 'fetch'.
     * @returns Le détail de l'utilisateur
     */
    find(utiId: number): Observable<UtilisateurDto> {
        return this.http.get<UtilisateurDto>(`/utilisateur/${utiId}`);
    }

    /**
     * Charge une liste d'utilisateurs par leur type
     * @param typeUtilisateurCode Type d'utilisateur en Many to one
     * @param options Options pour 'fetch'.
     * @returns Liste des utilisateurs
     */
    findAllByType(typeUtilisateurCode: TypeUtilisateurCode = "ADM", queryParams: any = {}): Observable<UtilisateurSearch[]> {
        if(typeUtilisateurCode) {
            queryParams['typeUtilisateurCode'] = typeUtilisateurCode
        }

        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }

        return this.http.get<UtilisateurSearch[]>(`/utilisateur/list`, httpOptions);
    }

    /**
     * Sauvegarde un utilisateur
     * @param utilisateur Utilisateur à sauvegarder
     * @param options Options pour 'fetch'.
     * @returns Utilisateur sauvegardé
     */
    save(utilisateur: UtilisateurDto): Observable<UtilisateurDto> {
        return this.http.post<UtilisateurDto>(`/utilisateur/save`, utilisateur);
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
    search(utiId?: number, age?: number, profilId?: number, email?: string, nom?: string, actif?: boolean, typeUtilisateurCode?: TypeUtilisateurCode, utilisateursEnfant?: number[], dateCreation?: string, dateModification?: string, queryParams: any = {}): Observable<Page<UtilisateurSearch>> {
        if(utiId) {
            queryParams['utiId'] = utiId
        }

        if(age) {
            queryParams['age'] = age
        }

        if(profilId) {
            queryParams['profilId'] = profilId
        }

        if(email) {
            queryParams['email'] = email
        }

        if(nom) {
            queryParams['nom'] = nom
        }

        if(actif) {
            queryParams['actif'] = actif
        }

        if(typeUtilisateurCode) {
            queryParams['typeUtilisateurCode'] = typeUtilisateurCode
        }

        if(utilisateursEnfant) {
            queryParams['utilisateursEnfant'] = utilisateursEnfant
        }

        if(dateCreation) {
            queryParams['dateCreation'] = dateCreation
        }

        if(dateModification) {
            queryParams['dateModification'] = dateModification
        }

        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }

        return this.http.post<Page<UtilisateurSearch>>(`/utilisateur/search`, httpOptions);
    }
}

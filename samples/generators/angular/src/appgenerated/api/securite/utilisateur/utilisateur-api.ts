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
        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }
        if(utiId !== null) {
            httpOptions.params.set('utiId', utiId)
        }

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
        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }
        if(typeUtilisateurCode !== null) {
            httpOptions.params.set('typeUtilisateurCode', typeUtilisateurCode)
        }

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
    search(utiId?: number, age: number = 6l, profilId?: number, email?: string, nom: string = "Jabx", actif?: boolean, typeUtilisateurCode: TypeUtilisateurCode = "ADM", utilisateursEnfant?: number[], dateCreation?: string, dateModification?: string, queryParams: any = {}): Observable<Page<UtilisateurSearch>> {
        const httpParams = new HttpParams({fromObject : queryParams});
        const httpOptions = { params: httpParams }
        if(utiId !== null) {
            httpOptions.params.set('utiId', utiId)
        }
        if(age !== null) {
            httpOptions.params.set('age', age)
        }
        if(profilId !== null) {
            httpOptions.params.set('profilId', profilId)
        }
        if(email !== null) {
            httpOptions.params.set('email', email)
        }
        if(nom !== null) {
            httpOptions.params.set('nom', nom)
        }
        if(actif !== null) {
            httpOptions.params.set('actif', actif)
        }
        if(typeUtilisateurCode !== null) {
            httpOptions.params.set('typeUtilisateurCode', typeUtilisateurCode)
        }
        if(utilisateursEnfant !== null) {
            httpOptions.params.set('utilisateursEnfant', utilisateursEnfant)
        }
        if(dateCreation !== null) {
            httpOptions.params.set('dateCreation', dateCreation)
        }
        if(dateModification !== null) {
            httpOptions.params.set('dateModification', dateModification)
        }

        return this.http.post<Page<UtilisateurSearch>>(`/utilisateur/search`, httpOptions);
    }
}

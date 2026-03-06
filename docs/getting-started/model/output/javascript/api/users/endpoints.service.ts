////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UtilisateurCreateDto } from "../../model/users/utilisateur-create-dto";
import { UtilisateurDetailDto } from "../../model/users/utilisateur-detail-dto";
import { UtilisateurUpdateDto } from "../../model/users/utilisateur-update-dto";
@Injectable({
    providedIn: 'root'
})
export class EndpointsService {

    private readonly http = inject(HttpClient);

    /**
     * @description Créé un nouvel Utilisateur
     * @param detail Le détail de l'utilisateur à créer
     * @returns Le détail de l'utilisateur créé
     */
    createUtilisateur(detail: UtilisateurCreateDto, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<UtilisateurDetailDto> {
        return this.http.post<UtilisateurDetailDto>(`/Utilisateur`, detail, {observe: 'body', ...options});
    }

    /**
     * @description Supprime un Utilisateur
     * @param utilisateurId Identifiant unique de l'utilisateur
     */
    deleteUtilisateur(utilisateurId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/Utilisateur/${utilisateurId}`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'un Utilisateur
     * @param utilisateurId Identifiant unique de l'utilisateur
     * @returns Le détail d'un Utilisateur
     */
    getUtilisateur(utilisateurId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<UtilisateurDetailDto> {
        return this.http.get<UtilisateurDetailDto>(`/Utilisateur/${utilisateurId}`, {observe: 'body', ...options});
    }

    /**
     * @description Modifie un Utilisateur
     * @param detail Le détail de l'utilisateur à modifier
     * @param utilisateurId Identifiant unique de l'utilisateur
     * @returns Le détail de l'utilisateur modifié
     */
    updateUtilisateur(detail: UtilisateurUpdateDto, utilisateurId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<UtilisateurDetailDto> {
        return this.http.patch<UtilisateurDetailDto>(`/Utilisateur/${utilisateurId}`, detail, {observe: 'body', ...options});
    }
}

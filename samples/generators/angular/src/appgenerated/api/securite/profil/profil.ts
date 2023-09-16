////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ProfilItem} from "../../../model/securite/profil/profil-item";
import {ProfilRead} from "../../../model/securite/profil/profil-read";
import {ProfilWrite} from "../../../model/securite/profil/profil-write";
@Injectable({
    providedIn: 'root'
})
export class ProfilService {

    constructor(private http: HttpClient) {}

    /**
     * Ajoute un Profil
     * @param profil Profil à sauvegarder
     * @param options Options pour 'fetch'.
     * @returns Profil sauvegardé
     */
    addProfil(profil: ProfilWrite): Observable<ProfilRead> {
        return this.http.post<ProfilRead>(`/api/profils`, profil);
    }

    /**
     * Charge le détail d'un Profil
     * @param proId Id technique
     * @param options Options pour 'fetch'.
     * @returns Le détail de l'Profil
     */
    getProfil(proId: number): Observable<ProfilRead> {
        return this.http.get<ProfilRead>(`/api/profils/${proId}`);
    }

    /**
     * Liste tous les Profils
     * @param options Options pour 'fetch'.
     * @returns Profils matchant les critères
     */
    getProfils(): Observable<ProfilItem[]> {
        return this.http.get<ProfilItem[]>(`/api/profils`);
    }

    /**
     * Sauvegarde un Profil
     * @param proId Id technique
     * @param profil Profil à sauvegarder
     * @param options Options pour 'fetch'.
     * @returns Profil sauvegardé
     */
    updateProfil(proId: number, profil: ProfilWrite): Observable<ProfilRead> {
        return this.http.put<ProfilRead>(`/api/profils/${proId}`, profil);
    }
}

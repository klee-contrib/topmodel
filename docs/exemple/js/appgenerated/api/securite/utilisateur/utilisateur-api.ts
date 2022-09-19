////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import {Page} from "@/types";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";
@Injectable({
	providedIn: 'root'
})
export class UtilisateurApiService {

	constructor(private http: HttpClient) {}

	/**
	 * Charge le détail d'un utilisateur
	 * @param utiId Id technique
	 * @param options Options pour 'fetch'.
	 * @returns Le détail de l'utilisateur
	 */
	getUtilisateur(utiId: number ): Observable<UtilisateurDto> {
		return this.http.get<UtilisateurDto>(`/utilisateur/${utiId}`);
	}

	/**
	 * Charge une liste d'utilisateurs par leur type
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param options Options pour 'fetch'.
	 * @returns Liste des utilisateurs
	 */
	getUtilisateurList(typeUtilisateurCode?: TypeUtilisateurCode , queryParams: any = {}): Observable<UtilisateurDto[]> {
		const httpParams = new HttpParams({fromObject : queryParams});
		const httpOptions = { params: httpParams }
		if(typeUtilisateurCode !== null) {
			httpOptions.params.set('typeUtilisateurCode', typeUtilisateurCode)
		}

		return this.http.get<UtilisateurDto[]>(`/utilisateur/list`, httpOptions);
	}

	/**
	 * Sauvegarde un utilisateur
	 * @param utilisateur Utilisateur à sauvegarder
	 * @param options Options pour 'fetch'.
	 * @returns Utilisateur sauvegardé
	 */
	saveUtilisateur(utilisateur: UtilisateurDto ): Observable<UtilisateurDto> {
		return this.http.post<UtilisateurDto>(`/utilisateur/save`, utilisateur);
	}

	/**
	 * Sauvegarde une liste d'utilisateurs
	 * @param utilisateur Utilisateur à sauvegarder
	 * @param options Options pour 'fetch'.
	 * @returns Utilisateur sauvegardé
	 */
	saveAllUtilisateur(utilisateur: UtilisateurDto[] ): Observable<UtilisateurDto[]> {
		return this.http.post<UtilisateurDto[]>(`/utilisateur/saveAll`, utilisateur);
	}

	/**
	 * Recherche des utilisateurs
	 * @param utiId Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param options Options pour 'fetch'.
	 * @returns Utilisateurs matchant les critères
	 */
	search(utiId?: number age?: number profilId?: number email?: string typeUtilisateurCode?: TypeUtilisateurCode , queryParams: any = {}): Observable<Page<UtilisateurDto>> {
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
		if(typeUtilisateurCode !== null) {
			httpOptions.params.set('typeUtilisateurCode', typeUtilisateurCode)
		}

		return this.http.post<Page<UtilisateurDto>>(`/utilisateur/search`, httpOptions);
	}
}

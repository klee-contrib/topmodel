////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.client.securite.utilisateur;

import java.net.URI;
import java.time.LocalDate;
import java.util.List;

import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurRead;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurWrite;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractUtilisateurClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractUtilisateurClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * UriComponentsBuilder pour la méthode addUtilisateur.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addUtilisateurUriComponentsBuilder() {
		String uri = host + "/api/utilisateurs";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	public ResponseEntity<UtilisateurRead> addUtilisateur(UtilisateurWrite utilisateur){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addUtilisateurUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(utilisateur, headers), UtilisateurRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteUtilisateur.
	 * @param utiId Id de l'utilisateur
	 */
	protected UriComponentsBuilder deleteUtilisateurUriComponentsBuilder(Integer utiId) {
		String uri = host + "/api/utilisateurs/%s".formatted(utiId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un utilisateur.
	 * @param utiId Id de l'utilisateur
	 */
	public ResponseEntity deleteUtilisateur(Integer utiId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteUtilisateurUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode getUtilisateur.
	 * @param utiId Id de l'utilisateur
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getUtilisateurUriComponentsBuilder(Integer utiId) {
		String uri = host + "/api/utilisateurs/%s".formatted(utiId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id de l'utilisateur
	 * @return Le détail de l'utilisateur
	 */
	public ResponseEntity<UtilisateurRead> getUtilisateur(Integer utiId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getUtilisateurUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), UtilisateurRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode searchUtilisateur.
	 * @param nom Nom de l'utilisateur
	 * @param prenom Nom de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param dateNaissance Age de l'utilisateur
	 * @param adresse Adresse de l'utilisateur
	 * @param actif Si l'utilisateur est actif
	 * @param profilId Profil de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchUtilisateurUriComponentsBuilder(String nom, String prenom, String email, LocalDate dateNaissance, String adresse, Boolean actif, Integer profilId, TypeUtilisateurCode typeUtilisateurCode) {
		String uri = host + "/api/utilisateurs";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("nom", nom);
		uriBuilder.queryParam("prenom", prenom);
		uriBuilder.queryParam("email", email);
		if (dateNaissance != null) {
			uriBuilder.queryParam("dateNaissance", dateNaissance);
		}

		if (adresse != null) {
			uriBuilder.queryParam("adresse", adresse);
		}

		uriBuilder.queryParam("actif", actif);
		uriBuilder.queryParam("profilId", profilId);
		uriBuilder.queryParam("typeUtilisateurCode", typeUtilisateurCode);
		return uriBuilder;
	}

	/**
	 * Recherche des utilisateurs.
	 * @param nom Nom de l'utilisateur
	 * @param prenom Nom de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param dateNaissance Age de l'utilisateur
	 * @param adresse Adresse de l'utilisateur
	 * @param actif Si l'utilisateur est actif
	 * @param profilId Profil de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur
	 * @return Utilisateurs matchant les critères
	 */
	public ResponseEntity<List<UtilisateurItem>> searchUtilisateur(String nom, String prenom, String email, LocalDate dateNaissance, String adresse, Boolean actif, Integer profilId, TypeUtilisateurCode typeUtilisateurCode){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchUtilisateurUriComponentsBuilder(nom, prenom, email, dateNaissance, adresse, actif, profilId, typeUtilisateurCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<UtilisateurItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode updateUtilisateur.
	 * @param utiId Id de l'utilisateur
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateUtilisateurUriComponentsBuilder(Integer utiId) {
		String uri = host + "/api/utilisateurs/%s".formatted(utiId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Sauvegarde un utilisateur.
	 * @param utiId Id de l'utilisateur
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	public ResponseEntity<UtilisateurRead> updateUtilisateur(Integer utiId, UtilisateurWrite utilisateur){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateUtilisateurUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(utilisateur, headers), UtilisateurRead.class);
	}
}

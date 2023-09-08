////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.client.securite.utilisateur;

import java.math.BigDecimal;
import java.net.URI;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.core.ParameterizedTypeReference;
import org.springframework.data.domain.Page;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch;
import topmodel.jpa.sample.demo.enums.utilisateur.TypeUtilisateurCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractUtilisateurApiClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractUtilisateurApiClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * UriComponentsBuilder pour la méthode deleteAll.
	 * @param utiId Id technique
	 */
	protected UriComponentsBuilder deleteAllUriComponentsBuilder(List<Integer> utiId) {
		String uri = host + "/utilisateur/deleteAll";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("utiId", utiId);
		return uriBuilder;
	}

	/**
	 * Recherche des utilisateurs.
	 * @param utiId Id technique
	 */
	public ResponseEntity deleteAll(List<Integer> utiId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteAllUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode find.
	 * @param utiId Id technique
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder findUriComponentsBuilder(Integer utiId) {
		String uri = host + "/utilisateur/%s".formatted(utiId);;
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	public ResponseEntity<UtilisateurDto> find(Integer utiId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.findUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), UtilisateurDto.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode findAllByType.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder findAllByTypeUriComponentsBuilder(TypeUtilisateurCode typeUtilisateurCode) {
		String uri = host + "/utilisateur/list";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		if (typeUtilisateurCode != null) {
			uriBuilder.queryParam("typeUtilisateurCode", typeUtilisateurCode);
		}

		return uriBuilder;
	}

	/**
	 * Charge une liste d'utilisateurs par leur type.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Liste des utilisateurs
	 */
	public ResponseEntity<List<UtilisateurSearch>> findAllByType(TypeUtilisateurCode typeUtilisateurCode){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.findAllByTypeUriComponentsBuilder(typeUtilisateurCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<UtilisateurSearch>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode save.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder saveUriComponentsBuilder() {
		String uri = host + "/utilisateur/save";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	public ResponseEntity<UtilisateurDto> save(UtilisateurDto utilisateur){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.saveUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(utilisateur, headers), UtilisateurDto.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode search.
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
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchUriComponentsBuilder(Integer utiId, BigDecimal age, Integer profilId, String email, String nom, Boolean actif, TypeUtilisateurCode typeUtilisateurCode, List<Integer> utilisateursEnfant, LocalDate dateCreation, LocalDateTime dateModification) {
		String uri = host + "/utilisateur/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("utiId", utiId);
		if (age != null) {
			uriBuilder.queryParam("age", age);
		}

		if (profilId != null) {
			uriBuilder.queryParam("profilId", profilId);
		}

		if (email != null) {
			uriBuilder.queryParam("email", email);
		}

		if (nom != null) {
			uriBuilder.queryParam("nom", nom);
		}

		if (actif != null) {
			uriBuilder.queryParam("actif", actif);
		}

		if (typeUtilisateurCode != null) {
			uriBuilder.queryParam("typeUtilisateurCode", typeUtilisateurCode);
		}

		if (utilisateursEnfant != null) {
			uriBuilder.queryParam("utilisateursEnfant", utilisateursEnfant);
		}

		if (dateCreation != null) {
			uriBuilder.queryParam("dateCreation", dateCreation);
		}

		if (dateModification != null) {
			uriBuilder.queryParam("dateModification", dateModification);
		}

		return uriBuilder;
	}

	/**
	 * Recherche des utilisateurs.
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
	 * @return Utilisateurs matchant les critères
	 */
	public ResponseEntity<Page<UtilisateurSearch>> search(Integer utiId, BigDecimal age, Integer profilId, String email, String nom, Boolean actif, TypeUtilisateurCode typeUtilisateurCode, List<Integer> utilisateursEnfant, LocalDate dateCreation, LocalDateTime dateModification){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchUriComponentsBuilder(utiId, age, profilId, email, nom, actif, typeUtilisateurCode, utilisateursEnfant, dateCreation, dateModification);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(headers), new ParameterizedTypeReference<Page<UtilisateurSearch>>() {});
	}
}

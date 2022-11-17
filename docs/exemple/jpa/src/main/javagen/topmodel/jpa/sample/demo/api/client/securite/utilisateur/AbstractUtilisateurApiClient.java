////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.client.securite.utilisateur;

import java.net.URI;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;

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
	 * UriComponentsBuilder pour la méthode find.
	 * @param utilisateurId Id technique
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder findUriComponentsBuilder(Long utilisateurId) {
		String uri = host + "/utilisateur/{utiId}";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("utilisateurId", utilisateurId);
		return uriBuilder;
	}

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utilisateurId Id technique
	 * @return Le détail de l'utilisateur
	 */
	public ResponseEntity<UtilisateurDto> find(Long utilisateurId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.findUriComponentsBuilder(utilisateurId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), UtilisateurDto.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode findAllByType.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder findAllByTypeUriComponentsBuilder(TypeUtilisateur.Values typeUtilisateurCode) {
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
	public ResponseEntity<List> findAllByType(TypeUtilisateur.Values typeUtilisateurCode){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.findAllByTypeUriComponentsBuilder(typeUtilisateurCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), List.class);
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
	 * @param utilisateurId Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchUriComponentsBuilder(Long utilisateurId, Long age, Long profilId, String email, TypeUtilisateur.Values typeUtilisateurCode, LocalDate dateCreation, LocalDateTime dateModification) {
		String uri = host + "/utilisateur/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("utilisateurId", utilisateurId);
		if (age != null) {
			uriBuilder.queryParam("age", age);
		}

		if (profilId != null) {
			uriBuilder.queryParam("profilId", profilId);
		}

		if (email != null) {
			uriBuilder.queryParam("email", email);
		}

		if (typeUtilisateurCode != null) {
			uriBuilder.queryParam("typeUtilisateurCode", typeUtilisateurCode);
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
	 * @param utilisateurId Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @return Utilisateurs matchant les critères
	 */
	public ResponseEntity<Page> search(Long utilisateurId, Long age, Long profilId, String email, TypeUtilisateur.Values typeUtilisateurCode, LocalDate dateCreation, LocalDateTime dateModification){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchUriComponentsBuilder(utilisateurId, age, profilId, email, typeUtilisateurCode, dateCreation, dateModification);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(headers), Page.class);
	}
}

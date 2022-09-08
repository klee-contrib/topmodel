////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.securite.utilisateur;

import java.net.URI;
import java.util.List;
import java.util.stream.Collectors;

import javax.annotation.Generated;
import javax.validation.constraints.Email;

import org.springframework.data.domain.Page;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import topmodel.exemple.name.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.entities.utilisateur.TypeUtilisateur;

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
	 * UriComponentsBuilder pour la méthode GetUtilisateur.
	 * @param utiId Id technique
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getUtilisateurUriComponentsBuilder(long utiId) {
		String uri = host + "utilisateur/%s".formatted(utiId);;
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	public ResponseEntity<UtilisateurDto> getUtilisateur(long utiId, HttpHeaders headers){
		UriComponentsBuilder uri = this.getUtilisateurUriComponentsBuilder(utiId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), UtilisateurDto.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode GetUtilisateurList.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getUtilisateurListUriComponentsBuilder(TypeUtilisateur.Values typeUtilisateurCode) {
		String uri = host + "utilisateur/list";
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
	public ResponseEntity<List> getUtilisateurList(TypeUtilisateur.Values typeUtilisateurCode, HttpHeaders headers){
		UriComponentsBuilder uri = this.getUtilisateurListUriComponentsBuilder(typeUtilisateurCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), List.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode SaveUtilisateur.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder saveUtilisateurUriComponentsBuilder() {
		String uri = host + "utilisateur/save";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	public ResponseEntity<UtilisateurDto> saveUtilisateur(UtilisateurDto utilisateur, HttpHeaders headers){
		UriComponentsBuilder uri = this.saveUtilisateurUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(utilisateur, headers), UtilisateurDto.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode SaveAllUtilisateur.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder saveAllUtilisateurUriComponentsBuilder() {
		String uri = host + "utilisateur/saveAll";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Sauvegarde une liste d'utilisateurs.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	public ResponseEntity<List> saveAllUtilisateur(List<UtilisateurDto> utilisateur, HttpHeaders headers){
		UriComponentsBuilder uri = this.saveAllUtilisateurUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(utilisateur, headers), List.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode Search.
	 * @param utiUtilisateurId Id technique
	 * @param utilisateurAge Age en années de l'utilisateur
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchUriComponentsBuilder(long utiUtilisateurId, Long utilisateurAge, String utilisateuremail, TypeUtilisateur.Values utilisateurTypeUtilisateurCode) {
		String uri = host + "utilisateur/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("utiUtilisateurId", utiUtilisateurId);
		if (utilisateurAge != null) {
			uriBuilder.queryParam("utilisateurAge", utilisateurAge);
		}

		if (utilisateuremail != null) {
			uriBuilder.queryParam("utilisateuremail", utilisateuremail);
		}

		if (utilisateurTypeUtilisateurCode != null) {
			uriBuilder.queryParam("utilisateurTypeUtilisateurCode", utilisateurTypeUtilisateurCode);
		}

		return uriBuilder;
	}

	/**
	 * Recherche des utilisateurs.
	 * @param utiUtilisateurId Id technique
	 * @param utilisateurAge Age en années de l'utilisateur
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Utilisateurs matchant les critères
	 */
	public ResponseEntity<Page> search(long utiUtilisateurId, Long utilisateurAge, String utilisateuremail, TypeUtilisateur.Values utilisateurTypeUtilisateurCode, HttpHeaders headers){
		UriComponentsBuilder uri = this.searchUriComponentsBuilder(utiUtilisateurId, utilisateurAge, utilisateuremail, utilisateurTypeUtilisateurCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(headers), Page.class);
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.client.securite.profil;

import java.net.URI;
import java.util.List;

import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractProfilClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractProfilClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * UriComponentsBuilder pour la méthode addProfil.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addProfilUriComponentsBuilder() {
		String uri = host + "/api/profils";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un Profil.
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	public ResponseEntity<ProfilRead> addProfil(ProfilWrite profil){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addProfilUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(profil, headers), ProfilRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getProfil.
	 * @param proId Id technique
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getProfilUriComponentsBuilder(Integer proId) {
		String uri = host + "/api/profils/%s".formatted(proId);;
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un Profil.
	 * @param proId Id technique
	 * @return Le détail de l'Profil
	 */
	public ResponseEntity<ProfilRead> getProfil(Integer proId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getProfilUriComponentsBuilder(proId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), ProfilRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getProfils.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getProfilsUriComponentsBuilder() {
		String uri = host + "/api/profils";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste tous les Profils.
	 * @return Profils matchant les critères
	 */
	public ResponseEntity<List<ProfilItem>> getProfils(){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getProfilsUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<ProfilItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode updateProfil.
	 * @param proId Id technique
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateProfilUriComponentsBuilder(Integer proId) {
		String uri = host + "/api/profils/%s".formatted(proId);;
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Sauvegarde un Profil.
	 * @param proId Id technique
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	public ResponseEntity<ProfilRead> updateProfil(Integer proId, ProfilWrite profil){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateProfilUriComponentsBuilder(proId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(profil, headers), ProfilRead.class);
	}
}

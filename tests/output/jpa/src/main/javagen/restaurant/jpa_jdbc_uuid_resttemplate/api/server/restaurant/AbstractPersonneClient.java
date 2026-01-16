////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.api.server.restaurant;

import java.net.URI;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.AvisClientRead;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.ClientItem;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.ClientRead;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.ClientWrite;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.CommandeItem;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.EmployeRead;
import restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant.EmployeWrite;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractPersonneClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractPersonneClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * UriComponentsBuilder pour la méthode addClient.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addClientUriComponentsBuilder() {
		String uri = host + "/api/restaurants/clients";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un client.
	 * @param client Client à créer
	 * @return Client créé
	 */
	public ResponseEntity<ClientRead> addClient(ClientWrite client){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addClientUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(client, headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode addEmploye.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addEmployeUriComponentsBuilder() {
		String uri = host + "/api/restaurants/employes";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer
	 * @return Employé créé
	 */
	public ResponseEntity<EmployeRead> addEmploye(EmployeWrite employe){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addEmployeUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(employe, headers), EmployeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteClient.
	 * @param perId Identifiant de la personne
	 */
	protected UriComponentsBuilder deleteClientUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un client.
	 * @param perId Identifiant de la personne
	 */
	public ResponseEntity deleteClient(Integer perId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteClientUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode getAvisClients.
	 * @param resRestaurantId Identifiant du restaurant
	 * @param noteMin Note sur 5
	 * @param approuve Indique si l'avis est approuvé par le restaurant
	 * @param dateDebut Date de l'avis
	 * @param dateFin Date de l'avis
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getAvisClientsUriComponentsBuilder(Integer resRestaurantId, Integer noteMin, Boolean approuve, LocalDateTime dateDebut, LocalDateTime dateFin) {
		String uri = host + "/api/restaurants/avis";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("resRestaurantId", resRestaurantId);
		uriBuilder.queryParam("noteMin", noteMin);
		uriBuilder.queryParam("approuve", approuve);
		uriBuilder.queryParam("dateDebut", dateDebut);
		uriBuilder.queryParam("dateFin", dateFin);
		return uriBuilder;
	}

	/**
	 * Liste les avis clients avec filtres.
	 * @param resRestaurantId Identifiant du restaurant
	 * @param noteMin Note sur 5
	 * @param approuve Indique si l'avis est approuvé par le restaurant
	 * @param dateDebut Date de l'avis
	 * @param dateFin Date de l'avis
	 * @return Liste des avis correspondant aux critères
	 */
	public ResponseEntity<List<AvisClientRead>> getAvisClients(Integer resRestaurantId, Integer noteMin, Boolean approuve, LocalDateTime dateDebut, LocalDateTime dateFin){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getAvisClientsUriComponentsBuilder(resRestaurantId, noteMin, approuve, dateDebut, dateFin);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<AvisClientRead>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getClient.
	 * @param perId Identifiant de la personne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un client.
	 * @param perId Identifiant de la personne
	 * @return Détail du client
	 */
	public ResponseEntity<ClientRead> getClient(Integer perId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getClientAvecCommandes.
	 * @param perId Identifiant de la personne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientAvecCommandesUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s/avec-commandes".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param perId Identifiant de la personne
	 * @return Client avec ses commandes
	 */
	public ResponseEntity<ClientAvecCommandes> getClientAvecCommandes(Integer perId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientAvecCommandesUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), ClientAvecCommandes.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getClientCommandes.
	 * @param perId Identifiant de la personne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientCommandesUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s/commandes".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste les commandes d'un client.
	 * @param perId Identifiant de la personne
	 * @return Liste des commandes du client
	 */
	public ResponseEntity<List<CommandeItem>> getClientCommandes(Integer perId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientCommandesUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getClients.
	 * @param nom Nom de la personne
	 * @param email Adresse email du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientsUriComponentsBuilder(String nom, String email) {
		String uri = host + "/api/restaurants/clients";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("nom", nom);
		if (email != null) {
			uriBuilder.queryParam("email", email);
		}

		return uriBuilder;
	}

	/**
	 * Liste tous les clients.
	 * @param nom Nom de la personne
	 * @param email Adresse email du client
	 * @return Liste des clients
	 */
	public ResponseEntity<List<ClientItem>> getClients(String nom, String email){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientsUriComponentsBuilder(nom, email);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<ClientItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode patchClient.
	 * @param perId Identifiant de la personne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchClientUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement un client.
	 * @param perId Identifiant de la personne
	 * @param client Données partielles du client
	 * @return Client mis à jour
	 */
	public ResponseEntity<ClientRead> patchClient(Integer perId, ClientWrite client){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchClientUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(client, headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updateClient.
	 * @param perId Identifiant de la personne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateClientUriComponentsBuilder(Integer perId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(perId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour un client.
	 * @param perId Identifiant de la personne
	 * @param client Client à mettre à jour
	 * @return Client mis à jour
	 */
	public ResponseEntity<ClientRead> updateClient(Integer perId, ClientWrite client){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateClientUriComponentsBuilder(perId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(client, headers), ClientRead.class);
	}
}

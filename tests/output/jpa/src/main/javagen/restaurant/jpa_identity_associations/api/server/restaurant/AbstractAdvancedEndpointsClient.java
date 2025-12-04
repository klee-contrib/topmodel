////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.api.server.restaurant;

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

import restaurant.jpa_identity_associations.dtos.restaurant.AvisClientRead;
import restaurant.jpa_identity_associations.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_identity_associations.dtos.restaurant.EmployeRead;
import restaurant.jpa_identity_associations.dtos.restaurant.EmployeWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.MenuComplet;
import restaurant.jpa_identity_associations.dtos.restaurant.MenuWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.PromotionRead;
import restaurant.jpa_identity_associations.dtos.restaurant.PromotionWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_identity_associations.dtos.restaurant.ReservationWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_identity_associations.dtos.restaurant.StatistiquesRestaurant;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractAdvancedEndpointsClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractAdvancedEndpointsClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

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
	 * UriComponentsBuilder pour la méthode createMenu.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder createMenuUriComponentsBuilder() {
		String uri = host + "/api/restaurants/menus";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer
	 * @return Menu créé avec ses plats
	 */
	public ResponseEntity<MenuComplet> createMenu(MenuWrite menu){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.createMenuUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(menu, headers), MenuComplet.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode createReservation.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder createReservationUriComponentsBuilder() {
		String uri = host + "/api/restaurants/reservations";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Crée une réservation.
	 * @param reservation Réservation à créer
	 * @return Réservation créée
	 */
	public ResponseEntity<ReservationAvecDetails> createReservation(ReservationWrite reservation){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.createReservationUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(reservation, headers), ReservationAvecDetails.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode exportCommandes.
	 * @param dateDebut Date et heure de la commande
	 * @param dateFin Date et heure de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder exportCommandesUriComponentsBuilder(LocalDateTime dateDebut, LocalDateTime dateFin) {
		String uri = host + "/api/restaurants/commandes/export";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("dateDebut", dateDebut);
		uriBuilder.queryParam("dateFin", dateFin);
		return uriBuilder;
	}

	/**
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande
	 * @param dateFin Date et heure de la commande
	 * @return Fichier CSV des commandes
	 */
	public ResponseEntity<byte[]> exportCommandes(LocalDateTime dateDebut, LocalDateTime dateFin){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.exportCommandesUriComponentsBuilder(dateDebut, dateFin);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), byte[].class);
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
	 * UriComponentsBuilder pour la méthode getClientAvecCommandes.
	 * @param cliId Identifiant du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientAvecCommandesUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s/avec-commandes".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param cliId Identifiant du client
	 * @return Client avec ses commandes
	 */
	public ResponseEntity<ClientAvecCommandes> getClientAvecCommandes(Integer cliId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientAvecCommandesUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), ClientAvecCommandes.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getCommandeDetail.
	 * @param comId Identifiant de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandeDetailUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s/detail".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Récupère le détail complet d'une commande avec ses lignes.
	 * @param comId Identifiant de la commande
	 * @return Détail complet de la commande
	 */
	public ResponseEntity<CommandeDetailRead> getCommandeDetail(Integer comId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandeDetailUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), CommandeDetailRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getRestaurantMenu.
	 * @param resId Identifiant du restaurant
	 * @param menId Identifiant du menu
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantMenuUriComponentsBuilder(Integer resId, Integer menId) {
		String uri = host + "/api/restaurants/%s/menus/%s".formatted(resId, menId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param menId Identifiant du menu
	 * @return Menu du restaurant
	 */
	public ResponseEntity<MenuComplet> getRestaurantMenu(Integer resId, Integer menId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantMenuUriComponentsBuilder(resId, menId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), MenuComplet.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getRestaurantStatistiques.
	 * @param resId Identifiant du restaurant
	 * @param dateDebut Date de début pour le calcul des statistiques
	 * @param dateFin Date de fin pour le calcul des statistiques
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantStatistiquesUriComponentsBuilder(Integer resId, LocalDateTime dateDebut, LocalDateTime dateFin) {
		String uri = host + "/api/restaurants/%s/statistiques".formatted(resId);
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("dateDebut", dateDebut);
		uriBuilder.queryParam("dateFin", dateFin);
		return uriBuilder;
	}

	/**
	 * Récupère les statistiques d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param dateDebut Date de début pour le calcul des statistiques
	 * @param dateFin Date de fin pour le calcul des statistiques
	 * @return Statistiques du restaurant
	 */
	public ResponseEntity<StatistiquesRestaurant> getRestaurantStatistiques(Integer resId, LocalDateTime dateDebut, LocalDateTime dateFin){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantStatistiquesUriComponentsBuilder(resId, dateDebut, dateFin);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), StatistiquesRestaurant.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode patchPromotion.
	 * @param proId Identifiant de la promotion
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchPromotionUriComponentsBuilder(Integer proId) {
		String uri = host + "/api/restaurants/promotions/%s".formatted(proId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement une promotion.
	 * @param proId Identifiant de la promotion
	 * @param promotion Données partielles de la promotion
	 * @return Promotion mise à jour
	 */
	public ResponseEntity<PromotionRead> patchPromotion(Integer proId, PromotionWrite promotion){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchPromotionUriComponentsBuilder(proId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(promotion, headers), PromotionRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode searchRestaurants.
	 * @param nom Nom du restaurant (recherche partielle)
	 * @param adresse Adresse du restaurant (recherche partielle)
	 * @param noteMin Note minimum requise
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchRestaurantsUriComponentsBuilder(String nom, String adresse, Integer noteMin) {
		String uri = host + "/api/restaurants/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("nom", nom);
		if (adresse != null) {
			uriBuilder.queryParam("adresse", adresse);
		}

		uriBuilder.queryParam("noteMin", noteMin);
		return uriBuilder;
	}

	/**
	 * Recherche avancée de restaurants.
	 * @param nom Nom du restaurant (recherche partielle)
	 * @param adresse Adresse du restaurant (recherche partielle)
	 * @param noteMin Note minimum requise
	 * @return Liste des restaurants correspondant aux critères
	 */
	public ResponseEntity<List<RestaurantAvecStatistiques>> searchRestaurants(String nom, String adresse, Integer noteMin){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchRestaurantsUriComponentsBuilder(nom, adresse, noteMin);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<RestaurantAvecStatistiques>>() {});
	}
}

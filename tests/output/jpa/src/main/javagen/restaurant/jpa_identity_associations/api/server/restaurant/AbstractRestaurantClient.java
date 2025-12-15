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

import restaurant.jpa_identity_associations.dtos.restaurant.MenuRead;
import restaurant.jpa_identity_associations.dtos.restaurant.PlatItem;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantItem;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantRead;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.StatistiquesRestaurant;
import restaurant.jpa_identity_associations.dtos.restaurant.TableItem;
import restaurant.jpa_identity_associations.dtos.restaurant.TableRead;
import restaurant.jpa_identity_associations.dtos.restaurant.TableWrite;
import restaurant.jpa_identity_associations.enums.restaurant.CategoriePlatCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractRestaurantClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractRestaurantClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * UriComponentsBuilder pour la méthode addRestaurant.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addRestaurantUriComponentsBuilder() {
		String uri = host + "/api/restaurants";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer
	 * @return Restaurant créé
	 */
	public ResponseEntity<RestaurantRead> addRestaurant(RestaurantWrite restaurant){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addRestaurantUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(restaurant, headers), RestaurantRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode addTable.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addTableUriComponentsBuilder() {
		String uri = host + "/api/restaurants/tables";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute une table.
	 * @param table Table à créer
	 * @return Table créée
	 */
	public ResponseEntity<TableRead> addTable(TableWrite table){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addTableUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(table, headers), TableRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteRestaurant.
	 * @param resId Identifiant du restaurant
	 */
	protected UriComponentsBuilder deleteRestaurantUriComponentsBuilder(Integer resId) {
		String uri = host + "/api/restaurants/%s".formatted(resId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant
	 */
	public ResponseEntity deleteRestaurant(Integer resId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteRestaurantUriComponentsBuilder(resId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteTable.
	 * @param tabId Identifiant de la table
	 */
	protected UriComponentsBuilder deleteTableUriComponentsBuilder(Integer tabId) {
		String uri = host + "/api/restaurants/tables/%s".formatted(tabId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table
	 */
	public ResponseEntity deleteTable(Integer tabId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteTableUriComponentsBuilder(tabId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode getRestaurant.
	 * @param resId Identifiant du restaurant
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantUriComponentsBuilder(Integer resId) {
		String uri = host + "/api/restaurants/%s".formatted(resId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @return Détail du restaurant
	 */
	public ResponseEntity<RestaurantRead> getRestaurant(Integer resId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantUriComponentsBuilder(resId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), RestaurantRead.class);
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
	public ResponseEntity<MenuRead> getRestaurantMenu(Integer resId, Integer menId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantMenuUriComponentsBuilder(resId, menId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), MenuRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getRestaurantPlats.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si le plat est disponible
	 * @param categoriePlatCode Catégorie du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantPlatsUriComponentsBuilder(Integer resId, Boolean disponible, CategoriePlatCode categoriePlatCode) {
		String uri = host + "/api/restaurants/%s/plats".formatted(resId);
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("disponible", disponible);
		uriBuilder.queryParam("categoriePlatCode", categoriePlatCode);
		return uriBuilder;
	}

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si le plat est disponible
	 * @param categoriePlatCode Catégorie du plat
	 * @return Liste des plats du restaurant
	 */
	public ResponseEntity<List<PlatItem>> getRestaurantPlats(Integer resId, Boolean disponible, CategoriePlatCode categoriePlatCode){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantPlatsUriComponentsBuilder(resId, disponible, categoriePlatCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
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
	 * UriComponentsBuilder pour la méthode getRestaurantTables.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si la table est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantTablesUriComponentsBuilder(Integer resId, Boolean disponible) {
		String uri = host + "/api/restaurants/%s/tables".formatted(resId);
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si la table est disponible
	 * @return Liste des tables du restaurant
	 */
	public ResponseEntity<List<TableItem>> getRestaurantTables(Integer resId, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantTablesUriComponentsBuilder(resId, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<TableItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getRestaurants.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantsUriComponentsBuilder() {
		String uri = host + "/api/restaurants";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste tous les restaurants.
	 * @return Liste des restaurants
	 */
	public ResponseEntity<List<RestaurantItem>> getRestaurants(){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantsUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<RestaurantItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getTable.
	 * @param tabId Identifiant de la table
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getTableUriComponentsBuilder(Integer tabId) {
		String uri = host + "/api/restaurants/tables/%s".formatted(tabId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table
	 * @return Détail de la table
	 */
	public ResponseEntity<TableRead> getTable(Integer tabId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getTableUriComponentsBuilder(tabId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), TableRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getTables.
	 * @param restaurantId Restaurant auquel appartient la table
	 * @param disponible Indique si la table est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getTablesUriComponentsBuilder(Integer restaurantId, Boolean disponible) {
		String uri = host + "/api/restaurants/tables";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("restaurantId", restaurantId);
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Liste toutes les tables.
	 * @param restaurantId Restaurant auquel appartient la table
	 * @param disponible Indique si la table est disponible
	 * @return Liste des tables
	 */
	public ResponseEntity<List<TableItem>> getTables(Integer restaurantId, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getTablesUriComponentsBuilder(restaurantId, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<TableItem>>() {});
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

	/**
	 * UriComponentsBuilder pour la méthode updateRestaurant.
	 * @param resId Identifiant du restaurant
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateRestaurantUriComponentsBuilder(Integer resId) {
		String uri = host + "/api/restaurants/%s".formatted(resId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param restaurant Restaurant à mettre à jour
	 * @return Restaurant mis à jour
	 */
	public ResponseEntity<RestaurantRead> updateRestaurant(Integer resId, RestaurantWrite restaurant){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateRestaurantUriComponentsBuilder(resId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(restaurant, headers), RestaurantRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updateTable.
	 * @param tabId Identifiant de la table
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateTableUriComponentsBuilder(Integer tabId) {
		String uri = host + "/api/restaurants/tables/%s".formatted(tabId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table
	 * @param table Table à mettre à jour
	 * @return Table mise à jour
	 */
	public ResponseEntity<TableRead> updateTable(Integer tabId, TableWrite table){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateTableUriComponentsBuilder(tabId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(table, headers), TableRead.class);
	}
}

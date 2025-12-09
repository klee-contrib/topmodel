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

import restaurant.jpa_identity_associations.dtos.restaurant.ClientItem;
import restaurant.jpa_identity_associations.dtos.restaurant.ClientRead;
import restaurant.jpa_identity_associations.dtos.restaurant.ClientWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeItem;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeRead;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.PlatItem;
import restaurant.jpa_identity_associations.dtos.restaurant.PlatRead;
import restaurant.jpa_identity_associations.dtos.restaurant.PlatWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantItem;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantRead;
import restaurant.jpa_identity_associations.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.TableClientItem;
import restaurant.jpa_identity_associations.dtos.restaurant.TableClientRead;
import restaurant.jpa_identity_associations.dtos.restaurant.TableClientWrite;
import restaurant.jpa_identity_associations.entities.restaurant.CategoriePlat;
import restaurant.jpa_identity_associations.entities.restaurant.StatutCommande;
import restaurant.jpa_identity_associations.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_identity_associations.enums.restaurant.StatutCommandeCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractEndpointsClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractEndpointsClient(RestTemplate restTemplate, String host) {
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
	 * UriComponentsBuilder pour la méthode addCommande.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addCommandeUriComponentsBuilder() {
		String uri = host + "/api/restaurants/commandes";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Crée une nouvelle commande.
	 * @param commande Commande à créer
	 * @return Commande créée
	 */
	public ResponseEntity<CommandeRead> addCommande(CommandeWrite commande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addCommandeUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(commande, headers), CommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode addLigneCommande.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addLigneCommandeUriComponentsBuilder() {
		String uri = host + "/api/restaurants/ligne-commandes";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute une ligne de commande.
	 * @param ligneCommande Ligne de commande à créer
	 * @return Ligne de commande créée
	 */
	public ResponseEntity<LigneCommandeRead> addLigneCommande(LigneCommandeWrite ligneCommande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addLigneCommandeUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(ligneCommande, headers), LigneCommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode addPlat.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addPlatUriComponentsBuilder() {
		String uri = host + "/api/restaurants/plats";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer
	 * @return Plat créé
	 */
	public ResponseEntity<PlatRead> addPlat(PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addPlatUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(plat, headers), PlatRead.class);
	}

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
	public ResponseEntity<TableClientRead> addTable(TableClientWrite table){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addTableUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(table, headers), TableClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteClient.
	 * @param cliId Identifiant du client
	 */
	protected UriComponentsBuilder deleteClientUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un client.
	 * @param cliId Identifiant du client
	 */
	public ResponseEntity deleteClient(Integer cliId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteClientUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteCommande.
	 * @param comId Identifiant de la commande
	 */
	protected UriComponentsBuilder deleteCommandeUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande
	 */
	public ResponseEntity deleteCommande(Integer comId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteCommandeUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode deleteLigneCommande.
	 * @param ligId Identifiant de la ligne
	 */
	protected UriComponentsBuilder deleteLigneCommandeUriComponentsBuilder(Integer ligId) {
		String uri = host + "/api/restaurants/ligne-commandes/%s".formatted(ligId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime une ligne de commande.
	 * @param ligId Identifiant de la ligne
	 */
	public ResponseEntity deleteLigneCommande(Integer ligId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deleteLigneCommandeUriComponentsBuilder(ligId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode deletePlat.
	 * @param plaId Identifiant du plat
	 */
	protected UriComponentsBuilder deletePlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat
	 */
	public ResponseEntity deletePlat(Integer plaId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deletePlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
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
	 * UriComponentsBuilder pour la méthode getCategoriePlats.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCategoriePlatsUriComponentsBuilder() {
		String uri = host + "/api/restaurants/categorie-plats";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste toutes les catégories de plats.
	 * @return Liste des catégories de plats
	 */
	public ResponseEntity<List<CategoriePlat>> getCategoriePlats(){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCategoriePlatsUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CategoriePlat>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getClient.
	 * @param cliId Identifiant du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un client.
	 * @param cliId Identifiant du client
	 * @return Détail du client
	 */
	public ResponseEntity<ClientRead> getClient(Integer cliId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getClientCommandes.
	 * @param cliId Identifiant du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getClientCommandesUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s/commandes".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste les commandes d'un client.
	 * @param cliId Identifiant du client
	 * @return Liste des commandes du client
	 */
	public ResponseEntity<List<CommandeItem>> getClientCommandes(Integer cliId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientCommandesUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getClients.
	 * @param nom Nom du client
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
	 * @param nom Nom du client
	 * @param email Adresse email du client
	 * @return Liste des clients
	 */
	public ResponseEntity<List<ClientItem>> getClients(String nom, String email){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getClientsUriComponentsBuilder(nom, email);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<ClientItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getCommande.
	 * @param comId Identifiant de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandeUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande
	 * @return Détail de la commande
	 */
	public ResponseEntity<CommandeRead> getCommande(Integer comId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandeUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), CommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getCommandeLignes.
	 * @param comId Identifiant de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandeLignesUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s/lignes".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste les lignes d'une commande.
	 * @param comId Identifiant de la commande
	 * @return Liste des lignes de la commande
	 */
	public ResponseEntity<List<LigneCommandeItem>> getCommandeLignes(Integer comId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandeLignesUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<LigneCommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getCommandes.
	 * @param clientId Client ayant passé la commande
	 * @param statutCommandeCode Statut de la commande
	 * @param tableClientId Table associée à la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandesUriComponentsBuilder(Integer clientId, StatutCommandeCode statutCommandeCode, Integer tableClientId) {
		String uri = host + "/api/restaurants/commandes";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("clientId", clientId);
		uriBuilder.queryParam("statutCommandeCode", statutCommandeCode);
		if (tableClientId != null) {
			uriBuilder.queryParam("tableClientId", tableClientId);
		}

		return uriBuilder;
	}

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande
	 * @param statutCommandeCode Statut de la commande
	 * @param tableClientId Table associée à la commande
	 * @return Liste des commandes
	 */
	public ResponseEntity<List<CommandeItem>> getCommandes(Integer clientId, StatutCommandeCode statutCommandeCode, Integer tableClientId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandesUriComponentsBuilder(clientId, statutCommandeCode, tableClientId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getCommandesByDate.
	 * @param dateCommande Date et heure de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandesByDateUriComponentsBuilder(LocalDateTime dateCommande) {
		String uri = host + "/api/restaurants/commandes/by-date";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("dateCommande", dateCommande);
		return uriBuilder;
	}

	/**
	 * Récupère les commandes par date.
	 * @param dateCommande Date et heure de la commande
	 * @return Commandes pour la date spécifiée
	 */
	public ResponseEntity<List<CommandeItem>> getCommandesByDate(LocalDateTime dateCommande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandesByDateUriComponentsBuilder(dateCommande);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getLigneCommande.
	 * @param ligId Identifiant de la ligne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getLigneCommandeUriComponentsBuilder(Integer ligId) {
		String uri = host + "/api/restaurants/ligne-commandes/%s".formatted(ligId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'une ligne de commande.
	 * @param ligId Identifiant de la ligne
	 * @return Détail de la ligne de commande
	 */
	public ResponseEntity<LigneCommandeRead> getLigneCommande(Integer ligId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getLigneCommandeUriComponentsBuilder(ligId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), LigneCommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getLigneCommandes.
	 * @param commandeId Commande à laquelle appartient la ligne
	 * @param platId Plat commandé
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getLigneCommandesUriComponentsBuilder(Integer commandeId, Integer platId) {
		String uri = host + "/api/restaurants/ligne-commandes";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("commandeId", commandeId);
		uriBuilder.queryParam("platId", platId);
		return uriBuilder;
	}

	/**
	 * Liste toutes les lignes de commande.
	 * @param commandeId Commande à laquelle appartient la ligne
	 * @param platId Plat commandé
	 * @return Liste des lignes de commande
	 */
	public ResponseEntity<List<LigneCommandeItem>> getLigneCommandes(Integer commandeId, Integer platId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getLigneCommandesUriComponentsBuilder(commandeId, platId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<LigneCommandeItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode getPlat.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getPlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat
	 * @return Détail du plat
	 */
	public ResponseEntity<PlatRead> getPlat(Integer plaId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getPlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), PlatRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getPlats.
	 * @param disponible Indique si le plat est disponible
	 * @param restaurantIdRestaurant Restaurant proposant ce plat
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getPlatsUriComponentsBuilder(Boolean disponible, Integer restaurantIdRestaurant, CategoriePlatCode categoriePlatCodeCategoriePlat) {
		String uri = host + "/api/restaurants/plats";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("disponible", disponible);
		uriBuilder.queryParam("restaurantIdRestaurant", restaurantIdRestaurant);
		uriBuilder.queryParam("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat);
		return uriBuilder;
	}

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible
	 * @param restaurantIdRestaurant Restaurant proposant ce plat
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @return Liste des plats
	 */
	public ResponseEntity<List<PlatItem>> getPlats(Boolean disponible, Integer restaurantIdRestaurant, CategoriePlatCode categoriePlatCodeCategoriePlat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getPlatsUriComponentsBuilder(disponible, restaurantIdRestaurant, categoriePlatCodeCategoriePlat);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
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
	 * UriComponentsBuilder pour la méthode getRestaurantPlats.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si le plat est disponible
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getRestaurantPlatsUriComponentsBuilder(Integer resId, Boolean disponible, CategoriePlatCode categoriePlatCodeCategoriePlat) {
		String uri = host + "/api/restaurants/%s/plats".formatted(resId);
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("disponible", disponible);
		uriBuilder.queryParam("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat);
		return uriBuilder;
	}

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant
	 * @param disponible Indique si le plat est disponible
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @return Liste des plats du restaurant
	 */
	public ResponseEntity<List<PlatItem>> getRestaurantPlats(Integer resId, Boolean disponible, CategoriePlatCode categoriePlatCodeCategoriePlat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantPlatsUriComponentsBuilder(resId, disponible, categoriePlatCodeCategoriePlat);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
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
	public ResponseEntity<List<TableClientItem>> getRestaurantTables(Integer resId, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getRestaurantTablesUriComponentsBuilder(resId, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<TableClientItem>>() {});
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
	 * UriComponentsBuilder pour la méthode getStatutCommandes.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getStatutCommandesUriComponentsBuilder() {
		String uri = host + "/api/restaurants/statuts-commande";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste tous les statuts de commande.
	 * @return Liste des statuts de commande
	 */
	public ResponseEntity<List<StatutCommande>> getStatutCommandes(){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getStatutCommandesUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<StatutCommande>>() {});
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
	public ResponseEntity<TableClientRead> getTable(Integer tabId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getTableUriComponentsBuilder(tabId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), TableClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode getTables.
	 * @param restaurantIdRestaurant Restaurant auquel appartient la table
	 * @param disponible Indique si la table est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getTablesUriComponentsBuilder(Integer restaurantIdRestaurant, Boolean disponible) {
		String uri = host + "/api/restaurants/tables";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("restaurantIdRestaurant", restaurantIdRestaurant);
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Liste toutes les tables.
	 * @param restaurantIdRestaurant Restaurant auquel appartient la table
	 * @param disponible Indique si la table est disponible
	 * @return Liste des tables
	 */
	public ResponseEntity<List<TableClientItem>> getTables(Integer restaurantIdRestaurant, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getTablesUriComponentsBuilder(restaurantIdRestaurant, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<TableClientItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode patchClient.
	 * @param cliId Identifiant du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchClientUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement un client.
	 * @param cliId Identifiant du client
	 * @param client Données partielles du client
	 * @return Client mis à jour
	 */
	public ResponseEntity<ClientRead> patchClient(Integer cliId, ClientWrite client){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchClientUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(client, headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode patchCommande.
	 * @param comId Identifiant de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchCommandeUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement une commande.
	 * @param comId Identifiant de la commande
	 * @param commande Données partielles de la commande
	 * @return Commande mise à jour
	 */
	public ResponseEntity<CommandeRead> patchCommande(Integer comId, CommandeWrite commande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchCommandeUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(commande, headers), CommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode patchPlat.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchPlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat
	 * @param plat Données partielles du plat
	 * @return Plat mis à jour
	 */
	public ResponseEntity<PlatRead> patchPlat(Integer plaId, PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchPlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(plat, headers), PlatRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode searchPlats.
	 * @param nom Nom du plat
	 * @param restaurantIdRestaurant Restaurant proposant ce plat
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchPlatsUriComponentsBuilder(String nom, Integer restaurantIdRestaurant, CategoriePlatCode categoriePlatCodeCategoriePlat, Boolean disponible) {
		String uri = host + "/api/restaurants/plats/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("nom", nom);
		uriBuilder.queryParam("restaurantIdRestaurant", restaurantIdRestaurant);
		uriBuilder.queryParam("categoriePlatCodeCategoriePlat", categoriePlatCodeCategoriePlat);
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Recherche de plats avec critères multiples.
	 * @param nom Nom du plat
	 * @param restaurantIdRestaurant Restaurant proposant ce plat
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return Plats correspondant aux critères de recherche
	 */
	public ResponseEntity<List<PlatItem>> searchPlats(String nom, Integer restaurantIdRestaurant, CategoriePlatCode categoriePlatCodeCategoriePlat, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchPlatsUriComponentsBuilder(nom, restaurantIdRestaurant, categoriePlatCodeCategoriePlat, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode updateClient.
	 * @param cliId Identifiant du client
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateClientUriComponentsBuilder(Integer cliId) {
		String uri = host + "/api/restaurants/clients/%s".formatted(cliId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour un client.
	 * @param cliId Identifiant du client
	 * @param client Client à mettre à jour
	 * @return Client mis à jour
	 */
	public ResponseEntity<ClientRead> updateClient(Integer cliId, ClientWrite client){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateClientUriComponentsBuilder(cliId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(client, headers), ClientRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updateCommande.
	 * @param comId Identifiant de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateCommandeUriComponentsBuilder(Integer comId) {
		String uri = host + "/api/restaurants/commandes/%s".formatted(comId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour une commande.
	 * @param comId Identifiant de la commande
	 * @param commande Commande à mettre à jour
	 * @return Commande mise à jour
	 */
	public ResponseEntity<CommandeRead> updateCommande(Integer comId, CommandeWrite commande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateCommandeUriComponentsBuilder(comId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(commande, headers), CommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updateCommandeStatut.
	 * @param comId Identifiant de la commande
	 * @param statutCommandeCode Statut de la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateCommandeStatutUriComponentsBuilder(Integer comId, StatutCommandeCode statutCommandeCode) {
		String uri = host + "/api/restaurants/commandes/%s/statut".formatted(comId);
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("statutCommandeCode", statutCommandeCode);
		return uriBuilder;
	}

	/**
	 * Met à jour uniquement le statut d'une commande.
	 * @param comId Identifiant de la commande
	 * @param statutCommandeCode Statut de la commande
	 * @return Commande avec le statut mis à jour
	 */
	public ResponseEntity<CommandeRead> updateCommandeStatut(Integer comId, StatutCommandeCode statutCommandeCode){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateCommandeStatutUriComponentsBuilder(comId, statutCommandeCode);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(headers), CommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updateLigneCommande.
	 * @param ligId Identifiant de la ligne
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updateLigneCommandeUriComponentsBuilder(Integer ligId) {
		String uri = host + "/api/restaurants/ligne-commandes/%s".formatted(ligId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour une ligne de commande.
	 * @param ligId Identifiant de la ligne
	 * @param ligneCommande Ligne de commande à mettre à jour
	 * @return Ligne de commande mise à jour
	 */
	public ResponseEntity<LigneCommandeRead> updateLigneCommande(Integer ligId, LigneCommandeWrite ligneCommande){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateLigneCommandeUriComponentsBuilder(ligId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(ligneCommande, headers), LigneCommandeRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode updatePlat.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updatePlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat
	 * @param plat Plat à mettre à jour
	 * @return Plat mis à jour
	 */
	public ResponseEntity<PlatRead> updatePlat(Integer plaId, PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updatePlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(plat, headers), PlatRead.class);
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
	public ResponseEntity<TableClientRead> updateTable(Integer tabId, TableClientWrite table){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updateTableUriComponentsBuilder(tabId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(table, headers), TableClientRead.class);
	}
}

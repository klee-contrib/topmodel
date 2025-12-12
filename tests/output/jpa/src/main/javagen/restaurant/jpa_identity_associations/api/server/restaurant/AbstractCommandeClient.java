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

import restaurant.jpa_identity_associations.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeItem;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeRead;
import restaurant.jpa_identity_associations.dtos.restaurant.CommandeWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_identity_associations.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_identity_associations.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_identity_associations.dtos.restaurant.ReservationWrite;
import restaurant.jpa_identity_associations.entities.restaurant.StatutCommande;
import restaurant.jpa_identity_associations.enums.restaurant.StatutCommandeCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractCommandeClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractCommandeClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return les headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

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
	 * @param tableId Table associée à la commande
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCommandesUriComponentsBuilder(Integer clientId, StatutCommandeCode statutCommandeCode, Integer tableId) {
		String uri = host + "/api/restaurants/commandes";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("clientId", clientId);
		uriBuilder.queryParam("statutCommandeCode", statutCommandeCode);
		if (tableId != null) {
			uriBuilder.queryParam("tableId", tableId);
		}

		return uriBuilder;
	}

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande
	 * @param statutCommandeCode Statut de la commande
	 * @param tableId Table associée à la commande
	 * @return Liste des commandes
	 */
	public ResponseEntity<List<CommandeItem>> getCommandes(Integer clientId, StatutCommandeCode statutCommandeCode, Integer tableId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCommandesUriComponentsBuilder(clientId, statutCommandeCode, tableId);
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
}

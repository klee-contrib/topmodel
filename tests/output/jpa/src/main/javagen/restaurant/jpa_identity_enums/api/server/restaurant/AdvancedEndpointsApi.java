////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_identity_enums.dtos.restaurant.AvisClientRead;
import restaurant.jpa_identity_enums.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_identity_enums.dtos.restaurant.EmployeRead;
import restaurant.jpa_identity_enums.dtos.restaurant.EmployeWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.MenuComplet;
import restaurant.jpa_identity_enums.dtos.restaurant.MenuWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.PromotionRead;
import restaurant.jpa_identity_enums.dtos.restaurant.PromotionWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_identity_enums.dtos.restaurant.ReservationWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_identity_enums.dtos.restaurant.StatistiquesRestaurant;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@FeignClient(name = "Restaurant", contextId = "AdvancedEndpointsApi")
public interface AdvancedEndpointsApi {

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer.
	 *
	 * @return Employé créé.
	 */
	@PreAuthorize("isAuthenticated()")
	@PostMapping(path = "api/restaurants/employes")
	@Operation(description = "Ajoute un employé (nécessite le rôle ADMIN)")
	EmployeRead addEmploye(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Employé à créer") @RequestBody @Valid EmployeWrite employe);

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer.
	 *
	 * @return Menu créé avec ses plats.
	 */
	@PreAuthorize("isAuthenticated()")
	@PostMapping(path = "api/restaurants/menus")
	@Operation(description = "Crée un menu avec ses plats")
	MenuComplet createMenu(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Menu à créer") @RequestBody @Valid MenuWrite menu);

	/**
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@Operation(description = "Crée une réservation")
	@PostMapping(path = "api/restaurants/reservations")
	ReservationAvecDetails createReservation(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Réservation à créer") @RequestBody @Valid ReservationWrite reservation);

	/**
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@PreAuthorize("isAuthenticated()")
	@Operation(description = "Exporte les commandes au format CSV")
	@GetMapping(path = "api/restaurants/commandes/export", produces = "application/octet-stream")
	byte[] exportCommandes(@Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Liste les avis clients avec filtres.
	 * @param resRestaurantId Identifiant du restaurant.
	 * @param noteMin Note sur 5.
	 * @param approuve Indique si l'avis est approuvé par le restaurant.
	 * @param dateDebut Date de l'avis.
	 * @param dateFin Date de l'avis.
	 *
	 * @return Liste des avis correspondant aux critères.
	 */
	@GetMapping(path = "api/restaurants/avis")
	@Operation(description = "Liste les avis clients avec filtres")
	List<AvisClientRead> getAvisClients(@Parameter(description = "Identifiant du restaurant") @RequestParam(value = "resRestaurantId", required = true) Integer resRestaurantId, @Parameter(description = "Note sur 5") @RequestParam(value = "noteMin", required = true) Integer noteMin, @Parameter(description = "Indique si l'avis est approuvé par le restaurant") @RequestParam(value = "approuve", required = true) Boolean approuve, @Parameter(description = "Date de l'avis") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date de l'avis") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param cliId Identifiant du client.
	 *
	 * @return Client avec ses commandes.
	 */
	@GetMapping(path = "api/restaurants/clients/{cliId}/avec-commandes")
	@Operation(description = "Récupère un client avec toutes ses commandes")
	ClientAvecCommandes getClientAvecCommandes(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Récupère le détail complet d'une commande avec ses lignes.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail complet de la commande.
	 */
	@GetMapping(path = "api/restaurants/commandes/{comId}/detail")
	@Operation(description = "Récupère le détail complet d'une commande avec ses lignes")
	CommandeDetailRead getCommandeDetail(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param menId Identifiant du menu.
	 *
	 * @return Menu du restaurant.
	 */
	@GetMapping(path = "api/restaurants/{resId}/menus/{menId}")
	@Operation(description = "Récupère un menu spécifique d'un restaurant")
	MenuComplet getRestaurantMenu(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Identifiant du menu") @PathVariable("menId") Integer menId);

	/**
	 * Récupère les statistiques d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param dateDebut Date de début pour le calcul des statistiques.
	 * @param dateFin Date de fin pour le calcul des statistiques.
	 *
	 * @return Statistiques du restaurant.
	 */
	@PreAuthorize("isAuthenticated()")
	@GetMapping(path = "api/restaurants/{resId}/statistiques")
	@Operation(description = "Récupère les statistiques d'un restaurant")
	StatistiquesRestaurant getRestaurantStatistiques(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Date de début pour le calcul des statistiques") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date de fin pour le calcul des statistiques") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Met à jour partiellement une promotion.
	 * @param proId Identifiant de la promotion.
	 * @param promotion Données partielles de la promotion.
	 *
	 * @return Promotion mise à jour.
	 */
	@PreAuthorize("isAuthenticated()")
	@PatchMapping(path = "api/restaurants/promotions/{proId}")
	@Operation(description = "Met à jour partiellement une promotion")
	PromotionRead patchPromotion(@Parameter(description = "Identifiant de la promotion") @PathVariable("proId") Integer proId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles de la promotion") @RequestBody @Valid PromotionWrite promotion);

	/**
	 * Recherche avancée de restaurants.
	 * @param nom Nom du restaurant (recherche partielle).
	 * @param adresse Adresse du restaurant (recherche partielle).
	 * @param noteMin Note minimum requise.
	 *
	 * @return Liste des restaurants correspondant aux critères.
	 */
	@GetMapping(path = "api/restaurants/search")
	@Operation(description = "Recherche avancée de restaurants")
	List<RestaurantAvecStatistiques> searchRestaurants(@Parameter(description = "Nom du restaurant (recherche partielle)") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Adresse du restaurant (recherche partielle)") @RequestParam(value = "adresse", required = false) String adresse, @Parameter(description = "Note minimum requise") @RequestParam(value = "noteMin", required = true) Integer noteMin);
}

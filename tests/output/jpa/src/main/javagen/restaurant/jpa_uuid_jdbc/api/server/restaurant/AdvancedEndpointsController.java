////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_uuid_jdbc.dtos.restaurant.AvisClientRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.EmployeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.EmployeWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuComplet;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PromotionRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PromotionWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ReservationWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.StatistiquesRestaurant;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface AdvancedEndpointsController {

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer.
	 *
	 * @return Employé créé.
	 */
	@PostMapping(path = "employes")
	@PreAuthorize("isAuthenticated()")
	EmployeRead addEmploye(@RequestBody @Valid EmployeWrite employe);

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer.
	 *
	 * @return Menu créé avec ses plats.
	 */
	@PostMapping(path = "menus")
	@PreAuthorize("isAuthenticated()")
	MenuComplet createMenu(@RequestBody @Valid MenuWrite menu);

	/**
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@PostMapping(path = "reservations")
	ReservationAvecDetails createReservation(@RequestBody @Valid ReservationWrite reservation);

	/**
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@PreAuthorize("isAuthenticated()")
	@GetMapping(path = "commandes/export", produces = "application/octet-stream")
	byte[] exportCommandes(@RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

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
	@GetMapping(path = "avis")
	List<AvisClientRead> getAvisClients(@RequestParam(value = "resRestaurantId", required = true) Integer resRestaurantId, @RequestParam(value = "noteMin", required = true) Integer noteMin, @RequestParam(value = "approuve", required = true) Boolean approuve, @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param cliId Identifiant du client.
	 *
	 * @return Client avec ses commandes.
	 */
	@GetMapping(path = "clients/{cliId}/avec-commandes")
	ClientAvecCommandes getClientAvecCommandes(@PathVariable("cliId") Integer cliId);

	/**
	 * Récupère le détail complet d'une commande avec ses lignes.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail complet de la commande.
	 */
	@GetMapping(path = "commandes/{comId}/detail")
	CommandeDetailRead getCommandeDetail(@PathVariable("comId") Integer comId);

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param menId Identifiant du menu.
	 *
	 * @return Menu du restaurant.
	 */
	@GetMapping(path = "{resId}/menus/{menId}")
	MenuComplet getRestaurantMenu(@PathVariable("resId") Integer resId, @PathVariable("menId") Integer menId);

	/**
	 * Récupère les statistiques d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param dateDebut Date de début pour le calcul des statistiques.
	 * @param dateFin Date de fin pour le calcul des statistiques.
	 *
	 * @return Statistiques du restaurant.
	 */
	@PreAuthorize("isAuthenticated()")
	@GetMapping(path = "{resId}/statistiques")
	StatistiquesRestaurant getRestaurantStatistiques(@PathVariable("resId") Integer resId, @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Met à jour partiellement une promotion.
	 * @param proId Identifiant de la promotion.
	 * @param promotion Données partielles de la promotion.
	 *
	 * @return Promotion mise à jour.
	 */
	@PreAuthorize("isAuthenticated()")
	@PatchMapping(path = "promotions/{proId}")
	PromotionRead patchPromotion(@PathVariable("proId") Integer proId, @RequestBody @Valid PromotionWrite promotion);

	/**
	 * Recherche avancée de restaurants.
	 * @param nom Nom du restaurant (recherche partielle).
	 * @param adresse Adresse du restaurant (recherche partielle).
	 * @param noteMin Note minimum requise.
	 *
	 * @return Liste des restaurants correspondant aux critères.
	 */
	@GetMapping(path = "search")
	List<RestaurantAvecStatistiques> searchRestaurants(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "adresse", required = false) String adresse, @RequestParam(value = "noteMin", required = true) Integer noteMin);
}

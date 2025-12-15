////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.StatistiquesRestaurant;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableWrite;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface RestaurantController {

	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer.
	 *
	 * @return Restaurant créé.
	 */
	@PostMapping(path = "")
	RestaurantRead addRestaurant(@RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@PostMapping(path = "tables")
	TableRead addTable(@RequestBody @Valid TableWrite table);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 */
	@DeleteMapping(path = "{resId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	void deleteRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "tables/{tabId}")
	void deleteTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetMapping(path = "{resId}")
	RestaurantRead getRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param menId Identifiant du menu.
	 *
	 * @return Menu du restaurant.
	 */
	@GetMapping(path = "{resId}/menus/{menId}")
	MenuRead getRestaurantMenu(@PathVariable("resId") Integer resId, @PathVariable("menId") Integer menId);

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si le plat est disponible.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats du restaurant.
	 */
	@GetMapping(path = "{resId}/plats")
	List<PlatItem> getRestaurantPlats(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "categoriePlatCode", required = true) String categoriePlatCode);

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
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetMapping(path = "{resId}/tables")
	List<TableItem> getRestaurantTables(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetMapping(path = "")
	List<RestaurantItem> getRestaurants();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetMapping(path = "tables/{tabId}")
	TableRead getTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantId Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetMapping(path = "tables")
	List<TableItem> getTables(@RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "disponible", required = true) Boolean disponible);

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

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutMapping(path = "{resId}")
	RestaurantRead updateRestaurant(@PathVariable("resId") Integer resId, @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@PutMapping(path = "tables/{tabId}")
	TableRead updateTable(@PathVariable("tabId") Integer tabId, @RequestBody @Valid TableWrite table);
}

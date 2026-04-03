////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.api.server.restaurant;

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

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_sequence_server.dtos.restaurant.MenuRead;
import restaurant.jpa_sequence_server.dtos.restaurant.PlatItem;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantItem;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantRead;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.StatistiquesRestaurant;
import restaurant.jpa_sequence_server.dtos.restaurant.TableItem;
import restaurant.jpa_sequence_server.dtos.restaurant.TableRead;
import restaurant.jpa_sequence_server.dtos.restaurant.TableWrite;
import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatCode;

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
	@Operation(description = "Ajoute un restaurant")
	RestaurantRead addRestaurant(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à créer") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@PostMapping(path = "tables")
	@Operation(description = "Ajoute une table")
	TableRead addTable(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à créer") @RequestBody @Valid TableWrite table);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 */
	@DeleteMapping(path = "{resId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un restaurant")
	void deleteRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "tables/{tabId}")
	@Operation(description = "Supprime une table")
	void deleteTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetMapping(path = "{resId}")
	@Operation(description = "Charge le détail d'un restaurant")
	RestaurantRead getRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId);

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param menId Identifiant du menu.
	 *
	 * @return Menu du restaurant.
	 */
	@GetMapping(path = "{resId}/menus/{menId}")
	@Operation(description = "Récupère un menu spécifique d'un restaurant")
	MenuRead getRestaurantMenu(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Identifiant du menu") @PathVariable("menId") Integer menId);

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si le plat est disponible.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats du restaurant.
	 */
	@GetMapping(path = "{resId}/plats")
	@Operation(description = "Liste les plats d'un restaurant")
	List<PlatItem> getRestaurantPlats(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si le plat est disponible") @RequestParam(required = true, value = "disponible") Boolean disponible, @Parameter(description = "Catégorie du plat") @RequestParam(required = true, value = "categoriePlatCode") CategoriePlatCode categoriePlatCode);

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
	@Operation(description = "Récupère les statistiques d'un restaurant")
	StatistiquesRestaurant getRestaurantStatistiques(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Date de début pour le calcul des statistiques") @RequestParam(required = true, value = "dateDebut") LocalDateTime dateDebut, @Parameter(description = "Date de fin pour le calcul des statistiques") @RequestParam(required = true, value = "dateFin") LocalDateTime dateFin);

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetMapping(path = "{resId}/tables")
	@Operation(description = "Liste les tables d'un restaurant")
	List<TableItem> getRestaurantTables(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si la table est disponible") @RequestParam(required = true, value = "disponible") Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetMapping(path = "")
	@Operation(description = "Liste tous les restaurants")
	List<RestaurantItem> getRestaurants();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetMapping(path = "tables/{tabId}")
	@Operation(description = "Charge le détail d'une table")
	TableRead getTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantId Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetMapping(path = "tables")
	@Operation(description = "Liste toutes les tables")
	List<TableItem> getTables(@Parameter(description = "Restaurant auquel appartient la table") @RequestParam(required = true, value = "restaurantId") Integer restaurantId, @Parameter(description = "Indique si la table est disponible") @RequestParam(required = true, value = "disponible") Boolean disponible);

	/**
	 * Recherche avancée de restaurants.
	 * @param nom Nom du restaurant (recherche partielle).
	 * @param adresse Adresse du restaurant (recherche partielle).
	 * @param noteMin Note minimum requise.
	 *
	 * @return Liste des restaurants correspondant aux critères.
	 */
	@GetMapping(path = "search")
	@Operation(description = "Recherche avancée de restaurants")
	List<RestaurantAvecStatistiques> searchRestaurants(@Parameter(description = "Nom du restaurant (recherche partielle)") @RequestParam(required = true, value = "nom") String nom, @Parameter(description = "Adresse du restaurant (recherche partielle)") @RequestParam(required = false, value = "adresse") String adresse, @Parameter(description = "Note minimum requise") @RequestParam(required = true, value = "noteMin") Integer noteMin);

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutMapping(path = "{resId}")
	@Operation(description = "Met à jour un restaurant")
	RestaurantRead updateRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à mettre à jour") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@PutMapping(path = "tables/{tabId}")
	@Operation(description = "Met à jour une table")
	TableRead updateTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à mettre à jour") @RequestBody @Valid TableWrite table);
}

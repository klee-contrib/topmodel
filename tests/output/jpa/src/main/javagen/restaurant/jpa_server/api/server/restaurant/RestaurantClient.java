////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.service.annotation.DeleteExchange;
import org.springframework.web.service.annotation.GetExchange;
import org.springframework.web.service.annotation.HttpExchange;
import org.springframework.web.service.annotation.PostExchange;
import org.springframework.web.service.annotation.PutExchange;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_server.dtos.restaurant.MenuRead;
import restaurant.jpa_server.dtos.restaurant.PlatItem;
import restaurant.jpa_server.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_server.dtos.restaurant.RestaurantItem;
import restaurant.jpa_server.dtos.restaurant.RestaurantRead;
import restaurant.jpa_server.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_server.dtos.restaurant.StatistiquesRestaurant;
import restaurant.jpa_server.dtos.restaurant.TableItem;
import restaurant.jpa_server.dtos.restaurant.TableRead;
import restaurant.jpa_server.dtos.restaurant.TableWrite;
import restaurant.jpa_server.enums.restaurant.CategoriePlatCode;

@HttpExchange("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface RestaurantClient {


	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer.
	 *
	 * @return Restaurant créé.
	 */
	@PostExchange("/")
	ResponseEntity<RestaurantRead> addRestaurant(@RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@PostExchange("/tables")
	ResponseEntity<TableRead> addTable(@RequestBody @Valid TableWrite table);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Aucun retour.
	 */
	@DeleteExchange("/{resId}")
	ResponseEntity<Void> deleteRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Aucun retour.
	 */
	@DeleteExchange("/tables/{tabId}")
	ResponseEntity<Void> deleteTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetExchange("/{resId}")
	ResponseEntity<RestaurantRead> getRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Récupère un menu spécifique d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param menId Identifiant du menu.
	 *
	 * @return Menu du restaurant.
	 */
	@GetExchange("/{resId}/menus/{menId}")
	ResponseEntity<MenuRead> getRestaurantMenu(@PathVariable("resId") Integer resId, @PathVariable("menId") Integer menId);

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si le plat est disponible.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats du restaurant.
	 */
	@GetExchange("/{resId}/plats")
	ResponseEntity<List<PlatItem>> getRestaurantPlats(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "categoriePlatCode", required = true) CategoriePlatCode categoriePlatCode);

	/**
	 * Récupère les statistiques d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param dateDebut Date de début pour le calcul des statistiques.
	 * @param dateFin Date de fin pour le calcul des statistiques.
	 *
	 * @return Statistiques du restaurant.
	 */
	@GetExchange("/{resId}/statistiques")
	ResponseEntity<StatistiquesRestaurant> getRestaurantStatistiques(@PathVariable("resId") Integer resId, @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetExchange("/{resId}/tables")
	ResponseEntity<List<TableItem>> getRestaurantTables(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetExchange("/")
	ResponseEntity<List<RestaurantItem>> getRestaurants();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetExchange("/tables/{tabId}")
	ResponseEntity<TableRead> getTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantId Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetExchange("/tables")
	ResponseEntity<List<TableItem>> getTables(@RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Recherche avancée de restaurants.
	 * @param nom Nom du restaurant (recherche partielle).
	 * @param adresse Adresse du restaurant (recherche partielle).
	 * @param noteMin Note minimum requise.
	 *
	 * @return Liste des restaurants correspondant aux critères.
	 */
	@GetExchange("/search")
	ResponseEntity<List<RestaurantAvecStatistiques>> searchRestaurants(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "adresse", required = false) String adresse, @RequestParam(value = "noteMin", required = true) Integer noteMin);

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutExchange("/{resId}")
	ResponseEntity<RestaurantRead> updateRestaurant(@PathVariable("resId") Integer resId, @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@PutExchange("/tables/{tabId}")
	ResponseEntity<TableRead> updateTable(@PathVariable("tabId") Integer tabId, @RequestBody @Valid TableWrite table);
}

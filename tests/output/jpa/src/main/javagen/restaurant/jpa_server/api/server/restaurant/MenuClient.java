////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.api.server.restaurant;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.service.annotation.DeleteExchange;
import org.springframework.web.service.annotation.GetExchange;
import org.springframework.web.service.annotation.HttpExchange;
import org.springframework.web.service.annotation.PatchExchange;
import org.springframework.web.service.annotation.PostExchange;
import org.springframework.web.service.annotation.PutExchange;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_server.dtos.restaurant.MenuRead;
import restaurant.jpa_server.dtos.restaurant.MenuWrite;
import restaurant.jpa_server.dtos.restaurant.PlatItem;
import restaurant.jpa_server.dtos.restaurant.PlatRead;
import restaurant.jpa_server.dtos.restaurant.PlatWrite;
import restaurant.jpa_server.dtos.restaurant.PromotionRead;
import restaurant.jpa_server.dtos.restaurant.PromotionWrite;
import restaurant.jpa_server.entities.restaurant.CategoriePlat;
import restaurant.jpa_server.enums.restaurant.CategoriePlatCode;

@HttpExchange("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface MenuClient {


	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer.
	 *
	 * @return Plat créé.
	 */
	@PostExchange("/plats")
	ResponseEntity<PlatRead> addPlat(@RequestBody @Valid PlatWrite plat);

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer.
	 *
	 * @return Menu créé avec ses plats.
	 */
	@PostExchange("/menus")
	ResponseEntity<MenuRead> createMenu(@RequestBody @Valid MenuWrite menu);

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Aucun retour.
	 */
	@DeleteExchange("/plats/{plaId}")
	ResponseEntity<Void> deletePlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetExchange("/categorie-plats")
	ResponseEntity<List<CategoriePlat>> getCategoriePlats();

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetExchange("/plats/{plaId}")
	ResponseEntity<PlatRead> getPlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible.
	 * @param restaurantId Restaurant proposant ce plat.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats.
	 */
	@GetExchange("/plats")
	ResponseEntity<List<PlatItem>> getPlats(@RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "categoriePlatCode", required = true) CategoriePlatCode categoriePlatCode);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchExchange("/plats/{plaId}")
	ResponseEntity<PlatRead> patchPlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);

	/**
	 * Met à jour partiellement une promotion.
	 * @param plaId Identifiant du plat.
	 * @param promotion Données partielles de la promotion.
	 *
	 * @return Promotion mise à jour.
	 */
	@PatchExchange("/plats/{plaId}/promotion")
	ResponseEntity<PromotionRead> patchPromotion(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PromotionWrite promotion);

	/**
	 * Recherche de plats avec critères multiples.
	 * @param nom Nom du plat.
	 * @param restaurantId Restaurant proposant ce plat.
	 * @param categoriePlatCode Catégorie du plat.
	 * @param disponible Indique si le plat est disponible.
	 *
	 * @return Plats correspondant aux critères de recherche.
	 */
	@GetExchange("/plats/search")
	ResponseEntity<List<PlatItem>> searchPlats(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "categoriePlatCode", required = true) CategoriePlatCode categoriePlatCode, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@PutExchange("/plats/{plaId}")
	ResponseEntity<PlatRead> updatePlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);
}

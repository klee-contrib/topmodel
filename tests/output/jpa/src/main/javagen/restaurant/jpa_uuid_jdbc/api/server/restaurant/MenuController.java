////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.api.server.restaurant;

import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuComplet;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PromotionRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PromotionWrite;
import restaurant.jpa_uuid_jdbc.entities.restaurant.CategoriePlat;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface MenuController {

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer.
	 *
	 * @return Plat créé.
	 */
	@PostMapping(path = "plats")
	PlatRead addPlat(@RequestBody @Valid PlatWrite plat);

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
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 */
	@DeleteMapping(path = "plats/{plaId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	void deletePlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetMapping(path = "categorie-plats")
	List<CategoriePlat> getCategoriePlats();

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetMapping(path = "plats/{plaId}")
	PlatRead getPlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible.
	 * @param restaurantId Restaurant proposant ce plat.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats.
	 */
	@GetMapping(path = "plats")
	List<PlatItem> getPlats(@RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "categoriePlatCode", required = true) String categoriePlatCode);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchMapping(path = "plats/{plaId}")
	PlatRead patchPlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);

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
	 * Recherche de plats avec critères multiples.
	 * @param nom Nom du plat.
	 * @param restaurantId Restaurant proposant ce plat.
	 * @param categoriePlatCode Catégorie du plat.
	 * @param disponible Indique si le plat est disponible.
	 *
	 * @return Plats correspondant aux critères de recherche.
	 */
	@GetMapping(path = "plats/search")
	List<PlatItem> searchPlats(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @RequestParam(value = "categoriePlatCode", required = true) String categoriePlatCode, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@PutMapping(path = "plats/{plaId}")
	PlatRead updatePlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);
}

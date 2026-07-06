////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.api.server.restaurant;

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

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

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
	@Operation(description = "Ajoute un plat")
	PlatRead addPlat(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à créer") @RequestBody @Valid PlatWrite plat);

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer.
	 *
	 * @return Menu créé avec ses plats.
	 */
	@PostMapping(path = "menus")
	@PreAuthorize("isAuthenticated()")
	@Operation(description = "Crée un menu avec ses plats")
	MenuRead createMenu(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Menu à créer") @RequestBody @Valid MenuWrite menu);

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 */
	@DeleteMapping(path = "plats/{plaId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un plat")
	void deletePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetMapping(path = "categorie-plats")
	@Operation(description = "Liste toutes les catégories de plats")
	List<CategoriePlat> getCategoriePlats();

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetMapping(path = "plats/{plaId}")
	@Operation(description = "Charge le détail d'un plat")
	PlatRead getPlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId);

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible.
	 * @param restaurantId Restaurant proposant ce plat.
	 * @param categoriePlatCode Catégorie du plat.
	 *
	 * @return Liste des plats.
	 */
	@GetMapping(path = "plats")
	@Operation(description = "Liste tous les plats")
	List<PlatItem> getPlats(@Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCode", required = true) CategoriePlatCode categoriePlatCode);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchMapping(path = "plats/{plaId}")
	@Operation(description = "Met à jour partiellement un plat")
	PlatRead patchPlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles du plat") @RequestBody @Valid PlatWrite plat);

	/**
	 * Met à jour partiellement une promotion.
	 * @param plaId Identifiant du plat.
	 * @param promotion Données partielles de la promotion.
	 *
	 * @return Promotion mise à jour.
	 */
	@PreAuthorize("isAuthenticated()")
	@PatchMapping(path = "plats/{plaId}/promotion")
	@Operation(description = "Met à jour partiellement une promotion")
	PromotionRead patchPromotion(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles de la promotion") @RequestBody @Valid PromotionWrite promotion);

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
	@Operation(description = "Recherche de plats avec critères multiples")
	List<PlatItem> searchPlats(@Parameter(description = "Nom du plat") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantId", required = true) Integer restaurantId, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCode", required = true) CategoriePlatCode categoriePlatCode, @Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@PutMapping(path = "plats/{plaId}")
	@Operation(description = "Met à jour un plat")
	PlatRead updatePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à mettre à jour") @RequestBody @Valid PlatWrite plat);
}

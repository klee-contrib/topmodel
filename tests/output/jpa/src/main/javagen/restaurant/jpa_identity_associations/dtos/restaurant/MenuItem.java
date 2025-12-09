////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

/**
 * Détail d'un menu en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface MenuItem {

	/**
	 * Identifiant du menu.
	 */
	Integer getId();

	/**
	 * Nom du menu.
	 */
	String getNom();

	/**
	 * Prix du menu.
	 */
	BigDecimal getPrix();

	/**
	 * Indique si le menu est disponible.
	 */
	Boolean getDisponible();

	/**
	 * Restaurant proposant ce menu.
	 */
	Integer getRestaurantIdRestaurant();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prix value to set.
	 * @param disponible value to set.
	 * @param restaurantIdRestaurant value to set.
	 */
	void hydrate(Integer id, String nom, BigDecimal prix, Boolean disponible, Integer restaurantIdRestaurant);
}

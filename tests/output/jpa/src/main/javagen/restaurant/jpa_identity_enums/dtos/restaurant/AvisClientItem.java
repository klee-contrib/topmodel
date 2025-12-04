////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;

/**
 * Détail d'un avis en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface AvisClientItem {

	/**
	 * Identifiant de l'avis.
	 */
	Integer getId();

	/**
	 * Note sur 5.
	 */
	Integer getNote();

	/**
	 * Date de l'avis.
	 */
	LocalDateTime getDateAvis();

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 */
	Boolean getApprouve();

	/**
	 * Client ayant donné l'avis.
	 */
	Integer getClientIdClient();

	/**
	 * Restaurant concerné par l'avis.
	 */
	Integer getRestaurantIdRestaurant();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param note value to set.
	 * @param dateAvis value to set.
	 * @param approuve value to set.
	 * @param clientIdClient value to set.
	 * @param restaurantIdRestaurant value to set.
	 */
	void hydrate(Integer id, Integer note, LocalDateTime dateAvis, Boolean approuve, Integer clientIdClient, Integer restaurantIdRestaurant);
}

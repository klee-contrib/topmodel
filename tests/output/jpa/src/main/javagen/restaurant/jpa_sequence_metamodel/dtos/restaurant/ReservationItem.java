////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.dtos.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;

/**
 * Détail d'une réservation en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ReservationItem {

	/**
	 * Identifiant de la réservation.
	 */
	Integer getId();

	/**
	 * Date et heure de la réservation.
	 */
	LocalDateTime getDateReservation();

	/**
	 * Nombre de personnes.
	 */
	Integer getNombrePersonnes();

	/**
	 * Indique si la réservation est confirmée.
	 */
	Boolean getConfirmee();

	/**
	 * Client ayant fait la réservation.
	 */
	Integer getClientIdClient();

	/**
	 * Restaurant concerné par la réservation.
	 */
	Integer getRestaurantIdRestaurant();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param dateReservation value to set.
	 * @param nombrePersonnes value to set.
	 * @param confirmee value to set.
	 * @param clientIdClient value to set.
	 * @param restaurantIdRestaurant value to set.
	 */
	void hydrate(Integer id, LocalDateTime dateReservation, Integer nombrePersonnes, Boolean confirmee, Integer clientIdClient, Integer restaurantIdRestaurant);
}

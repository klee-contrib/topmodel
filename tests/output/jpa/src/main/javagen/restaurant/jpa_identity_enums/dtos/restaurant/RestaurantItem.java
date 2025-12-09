////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import jakarta.annotation.Generated;

/**
 * Détail d'un restaurant en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface RestaurantItem {

	/**
	 * Identifiant du restaurant.
	 */
	Integer getId();

	/**
	 * Nom du restaurant.
	 */
	String getNom();

	/**
	 * Adresse du restaurant.
	 */
	String getAdresse();

	/**
	 * Numéro de téléphone.
	 */
	String getTelephone();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param adresse value to set.
	 * @param telephone value to set.
	 */
	void hydrate(Integer id, String nom, String adresse, String telephone);
}

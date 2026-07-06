////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

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
	 * Identifiant du restaurant.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Nom du restaurant.
	 * @param nom value to set.
	 */
	void setNom(String nom);

	/**
	 * Adresse du restaurant.
	 * @param adresse value to set.
	 */
	void setAdresse(String adresse);

	/**
	 * Numéro de téléphone.
	 * @param telephone value to set.
	 */
	void setTelephone(String telephone);
}

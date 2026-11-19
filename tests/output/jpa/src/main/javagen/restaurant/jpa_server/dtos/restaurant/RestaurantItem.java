////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

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
	 * @param id Valeur à définir.
	 */
	void setId(Integer id);

	/**
	 * Nom du restaurant.
	 * @param nom Valeur à définir.
	 */
	void setNom(String nom);

	/**
	 * Adresse du restaurant.
	 * @param adresse Valeur à définir.
	 */
	void setAdresse(String adresse);

	/**
	 * Numéro de téléphone.
	 * @param telephone Valeur à définir.
	 */
	void setTelephone(String telephone);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import jakarta.annotation.Generated;

/**
 * Détail d'un employé en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EmployeItem {

	/**
	 * Matricule de l'employé.
	 */
	String getMatricule();

	/**
	 * Restaurant où travaille l'employé.
	 */
	Integer getRestaurantIdRestaurant();

	/**
	 * Identifiant de l'employé.
	 */
	Integer getId();

	/**
	 * Nom de l'employé.
	 */
	String getNom();

	/**
	 * Prénom de l'employé.
	 */
	String getPrenom();

	/**
	 * Hydrate values of instance.
	 * @param matricule value to set.
	 * @param restaurantIdRestaurant value to set.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prenom value to set.
	 */
	void hydrate(String matricule, Integer restaurantIdRestaurant, Integer id, String nom, String prenom);
}

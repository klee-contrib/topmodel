////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import jakarta.annotation.Generated;

/**
 * Détail d'un employé en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EmployeItem {

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
	 * Matricule de l'employé.
	 */
	String getMatricule();

	/**
	 * Restaurant où travaille l'employé.
	 */
	Integer getRestaurantId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prenom value to set.
	 * @param matricule value to set.
	 * @param restaurantId value to set.
	 */
	void hydrate(Integer id, String nom, String prenom, String matricule, Integer restaurantId);
}

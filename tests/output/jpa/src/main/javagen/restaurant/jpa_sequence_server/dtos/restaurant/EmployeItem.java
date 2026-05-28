////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.dtos.restaurant;

import java.util.List;

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
	 * Liste des autres employés.
	 */
	List<EmployeItem> getAutresEmployes();

	/**
	 * Identifiant de l'employé.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Nom de l'employé.
	 * @param nom value to set.
	 */
	void setNom(String nom);

	/**
	 * Prénom de l'employé.
	 * @param prenom value to set.
	 */
	void setPrenom(String prenom);

	/**
	 * Matricule de l'employé.
	 * @param matricule value to set.
	 */
	void setMatricule(String matricule);

	/**
	 * Restaurant où travaille l'employé.
	 * @param restaurantId value to set.
	 */
	void setRestaurantId(Integer restaurantId);

	/**
	 * Liste des autres employés.
	 * @param autresEmployes value to set.
	 */
	void setAutresEmployes(List<EmployeItem> autresEmployes);
}

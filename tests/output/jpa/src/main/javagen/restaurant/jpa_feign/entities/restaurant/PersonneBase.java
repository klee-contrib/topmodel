////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import jakarta.annotation.Generated;

/**
 * Définition d'une personne.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PersonneBase {

	/**
	 * Identifiant de la personne.
	 */
	Integer getId();

	/**
	 * Nom de la personne.
	 */
	String getNom();

	/**
	 * Prénom de la personne.
	 */
	String getPrenom();

	/**
	 * Identifiant de la personne.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Nom de la personne.
	 * @param nom value to set.
	 */
	void setNom(String nom);

	/**
	 * Prénom de la personne.
	 * @param prenom value to set.
	 */
	void setPrenom(String prenom);
}

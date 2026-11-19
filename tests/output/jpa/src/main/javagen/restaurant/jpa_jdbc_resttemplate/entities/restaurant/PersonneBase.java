////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

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
	 * @param id Valeur à définir.
	 */
	void setId(Integer id);

	/**
	 * Nom de la personne.
	 * @param nom Valeur à définir.
	 */
	void setNom(String nom);

	/**
	 * Prénom de la personne.
	 * @param prenom Valeur à définir.
	 */
	void setPrenom(String prenom);
}

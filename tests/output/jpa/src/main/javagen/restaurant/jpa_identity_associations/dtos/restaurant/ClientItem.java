////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import jakarta.annotation.Generated;

/**
 * Détail d'un client en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ClientItem {

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
	 * Adresse email du client.
	 */
	String getEmail();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prenom value to set.
	 * @param email value to set.
	 */
	void hydrate(Integer id, String nom, String prenom, String email);
}

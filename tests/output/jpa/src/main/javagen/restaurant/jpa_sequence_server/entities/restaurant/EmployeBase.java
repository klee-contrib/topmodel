////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;

/**
 * Définition d'une personne.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EmployeBase extends PersonneBase {

	/**
	 * Numéro de téléphone de l'employé.
	 */
	String getTelephone();

	/**
	 * Numéro de téléphone de l'employé.
	 * @param telephone value to set.
	 */
	void setTelephone(String telephone);
}

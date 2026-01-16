////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import jakarta.annotation.Generated;

/**
 * Détail d'une table en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface TableItem {

	/**
	 * Identifiant de la table.
	 */
	Integer getId();

	/**
	 * Numéro de la table.
	 */
	String getNumero();

	/**
	 * Capacité de la table (nombre de places).
	 */
	Integer getCapacite();

	/**
	 * Indique si la table est disponible.
	 */
	Boolean getDisponible();

	/**
	 * Restaurant auquel appartient la table.
	 */
	Integer getRestaurantId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param numero value to set.
	 * @param capacite value to set.
	 * @param disponible value to set.
	 * @param restaurantId value to set.
	 */
	void hydrate(Integer id, String numero, Integer capacite, Boolean disponible, Integer restaurantId);
}

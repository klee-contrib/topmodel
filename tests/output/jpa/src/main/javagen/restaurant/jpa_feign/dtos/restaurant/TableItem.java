////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

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
	 * Identifiant de la table.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Numéro de la table.
	 * @param numero value to set.
	 */
	void setNumero(String numero);

	/**
	 * Capacité de la table (nombre de places).
	 * @param capacite value to set.
	 */
	void setCapacite(Integer capacite);

	/**
	 * Indique si la table est disponible.
	 * @param disponible value to set.
	 */
	void setDisponible(Boolean disponible);

	/**
	 * Restaurant auquel appartient la table.
	 * @param restaurantId value to set.
	 */
	void setRestaurantId(Integer restaurantId);
}

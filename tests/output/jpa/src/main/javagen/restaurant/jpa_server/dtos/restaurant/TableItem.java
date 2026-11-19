////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

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
	 * @param id Valeur à définir.
	 */
	void setId(Integer id);

	/**
	 * Numéro de la table.
	 * @param numero Valeur à définir.
	 */
	void setNumero(String numero);

	/**
	 * Capacité de la table (nombre de places).
	 * @param capacite Valeur à définir.
	 */
	void setCapacite(Integer capacite);

	/**
	 * Indique si la table est disponible.
	 * @param disponible Valeur à définir.
	 */
	void setDisponible(Boolean disponible);

	/**
	 * Restaurant auquel appartient la table.
	 * @param restaurantId Valeur à définir.
	 */
	void setRestaurantId(Integer restaurantId);
}

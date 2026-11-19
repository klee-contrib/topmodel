////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une table en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Numéro de la table.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Table#getNumero() Table#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Table#getCapacite() Table#getCapacite()}
	 */
	@NotNull
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Table#getDisponible() Table#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Table#getRestaurantId() Table#getRestaurantId()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Accesseur de 'numero'.
	 *
	 * @return Valeur de {@link #numero numero}.
	 */
	public String getNumero() {
		return this.numero;
	}

	/**
	 * Accesseur de 'capacite'.
	 *
	 * @return Valeur de {@link #capacite capacite}.
	 */
	public Integer getCapacite() {
		return this.capacite;
	}

	/**
	 * Accesseur de 'disponible'.
	 *
	 * @return Valeur de {@link #disponible disponible}.
	 */
	public Boolean getDisponible() {
		return this.disponible;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Définit la valeur de {@link #numero numero}.
	 * @param numero Valeur à définir.
	 */
	public void setNumero(String numero) {
		this.numero = numero;
	}

	/**
	 * Définit la valeur de {@link #capacite capacite}.
	 * @param capacite Valeur à définir.
	 */
	public void setCapacite(Integer capacite) {
		this.capacite = capacite;
	}

	/**
	 * Définit la valeur de {@link #disponible disponible}.
	 * @param disponible Valeur à définir.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}
}

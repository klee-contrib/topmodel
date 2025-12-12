////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une table en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Numéro de la table.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getNumero() Table#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("tab_numero")
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getCapacite() Table#getCapacite()}
	 */
	@NotNull
	@Column("tab_capacite")
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getDisponible() Table#getDisponible()}
	 */
	@NotNull
	@Column("tab_disponible")
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getRestaurantId() Table#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

	/**
	 * Association réciproque de Commande.TableId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getCommandes() Table#getCommandes()}
	 */
	@Column("com_id")
	private List<Integer> commandes;

	/**
	 * Association réciproque de Reservation.TableId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Table#getReservations() Table#getReservations()}
	 */
	@Column("rev_id")
	private List<Integer> reservations;

	/**
	 * Getter for numero.
	 *
	 * @return value of {@link #numero numero}.
	 */
	public String getNumero() {
		return this.numero;
	}

	/**
	 * Getter for capacite.
	 *
	 * @return value of {@link #capacite capacite}.
	 */
	public Integer getCapacite() {
		return this.capacite;
	}

	/**
	 * Getter for disponible.
	 *
	 * @return value of {@link #disponible disponible}.
	 */
	public Boolean getDisponible() {
		return this.disponible;
	}

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Getter for commandes.
	 *
	 * @return value of {@link #commandes commandes}.
	 */
	public List<Integer> getCommandes() {
		return this.commandes;
	}

	/**
	 * Getter for reservations.
	 *
	 * @return value of {@link #reservations reservations}.
	 */
	public List<Integer> getReservations() {
		return this.reservations;
	}

	/**
	 * Set the value of {@link #numero numero}.
	 * @param numero value to set.
	 */
	public void setNumero(String numero) {
		this.numero = numero;
	}

	/**
	 * Set the value of {@link #capacite capacite}.
	 * @param capacite value to set.
	 */
	public void setCapacite(Integer capacite) {
		this.capacite = capacite;
	}

	/**
	 * Set the value of {@link #disponible disponible}.
	 * @param disponible value to set.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Set the value of {@link #commandes commandes}.
	 * @param commandes value to set.
	 */
	public void setCommandes(List<Integer> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #reservations reservations}.
	 * @param reservations value to set.
	 */
	public void setReservations(List<Integer> reservations) {
		this.reservations = reservations;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.TableWrite TableWrite}.
	 */
	public enum Fields {
		NUMERO(String.class),
		CAPACITE(Integer.class),
		DISPONIBLE(Boolean.class),
		RESTAURANT_ID(Integer.class),
		COMMANDES(List.class),
		RESERVATIONS(List.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

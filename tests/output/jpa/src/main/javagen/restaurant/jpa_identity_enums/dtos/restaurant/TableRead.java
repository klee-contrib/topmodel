////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_enums.entities.restaurant.RestaurantMappers;
import restaurant.jpa_identity_enums.entities.restaurant.Table;

/**
 * Détail d'une table en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getId() Table#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Numéro de la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getNumero() Table#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getCapacite() Table#getCapacite()}
	 */
	@NotNull
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getDisponible() Table#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getRestaurant() Table#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Association réciproque de Commande.TableId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getCommandes() Table#getCommandes()}
	 */
	private List<Integer> commandes;

	/**
	 * Association réciproque de Reservation.TableId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Table#getReservations() Table#getReservations()}
	 */
	private List<Integer> reservations;

	/**
	 * No arg constructor.
	 */
	public TableRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'TableRead'.
	 * @param table Instance de 'Table'.
	 *
	 * @return Une nouvelle instance de 'TableRead'.
	 */
	public TableRead(Table table) {
		RestaurantMappers.mapTableRead(table, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.TableRead TableRead}.
	 */
	public enum Fields {
		ID(Integer.class),
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

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
import restaurant.jpa_identity_enums.entities.restaurant.TableClient;

/**
 * Détail d'une table en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableClientRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getId() TableClient#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Numéro de la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getNumero() TableClient#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getCapacite() TableClient#getCapacite()}
	 */
	@NotNull
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getDisponible() TableClient#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getRestaurantRestaurant() TableClient#getRestaurantRestaurant()}
	 */
	@NotNull
	private Integer restaurantIdRestaurant;

	/**
	 * Association réciproque de Commande.TableClientId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getCommandes() TableClient#getCommandes()}
	 */
	private List<Integer> commandes;

	/**
	 * Association réciproque de Reservation.TableClientIdTable.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getReservationsTable() TableClient#getReservationsTable()}
	 */
	private List<Integer> reservationsTable;

	/**
	 * No arg constructor.
	 */
	public TableClientRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'TableClientRead'.
	 * @param tableClient Instance de 'TableClient'.
	 *
	 * @return Une nouvelle instance de 'TableClientRead'.
	 */
	public TableClientRead(TableClient tableClient) {
		RestaurantMappers.mapTableClientRead(tableClient, this);
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
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
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
	 * Getter for reservationsTable.
	 *
	 * @return value of {@link #reservationsTable reservationsTable}.
	 */
	public List<Integer> getReservationsTable() {
		return this.reservationsTable;
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
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #commandes commandes}.
	 * @param commandes value to set.
	 */
	public void setCommandes(List<Integer> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #reservationsTable reservationsTable}.
	 * @param reservationsTable value to set.
	 */
	public void setReservationsTable(List<Integer> reservationsTable) {
		this.reservationsTable = reservationsTable;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.TableClientRead TableClientRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NUMERO(String.class),
		CAPACITE(Integer.class),
		DISPONIBLE(Boolean.class),
		RESTAURANT_ID_RESTAURANT(Integer.class),
		COMMANDES(List.class),
		RESERVATIONS_TABLE(List.class);

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

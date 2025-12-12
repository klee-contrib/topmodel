////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Table du restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "TABLE", uniqueConstraints = {@UniqueConstraint(columnNames = {"RES_ID", "TAB_NUMERO"})})
public class Table {

	/**
	 * Identifiant de la table.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "TAB_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Numéro de la table.
	 */
	@Column(name = "TAB_NUMERO", nullable = false, length = 10, columnDefinition = "varchar")
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 */
	@Column(name = "TAB_CAPACITE", nullable = false, columnDefinition = "int")
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 */
	@Column(name = "TAB_DISPONIBLE", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 */
	@JoinColumn(name = "RES_ID", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Association réciproque de Commande.TableId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "table")
	private List<Commande> commandes;

	/**
	 * Association réciproque de Reservation.TableId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "table")
	private List<Reservation> reservations;

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
	 * Getter for restaurant.
	 *
	 * @return value of {@link #restaurant restaurant}.
	 */
	public Restaurant getRestaurant() {
		return this.restaurant;
	}

	/**
	 * Getter for commandes.
	 *
	 * @return value of {@link #commandes commandes}.
	 */
	public List<Commande> getCommandes() {
		if (this.commandes == null) {
			this.commandes = new ArrayList<>();
		}
		return this.commandes;
	}

	/**
	 * Getter for reservations.
	 *
	 * @return value of {@link #reservations reservations}.
	 */
	public List<Reservation> getReservations() {
		if (this.reservations == null) {
			this.reservations = new ArrayList<>();
		}
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
	 * Set the value of {@link #restaurant restaurant}.
	 * @param restaurant value to set.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Set the value of {@link #commandes commandes}.
	 * @param commandes value to set.
	 */
	public void setCommandes(List<Commande> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #reservations reservations}.
	 * @param reservations value to set.
	 */
	public void setReservations(List<Reservation> reservations) {
		this.reservations = reservations;
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Table#commandes commandes}.
	 * @param commande value to add to table.
	 */
	void addCommande(Commande commande) {
		this.commandes.add(commande);
		commande.setTable(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Table#reservations reservations}.
	 * @param reservation value to add to table.
	 */
	void addReservation(Reservation reservation) {
		this.reservations.add(reservation);
		reservation.setTable(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Table#commandes commandes}.
	 * @param commande commande value to remove.
	 */
	void removeCommande(Commande commande) {
		this.commandes.remove(commande);
		commande.setTable(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Table#reservations reservations}.
	 * @param reservation reservation value to remove.
	 */
	void removeReservation(Reservation reservation) {
		this.reservations.remove(reservation);
		reservation.setTable(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.entities.restaurant.Table Table}.
	 */
	public enum Fields {
		ID(Integer.class),
		NUMERO(String.class),
		CAPACITE(Integer.class),
		DISPONIBLE(Boolean.class),
		RESTAURANT(Restaurant.class),
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

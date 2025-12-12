////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Réservation d'une table.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "RESERVATION", uniqueConstraints = {@UniqueConstraint(columnNames = {"TAB_ID", "REV_DATE_RESERVATION"})})
public class Reservation {

	/**
	 * Identifiant de la réservation.
	 */
	@Id
	@Column(name = "REV_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_RESERVATION")
	@SequenceGenerator(sequenceName = "SEQ_RESERVATION", name = "SEQ_RESERVATION", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 */
	@Column(name = "REV_DATE_RESERVATION", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 */
	@Column(name = "REV_NOMBRE_PERSONNES", nullable = false, columnDefinition = "int")
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 */
	@Column(name = "REV_COMMENTAIRE", length = 100, columnDefinition = "varchar")
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 */
	@Column(name = "REV_CONFIRMEE", nullable = false, columnDefinition = "boolean")
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 */
	@JoinColumn(name = "PER_ID", referencedColumnName = "PER_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Table réservée.
	 */
	@JoinColumn(name = "TAB_ID", referencedColumnName = "TAB_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Table.class)
	private Table table;

	/**
	 * Restaurant concerné par la réservation.
	 */
	@JoinColumn(name = "RES_ID", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for dateReservation.
	 *
	 * @return value of {@link #dateReservation dateReservation}.
	 */
	public LocalDateTime getDateReservation() {
		return this.dateReservation;
	}

	/**
	 * Getter for nombrePersonnes.
	 *
	 * @return value of {@link #nombrePersonnes nombrePersonnes}.
	 */
	public Integer getNombrePersonnes() {
		return this.nombrePersonnes;
	}

	/**
	 * Getter for commentaire.
	 *
	 * @return value of {@link #commentaire commentaire}.
	 */
	public String getCommentaire() {
		return this.commentaire;
	}

	/**
	 * Getter for confirmee.
	 *
	 * @return value of {@link #confirmee confirmee}.
	 */
	public Boolean getConfirmee() {
		return this.confirmee;
	}

	/**
	 * Getter for client.
	 *
	 * @return value of {@link #client client}.
	 */
	public Client getClient() {
		return this.client;
	}

	/**
	 * Getter for table.
	 *
	 * @return value of {@link #table table}.
	 */
	public Table getTable() {
		return this.table;
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
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #dateReservation dateReservation}.
	 * @param dateReservation value to set.
	 */
	public void setDateReservation(LocalDateTime dateReservation) {
		this.dateReservation = dateReservation;
	}

	/**
	 * Set the value of {@link #nombrePersonnes nombrePersonnes}.
	 * @param nombrePersonnes value to set.
	 */
	public void setNombrePersonnes(Integer nombrePersonnes) {
		this.nombrePersonnes = nombrePersonnes;
	}

	/**
	 * Set the value of {@link #commentaire commentaire}.
	 * @param commentaire value to set.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Set the value of {@link #confirmee confirmee}.
	 * @param confirmee value to set.
	 */
	public void setConfirmee(Boolean confirmee) {
		this.confirmee = confirmee;
	}

	/**
	 * Set the value of {@link #client client}.
	 * @param client value to set.
	 */
	public void setClient(Client client) {
		this.client = client;
	}

	/**
	 * Set the value of {@link #table table}.
	 * @param table value to set.
	 */
	public void setTable(Table table) {
		this.table = table;
	}

	/**
	 * Set the value of {@link #restaurant restaurant}.
	 * @param restaurant value to set.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Reservation Reservation}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_RESERVATION(LocalDateTime.class),
		NOMBRE_PERSONNES(Integer.class),
		COMMENTAIRE(String.class),
		CONFIRMEE(Boolean.class),
		CLIENT(Client.class),
		TABLE(Table.class),
		RESTAURANT(Restaurant.class);

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

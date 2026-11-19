////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.ReadOnlyProperty;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
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
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "reservation",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"tab_id", "rev_date_reservation"})
	}
)
public class Reservation {

	/**
	 * Identifiant de la réservation.
	 */
	@Id
	@ReadOnlyProperty
	@Column(name = "rev_id", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_RESERVATION")
	@SequenceGenerator(sequenceName = "SEQ_RESERVATION", name = "SEQ_RESERVATION", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 */
	@Column(name = "rev_date_reservation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 */
	@Column(name = "rev_nombre_personnes", nullable = false, columnDefinition = "int")
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 */
	@Column(name = "rev_commentaire", length = 100, columnDefinition = "varchar")
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 */
	@Column(name = "rev_confirmee", nullable = false, columnDefinition = "boolean")
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 */
	@JoinColumn(name = "per_id", referencedColumnName = "per_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Table réservée.
	 */
	@Column(name = "tab_id", columnDefinition = "int")
	private Integer tableId;

	/**
	 * Restaurant concerné par la réservation.
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "rev_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'dateReservation'.
	 *
	 * @return Valeur de {@link #dateReservation dateReservation}.
	 */
	public LocalDateTime getDateReservation() {
		return this.dateReservation;
	}

	/**
	 * Accesseur de 'nombrePersonnes'.
	 *
	 * @return Valeur de {@link #nombrePersonnes nombrePersonnes}.
	 */
	public Integer getNombrePersonnes() {
		return this.nombrePersonnes;
	}

	/**
	 * Accesseur de 'commentaire'.
	 *
	 * @return Valeur de {@link #commentaire commentaire}.
	 */
	public String getCommentaire() {
		return this.commentaire;
	}

	/**
	 * Accesseur de 'confirmee'.
	 *
	 * @return Valeur de {@link #confirmee confirmee}.
	 */
	public Boolean getConfirmee() {
		return this.confirmee;
	}

	/**
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public Client getClient() {
		return this.client;
	}

	/**
	 * Accesseur de 'tableId'.
	 *
	 * @return Valeur de {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
	}

	/**
	 * Accesseur de 'restaurant'.
	 *
	 * @return Valeur de {@link #restaurant restaurant}.
	 */
	public Restaurant getRestaurant() {
		return this.restaurant;
	}

	/**
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #dateReservation dateReservation}.
	 * @param dateReservation Valeur à définir.
	 */
	public void setDateReservation(LocalDateTime dateReservation) {
		this.dateReservation = dateReservation;
	}

	/**
	 * Définit la valeur de {@link #nombrePersonnes nombrePersonnes}.
	 * @param nombrePersonnes Valeur à définir.
	 */
	public void setNombrePersonnes(Integer nombrePersonnes) {
		this.nombrePersonnes = nombrePersonnes;
	}

	/**
	 * Définit la valeur de {@link #commentaire commentaire}.
	 * @param commentaire Valeur à définir.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Définit la valeur de {@link #confirmee confirmee}.
	 * @param confirmee Valeur à définir.
	 */
	public void setConfirmee(Boolean confirmee) {
		this.confirmee = confirmee;
	}

	/**
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(Client client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Reservation Reservation}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_RESERVATION(LocalDateTime.class),
		NOMBRE_PERSONNES(Integer.class),
		COMMENTAIRE(String.class),
		CONFIRMEE(Boolean.class),
		CLIENT(Client.class),
		TABLE_ID(Integer.class),
		RESTAURANT(Restaurant.class),
		DATE_CREATION(LocalDateTime.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
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
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

import topmodel.test.NombreVuesBase;

/**
 * Avis d'un client sur un restaurant.
 */
@Entity
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "avis_client",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"per_id", "lie_id", "avi_date_avis"})
	}
)
public class AvisClient extends NombreVuesBase {

	/**
	 * Identifiant de l'avis.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "avi_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Note sur 5.
	 */
	@Column(name = "avi_note", nullable = false, columnDefinition = "int")
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 */
	@Column(name = "avi_commentaire", length = 100, columnDefinition = "varchar")
	private String commentaire;

	/**
	 * Date de l'avis.
	 */
	@Column(name = "avi_date_avis", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateAvis;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 */
	@Column(name = "avi_approuve", nullable = false, columnDefinition = "boolean")
	private Boolean approuve = false;

	/**
	 * Client ayant donné l'avis.
	 */
	@JoinColumn(name = "per_id", referencedColumnName = "per_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Restaurant concerné par l'avis.
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "avi_date_creation", nullable = false, columnDefinition = "timestamp")
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
	 * Accesseur de 'note'.
	 *
	 * @return Valeur de {@link #note note}.
	 */
	public Integer getNote() {
		return this.note;
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
	 * Accesseur de 'dateAvis'.
	 *
	 * @return Valeur de {@link #dateAvis dateAvis}.
	 */
	public LocalDateTime getDateAvis() {
		return this.dateAvis;
	}

	/**
	 * Accesseur de 'approuve'.
	 *
	 * @return Valeur de {@link #approuve approuve}.
	 */
	public Boolean getApprouve() {
		return this.approuve;
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
	 * Définit la valeur de {@link #note note}.
	 * @param note Valeur à définir.
	 */
	public void setNote(Integer note) {
		this.note = note;
	}

	/**
	 * Définit la valeur de {@link #commentaire commentaire}.
	 * @param commentaire Valeur à définir.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Définit la valeur de {@link #dateAvis dateAvis}.
	 * @param dateAvis Valeur à définir.
	 */
	public void setDateAvis(LocalDateTime dateAvis) {
		this.dateAvis = dateAvis;
	}

	/**
	 * Définit la valeur de {@link #approuve approuve}.
	 * @param approuve Valeur à définir.
	 */
	public void setApprouve(Boolean approuve) {
		this.approuve = approuve;
	}

	/**
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(Client client) {
		this.client = client;
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
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.AvisClient AvisClient}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOTE(Integer.class),
		COMMENTAIRE(String.class),
		DATE_AVIS(LocalDateTime.class),
		APPROUVE(Boolean.class),
		CLIENT(Client.class),
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

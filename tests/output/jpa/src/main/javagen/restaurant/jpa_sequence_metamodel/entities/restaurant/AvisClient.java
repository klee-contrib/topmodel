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
 * Avis d'un client sur un restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "AVIS_CLIENT", uniqueConstraints = {@UniqueConstraint(columnNames = {"CLI_ID_CLIENT", "RES_ID_RESTAURANT", "AVI_DATE_AVIS"})})
public class AvisClient {

	/**
	 * Identifiant de l'avis.
	 */
	@Id
	@Column(name = "AVI_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_AVIS_CLIENT")
	@SequenceGenerator(sequenceName = "SEQ_AVIS_CLIENT", name = "SEQ_AVIS_CLIENT", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Note sur 5.
	 */
	@Column(name = "AVI_NOTE", nullable = false, columnDefinition = "int")
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 */
	@Column(name = "AVI_COMMENTAIRE", length = 100, columnDefinition = "varchar")
	private String commentaire;

	/**
	 * Date de l'avis.
	 */
	@Column(name = "AVI_DATE_AVIS", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateAvis;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 */
	@Column(name = "AVI_APPROUVE", nullable = false, columnDefinition = "boolean")
	private Boolean approuve = false;

	/**
	 * Nombre de vues de l'avis (calculé).
	 */
	@Column(name = "AVI_NOMBRE_VUES", nullable = false, columnDefinition = "int")
	private Integer nombreVues = 0;

	/**
	 * Client ayant donné l'avis.
	 */
	@JoinColumn(name = "CLI_ID_CLIENT", referencedColumnName = "CLI_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client clientClient;

	/**
	 * Restaurant concerné par l'avis.
	 */
	@JoinColumn(name = "RES_ID_RESTAURANT", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurantRestaurant;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for note.
	 *
	 * @return value of {@link #note note}.
	 */
	public Integer getNote() {
		return this.note;
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
	 * Getter for dateAvis.
	 *
	 * @return value of {@link #dateAvis dateAvis}.
	 */
	public LocalDateTime getDateAvis() {
		return this.dateAvis;
	}

	/**
	 * Getter for approuve.
	 *
	 * @return value of {@link #approuve approuve}.
	 */
	public Boolean getApprouve() {
		return this.approuve;
	}

	/**
	 * Getter for nombreVues.
	 *
	 * @return value of {@link #nombreVues nombreVues}.
	 */
	public Integer getNombreVues() {
		return this.nombreVues;
	}

	/**
	 * Getter for clientClient.
	 *
	 * @return value of {@link #clientClient clientClient}.
	 */
	public Client getClientClient() {
		return this.clientClient;
	}

	/**
	 * Getter for restaurantRestaurant.
	 *
	 * @return value of {@link #restaurantRestaurant restaurantRestaurant}.
	 */
	public Restaurant getRestaurantRestaurant() {
		return this.restaurantRestaurant;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #note note}.
	 * @param note value to set.
	 */
	public void setNote(Integer note) {
		this.note = note;
	}

	/**
	 * Set the value of {@link #commentaire commentaire}.
	 * @param commentaire value to set.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Set the value of {@link #dateAvis dateAvis}.
	 * @param dateAvis value to set.
	 */
	public void setDateAvis(LocalDateTime dateAvis) {
		this.dateAvis = dateAvis;
	}

	/**
	 * Set the value of {@link #approuve approuve}.
	 * @param approuve value to set.
	 */
	public void setApprouve(Boolean approuve) {
		this.approuve = approuve;
	}

	/**
	 * Set the value of {@link #nombreVues nombreVues}.
	 * @param nombreVues value to set.
	 */
	public void setNombreVues(Integer nombreVues) {
		this.nombreVues = nombreVues;
	}

	/**
	 * Set the value of {@link #clientClient clientClient}.
	 * @param clientClient value to set.
	 */
	public void setClientClient(Client clientClient) {
		this.clientClient = clientClient;
	}

	/**
	 * Set the value of {@link #restaurantRestaurant restaurantRestaurant}.
	 * @param restaurantRestaurant value to set.
	 */
	public void setRestaurantRestaurant(Restaurant restaurantRestaurant) {
		this.restaurantRestaurant = restaurantRestaurant;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.AvisClient AvisClient}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOTE(Integer.class),
		COMMENTAIRE(String.class),
		DATE_AVIS(LocalDateTime.class),
		APPROUVE(Boolean.class),
		NOMBRE_VUES(Integer.class),
		CLIENT_CLIENT(Client.class),
		RESTAURANT_RESTAURANT(Restaurant.class);

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

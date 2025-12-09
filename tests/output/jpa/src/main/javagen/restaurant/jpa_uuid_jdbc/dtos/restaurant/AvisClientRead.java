////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un avis en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClientRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getId() AvisClient#getId()}
	 */
	@NotNull
	@Column("avi_id")
	private Integer id;

	/**
	 * Note sur 5.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getNote() AvisClient#getNote()}
	 */
	@NotNull
	@Column("avi_note")
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getCommentaire() AvisClient#getCommentaire()}
	 */
	@Size(max = 100)
	@Column("avi_commentaire")
	private String commentaire;

	/**
	 * Date de l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getDateAvis() AvisClient#getDateAvis()}
	 */
	@NotNull
	@Column("avi_date_avis")
	private LocalDateTime dateAvis;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getApprouve() AvisClient#getApprouve()}
	 */
	@NotNull
	@Column("avi_approuve")
	private Boolean approuve = false;

	/**
	 * Nombre de vues de l'avis (calculé).
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getNombreVues() AvisClient#getNombreVues()}
	 */
	@NotNull
	@Column("avi_nombre_vues")
	private Integer nombreVues = 0;

	/**
	 * Client ayant donné l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getClientIdClient() AvisClient#getClientIdClient()}
	 */
	@NotNull
	@Column("cli_id_client")
	private Integer clientIdClient;

	/**
	 * Restaurant concerné par l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getRestaurantIdRestaurant() AvisClient#getRestaurantIdRestaurant()}
	 */
	@NotNull
	@Column("res_id_restaurant")
	private Integer restaurantIdRestaurant;

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
	 * Getter for clientIdClient.
	 *
	 * @return value of {@link #clientIdClient clientIdClient}.
	 */
	public Integer getClientIdClient() {
		return this.clientIdClient;
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
	 * Set the value of {@link #clientIdClient clientIdClient}.
	 * @param clientIdClient value to set.
	 */
	public void setClientIdClient(Integer clientIdClient) {
		this.clientIdClient = clientIdClient;
	}

	/**
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.AvisClientRead AvisClientRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOTE(Integer.class),
		COMMENTAIRE(String.class),
		DATE_AVIS(LocalDateTime.class),
		APPROUVE(Boolean.class),
		NOMBRE_VUES(Integer.class),
		CLIENT_ID_CLIENT(Integer.class),
		RESTAURANT_ID_RESTAURANT(Integer.class);

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

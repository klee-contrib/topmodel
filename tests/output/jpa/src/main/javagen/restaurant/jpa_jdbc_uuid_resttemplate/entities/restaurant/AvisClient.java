////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Avis d'un client sur un restaurant.
 */
@Table(name = "avis_client")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClient {

	/**
	 * Identifiant de l'avis.
	 */
	@Id
	@Column("avi_id")
	private Integer id;

	/**
	 * Note sur 5.
	 */
	@NotNull
	@Column("avi_note")
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 */
	@Column("avi_commentaire")
	private String commentaire;

	/**
	 * Date de l'avis.
	 */
	@NotNull
	@Column("avi_date_avis")
	private LocalDateTime dateAvis;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 */
	@NotNull
	@Column("avi_approuve")
	private Boolean approuve = false;

	/**
	 * Nombre de vues de l'avis (calculé).
	 */
	@NotNull
	@Column("avi_nombre_vues")
	private Integer nombreVues = 0;

	/**
	 * Client ayant donné l'avis.
	 */
	@NotNull
	@Column("per_id")
	private Integer clientId;

	/**
	 * Restaurant concerné par l'avis.
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

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
	 * Getter for clientId.
	 *
	 * @return value of {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
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
	 * Set the value of {@link #clientId clientId}.
	 * @param clientId value to set.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}
}

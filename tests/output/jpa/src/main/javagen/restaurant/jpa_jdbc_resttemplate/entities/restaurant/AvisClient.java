////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

import topmodel.test.NombreVuesBase;

/**
 * Avis d'un client sur un restaurant.
 */
@Table(name = "avis_client")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClient extends NombreVuesBase {

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
	 * Client ayant donné l'avis.
	 */
	@NotNull
	@Column("per_id")
	private Integer client;

	/**
	 * Restaurant concerné par l'avis.
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("avi_date_creation")
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
	public Integer getClient() {
		return this.client;
	}

	/**
	 * Accesseur de 'restaurant'.
	 *
	 * @return Valeur de {@link #restaurant restaurant}.
	 */
	public Integer getRestaurant() {
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
	public void setClient(Integer client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Integer restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

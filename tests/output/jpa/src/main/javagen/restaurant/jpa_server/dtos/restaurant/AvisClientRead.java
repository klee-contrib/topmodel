////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un avis en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClientRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de l'avis.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getId() AvisClient#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Note sur 5.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getNote() AvisClient#getNote()}
	 */
	@NotNull
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getCommentaire() AvisClient#getCommentaire()}
	 */
	@Size(max = 100)
	private String commentaire;

	/**
	 * Date de l'avis.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getDateAvis() AvisClient#getDateAvis()}
	 */
	@NotNull
	private LocalDateTime dateAvis;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getApprouve() AvisClient#getApprouve()}
	 */
	@NotNull
	private Boolean approuve = false;

	/**
	 * Client ayant donné l'avis.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getClient() AvisClient#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Restaurant concerné par l'avis.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getRestaurant() AvisClient#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getDateCreation() AvisClient#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Nombre de vues de l'avis (calculé).
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.AvisClient#getNombreVues() AvisClient#getNombreVues()}
	 */
	@NotNull
	private Integer nombreVues = 0;

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
	 * Accesseur de 'clientId'.
	 *
	 * @return Valeur de {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Accesseur de 'nombreVues'.
	 *
	 * @return Valeur de {@link #nombreVues nombreVues}.
	 */
	public Integer getNombreVues() {
		return this.nombreVues;
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
	 * Définit la valeur de {@link #clientId clientId}.
	 * @param clientId Valeur à définir.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #nombreVues nombreVues}.
	 * @param nombreVues Valeur à définir.
	 */
	public void setNombreVues(Integer nombreVues) {
		this.nombreVues = nombreVues;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un avis en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClientWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Note sur 5.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.AvisClient#getNote() AvisClient#getNote()}
	 */
	@NotNull
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.AvisClient#getCommentaire() AvisClient#getCommentaire()}
	 */
	@Size(max = 100)
	private String commentaire;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.AvisClient#getApprouve() AvisClient#getApprouve()}
	 */
	@NotNull
	private Boolean approuve = false;

	/**
	 * Client ayant donné l'avis.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.AvisClient#getClient() AvisClient#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Restaurant concerné par l'avis.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.AvisClient#getRestaurant() AvisClient#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

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
	 * Getter for approuve.
	 *
	 * @return value of {@link #approuve approuve}.
	 */
	public Boolean getApprouve() {
		return this.approuve;
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
	 * Set the value of {@link #approuve approuve}.
	 * @param approuve value to set.
	 */
	public void setApprouve(Boolean approuve) {
		this.approuve = approuve;
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

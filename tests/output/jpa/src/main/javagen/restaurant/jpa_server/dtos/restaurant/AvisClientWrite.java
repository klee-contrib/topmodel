////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

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
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

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
}

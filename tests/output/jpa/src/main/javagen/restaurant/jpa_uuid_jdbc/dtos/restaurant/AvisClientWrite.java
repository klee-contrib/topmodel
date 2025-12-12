////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

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
	 * Indique si l'avis est approuvé par le restaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getApprouve() AvisClient#getApprouve()}
	 */
	@NotNull
	@Column("avi_approuve")
	private Boolean approuve = false;

	/**
	 * Client ayant donné l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getClientId() AvisClient#getClientId()}
	 */
	@NotNull
	@Column("per_id")
	private Integer clientId;

	/**
	 * Restaurant concerné par l'avis.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.AvisClient#getRestaurantId() AvisClient#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.AvisClientWrite AvisClientWrite}.
	 */
	public enum Fields {
		NOTE(Integer.class),
		COMMENTAIRE(String.class),
		APPROUVE(Boolean.class),
		CLIENT_ID(Integer.class),
		RESTAURANT_ID(Integer.class);

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

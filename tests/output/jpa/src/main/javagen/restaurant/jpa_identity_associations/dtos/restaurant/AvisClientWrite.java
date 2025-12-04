////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_associations.entities.restaurant.AvisClient;
import restaurant.jpa_identity_associations.entities.restaurant.RestaurantMappers;

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
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.AvisClient#getNote() AvisClient#getNote()}
	 */
	@NotNull
	private Integer note;

	/**
	 * Commentaire de l'avis.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.AvisClient#getCommentaire() AvisClient#getCommentaire()}
	 */
	@Size(max = 100)
	private String commentaire;

	/**
	 * Indique si l'avis est approuvé par le restaurant.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.AvisClient#getApprouve() AvisClient#getApprouve()}
	 */
	@NotNull
	private Boolean approuve = false;

	/**
	 * Client ayant donné l'avis.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.AvisClient#getClientClient() AvisClient#getClientClient()}
	 */
	@NotNull
	private Integer clientIdClient;

	/**
	 * Restaurant concerné par l'avis.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.AvisClient#getRestaurantRestaurant() AvisClient#getRestaurantRestaurant()}
	 */
	@NotNull
	private Integer restaurantIdRestaurant;

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
	 * Mappe 'AvisClientWrite' vers 'AvisClient'.
	 * @param target Instance pré-existante de 'AvisClient'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'AvisClient'.
	 */
	public AvisClient toAvisClient(AvisClient target) {
		return RestaurantMappers.toAvisClient(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.dtos.restaurant.AvisClientWrite AvisClientWrite}.
	 */
	public enum Fields {
		NOTE(Integer.class),
		COMMENTAIRE(String.class),
		APPROUVE(Boolean.class),
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

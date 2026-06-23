////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_feign.entities.restaurant.Promotion;
import restaurant.jpa_feign.entities.restaurant.RestaurantMappers;

/**
 * Détail d'une promotion en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Libellé de la promotion.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getLibelle() Promotion#getLibelle()}
	 */
	@NotNull
	@Size(max = 100)
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getPourcentageReduction() Promotion#getPourcentageReduction()}
	 */
	@NotNull
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getDateDebut() Promotion#getDateDebut()}
	 */
	@NotNull
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getDateFin() Promotion#getDateFin()}
	 */
	@NotNull
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getActive() Promotion#getActive()}
	 */
	@NotNull
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Promotion#getRestaurant() Promotion#getRestaurant()}
	 */
	private Integer restaurantId;

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for pourcentageReduction.
	 *
	 * @return value of {@link #pourcentageReduction pourcentageReduction}.
	 */
	public Integer getPourcentageReduction() {
		return this.pourcentageReduction;
	}

	/**
	 * Getter for dateDebut.
	 *
	 * @return value of {@link #dateDebut dateDebut}.
	 */
	public LocalDateTime getDateDebut() {
		return this.dateDebut;
	}

	/**
	 * Getter for dateFin.
	 *
	 * @return value of {@link #dateFin dateFin}.
	 */
	public LocalDateTime getDateFin() {
		return this.dateFin;
	}

	/**
	 * Getter for active.
	 *
	 * @return value of {@link #active active}.
	 */
	public Boolean getActive() {
		return this.active;
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
	 * Set the value of {@link #libelle libelle}.
	 * @param libelle value to set.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link #pourcentageReduction pourcentageReduction}.
	 * @param pourcentageReduction value to set.
	 */
	public void setPourcentageReduction(Integer pourcentageReduction) {
		this.pourcentageReduction = pourcentageReduction;
	}

	/**
	 * Set the value of {@link #dateDebut dateDebut}.
	 * @param dateDebut value to set.
	 */
	public void setDateDebut(LocalDateTime dateDebut) {
		this.dateDebut = dateDebut;
	}

	/**
	 * Set the value of {@link #dateFin dateFin}.
	 * @param dateFin value to set.
	 */
	public void setDateFin(LocalDateTime dateFin) {
		this.dateFin = dateFin;
	}

	/**
	 * Set the value of {@link #active active}.
	 * @param active value to set.
	 */
	public void setActive(Boolean active) {
		this.active = active;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Mappe 'PromotionWrite' vers 'Promotion'.
	 * @param target Instance pré-existante de 'Promotion'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Promotion'.
	 */
	public Promotion toPromotion(Promotion target) {
		return RestaurantMappers.toPromotion(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.PromotionWrite PromotionWrite}.
	 */
	public enum Fields {
		LIBELLE(String.class),
		POURCENTAGE_REDUCTION(Integer.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		ACTIVE(Boolean.class),
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

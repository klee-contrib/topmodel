////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_enums.entities.restaurant.Promotion;
import restaurant.jpa_identity_enums.entities.restaurant.RestaurantMappers;

/**
 * Détail d'une promotion en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la promotion.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getId() Promotion#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Libellé de la promotion.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getLibelle() Promotion#getLibelle()}
	 */
	@NotNull
	@Size(max = 100)
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getPourcentageReduction() Promotion#getPourcentageReduction()}
	 */
	@NotNull
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getDateDebut() Promotion#getDateDebut()}
	 */
	@NotNull
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getDateFin() Promotion#getDateFin()}
	 */
	@NotNull
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getActive() Promotion#getActive()}
	 */
	@NotNull
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getRestaurantRestaurant() Promotion#getRestaurantRestaurant()}
	 */
	private Integer restaurantIdRestaurant;

	/**
	 * Association réciproque de PromotionPlat.PromotionIdPromotion.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Promotion#getPromotionPlatsPromotion() Promotion#getPromotionPlatsPromotion()}
	 */
	@NotNull
	private List<Integer> promotionPlatsPromotion;

	/**
	 * No arg constructor.
	 */
	public PromotionRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'PromotionRead'.
	 * @param promotion Instance de 'Promotion'.
	 *
	 * @return Une nouvelle instance de 'PromotionRead'.
	 */
	public PromotionRead(Promotion promotion) {
		RestaurantMappers.mapPromotionRead(promotion, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
	}

	/**
	 * Getter for promotionPlatsPromotion.
	 *
	 * @return value of {@link #promotionPlatsPromotion promotionPlatsPromotion}.
	 */
	public List<Integer> getPromotionPlatsPromotion() {
		return this.promotionPlatsPromotion;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #promotionPlatsPromotion promotionPlatsPromotion}.
	 * @param promotionPlatsPromotion value to set.
	 */
	public void setPromotionPlatsPromotion(List<Integer> promotionPlatsPromotion) {
		this.promotionPlatsPromotion = promotionPlatsPromotion;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.PromotionRead PromotionRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		LIBELLE(String.class),
		POURCENTAGE_REDUCTION(Integer.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		ACTIVE(Boolean.class),
		RESTAURANT_ID_RESTAURANT(Integer.class),
		PROMOTION_PLATS_PROMOTION(List.class);

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

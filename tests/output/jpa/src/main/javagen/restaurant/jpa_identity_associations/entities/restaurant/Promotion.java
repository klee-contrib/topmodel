////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

/**
 * Promotion sur les plats.
 */
@Entity
@Table(name = "PROMOTION")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Promotion {

	/**
	 * Identifiant de la promotion.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "PRO_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Libellé de la promotion.
	 */
	@Column(name = "PRO_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 */
	@Column(name = "PRO_POURCENTAGE_REDUCTION", nullable = false, columnDefinition = "int")
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 */
	@Column(name = "PRO_DATE_DEBUT", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 */
	@Column(name = "PRO_DATE_FIN", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 */
	@Column(name = "PRO_ACTIVE", nullable = false, columnDefinition = "boolean")
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 */
	@JoinColumn(name = "RES_ID_RESTAURANT", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Restaurant.class)
	private Restaurant restaurantRestaurant;

	/**
	 * Association réciproque de PromotionPlat.PromotionIdPromotion.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "promotionPromotion")
	private List<PromotionPlat> promotionPlatsPromotion;

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
	 * Getter for restaurantRestaurant.
	 *
	 * @return value of {@link #restaurantRestaurant restaurantRestaurant}.
	 */
	public Restaurant getRestaurantRestaurant() {
		return this.restaurantRestaurant;
	}

	/**
	 * Getter for promotionPlatsPromotion.
	 *
	 * @return value of {@link #promotionPlatsPromotion promotionPlatsPromotion}.
	 */
	public List<PromotionPlat> getPromotionPlatsPromotion() {
		if (this.promotionPlatsPromotion == null) {
			this.promotionPlatsPromotion = new ArrayList<>();
		}
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
	 * Set the value of {@link #restaurantRestaurant restaurantRestaurant}.
	 * @param restaurantRestaurant value to set.
	 */
	public void setRestaurantRestaurant(Restaurant restaurantRestaurant) {
		this.restaurantRestaurant = restaurantRestaurant;
	}

	/**
	 * Set the value of {@link #promotionPlatsPromotion promotionPlatsPromotion}.
	 * @param promotionPlatsPromotion value to set.
	 */
	public void setPromotionPlatsPromotion(List<PromotionPlat> promotionPlatsPromotion) {
		this.promotionPlatsPromotion = promotionPlatsPromotion;
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Promotion#promotionPlatsPromotion promotionPlatsPromotion}.
	 * @param promotionPlat value to add to promotionPromotion.
	 */
	void addPromotionPlatPromotion(PromotionPlat promotionPlat) {
		this.promotionPlatsPromotion.add(promotionPlat);
		promotionPlat.setPromotionPromotion(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Promotion#promotionPlatsPromotion promotionPlatsPromotion}.
	 * @param promotionPlat promotionPlat value to remove.
	 */
	void removePromotionPlatPromotion(PromotionPlat promotionPlat) {
		this.promotionPlatsPromotion.remove(promotionPlat);
		promotionPlat.setPromotionPromotion(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.entities.restaurant.Promotion Promotion}.
	 */
	public enum Fields {
		ID(Integer.class),
		LIBELLE(String.class),
		POURCENTAGE_REDUCTION(Integer.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		ACTIVE(Boolean.class),
		RESTAURANT_RESTAURANT(Restaurant.class),
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

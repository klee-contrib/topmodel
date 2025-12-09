////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Association entre une promotion et un plat.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "PROMOTION_PLAT", uniqueConstraints = {@UniqueConstraint(columnNames = {"PRO_ID_PROMOTION", "PLA_ID_PLAT"})})
public class PromotionPlat {

	/**
	 * Identifiant de l'association.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "PPL_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Promotion concernée.
	 */
	@JoinColumn(name = "PRO_ID_PROMOTION", referencedColumnName = "PRO_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Promotion.class)
	private Promotion promotionPromotion;

	/**
	 * Plat concerné par la promotion.
	 */
	@JoinColumn(name = "PLA_ID_PLAT", referencedColumnName = "PLA_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
	private Plat platPlat;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for promotionPromotion.
	 *
	 * @return value of {@link #promotionPromotion promotionPromotion}.
	 */
	public Promotion getPromotionPromotion() {
		return this.promotionPromotion;
	}

	/**
	 * Getter for platPlat.
	 *
	 * @return value of {@link #platPlat platPlat}.
	 */
	public Plat getPlatPlat() {
		return this.platPlat;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #promotionPromotion promotionPromotion}.
	 * @param promotionPromotion value to set.
	 */
	public void setPromotionPromotion(Promotion promotionPromotion) {
		this.promotionPromotion = promotionPromotion;
	}

	/**
	 * Set the value of {@link #platPlat platPlat}.
	 * @param platPlat value to set.
	 */
	public void setPlatPlat(Plat platPlat) {
		this.platPlat = platPlat;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.entities.restaurant.PromotionPlat PromotionPlat}.
	 */
	public enum Fields {
		ID(Integer.class),
		PROMOTION_PROMOTION(Promotion.class),
		PLAT_PLAT(Plat.class);

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

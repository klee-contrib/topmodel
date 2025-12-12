////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.util.Objects;

import jakarta.annotation.Generated;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

/**
 * Association entre une promotion et un plat.
 */
@Entity
@Table(name = "PROMOTION_PLAT")
@IdClass(PromotionPlat.PromotionPlatId.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionPlat {

	/**
	 * Promotion concernée.
	 */
	@Id
	private Promotion promotion;

	/**
	 * Plat concerné par la promotion.
	 */
	@Id
	private Plat plat;

	/**
	 * Getter for promotion.
	 *
	 * @return value of {@link #promotion promotion}.
	 */
	public Promotion getPromotion() {
		return this.promotion;
	}

	/**
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
	}

	/**
	 * Set the value of {@link #promotion promotion}.
	 * @param promotion value to set.
	 */
	public void setPromotion(Promotion promotion) {
		this.promotion = promotion;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.entities.restaurant.PromotionPlat PromotionPlat}.
	 */
	public enum Fields {
		PROMOTION(Promotion.class),
		PLAT(Plat.class);

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

	public static class PromotionPlatId {

		@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Promotion.class)
		private Promotion promotion;

		@JoinColumn(name = "PLA_ID", referencedColumnName = "PLA_ID")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
		private Plat plat;

		/**
		 * Getter for promotion.
		 *
		 * @return value of {@link #promotion promotion}.
		 */
		public Promotion getPromotion() {
			return this.promotion;
		}

		/**
		 * Set the value of {@link #promotion promotion}.
		 * @param promotion value to set.
		 */
		public void setPromotion(Promotion promotion) {
			this.promotion = promotion;
		}

		/**
		 * Getter for plat.
		 *
		 * @return value of {@link #plat plat}.
		 */
		public Plat getPlat() {
			return this.plat;
		}

		/**
		 * Set the value of {@link #plat plat}.
		 * @param plat value to set.
		 */
		public void setPlat(Plat plat) {
			this.plat = plat;
		}

		public boolean equals(Object o) {
			if (o == this) {
				return true;
			}

			if (o == null) {
				return false;
			}

			if (this.getClass() != o.getClass()) {
				return false;
			}

			PromotionPlatId oId = (PromotionPlatId) o;

			if (this.promotion == null || oId.promotion == null || this.plat == null || oId.plat == null) {
				return false;
			}

			return Objects.equals(this.promotion.getId(), oId.promotion.getId())
			 && Objects.equals(this.plat.getId(), oId.plat.getId());
		}

		@Override
		public int hashCode() {
			return Objects.hash(promotion == null ? null : promotion.getId(), plat == null ? null : plat.getId());
		}
	}
}

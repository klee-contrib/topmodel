////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

/**
 * Association entre une promotion et un plat.
 */
@Table(name = "promotion_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionPlat {

	/**
	 * Promotion concernée.
	 */
	@Id
	private Integer promotionId;

	/**
	 * Plat concerné par la promotion.
	 */
	@Id
	private Integer platId;

	/**
	 * Getter for promotionId.
	 *
	 * @return value of {@link #promotionId promotionId}.
	 */
	public Integer getPromotionId() {
		return this.promotionId;
	}

	/**
	 * Getter for platId.
	 *
	 * @return value of {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Set the value of {@link #promotionId promotionId}.
	 * @param promotionId value to set.
	 */
	public void setPromotionId(Integer promotionId) {
		this.promotionId = promotionId;
	}

	/**
	 * Set the value of {@link #platId platId}.
	 * @param platId value to set.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}
}

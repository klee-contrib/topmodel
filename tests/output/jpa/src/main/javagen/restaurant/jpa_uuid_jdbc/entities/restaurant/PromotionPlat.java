////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

/**
 * Association entre une promotion et un plat.
 */
@Table(name = "promotion_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionPlat {

	/**
	 * Identifiant de l'association.
	 */
	@Id
	@Column("ppl_id")
	private Integer id;

	/**
	 * Promotion concernée.
	 */
	@Column("pro_id_promotion")
	private Integer promotionIdPromotion;

	/**
	 * Plat concerné par la promotion.
	 */
	@Column("pla_id_plat")
	private Integer platIdPlat;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for promotionIdPromotion.
	 *
	 * @return value of {@link #promotionIdPromotion promotionIdPromotion}.
	 */
	public Integer getPromotionIdPromotion() {
		return this.promotionIdPromotion;
	}

	/**
	 * Getter for platIdPlat.
	 *
	 * @return value of {@link #platIdPlat platIdPlat}.
	 */
	public Integer getPlatIdPlat() {
		return this.platIdPlat;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #promotionIdPromotion promotionIdPromotion}.
	 * @param promotionIdPromotion value to set.
	 */
	public void setPromotionIdPromotion(Integer promotionIdPromotion) {
		this.promotionIdPromotion = promotionIdPromotion;
	}

	/**
	 * Set the value of {@link #platIdPlat platIdPlat}.
	 * @param platIdPlat value to set.
	 */
	public void setPlatIdPlat(Integer platIdPlat) {
		this.platIdPlat = platIdPlat;
	}
}

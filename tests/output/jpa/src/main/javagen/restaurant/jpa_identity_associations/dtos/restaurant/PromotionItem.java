////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;

/**
 * Détail d'une promotion en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PromotionItem {

	/**
	 * Identifiant de la promotion.
	 */
	Integer getId();

	/**
	 * Libellé de la promotion.
	 */
	String getLibelle();

	/**
	 * Pourcentage de réduction (0-100).
	 */
	Integer getPourcentageReduction();

	/**
	 * Date de début de la promotion.
	 */
	LocalDateTime getDateDebut();

	/**
	 * Date de fin de la promotion.
	 */
	LocalDateTime getDateFin();

	/**
	 * Indique si la promotion est active.
	 */
	Boolean getActive();

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 */
	Integer getRestaurantId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param libelle value to set.
	 * @param pourcentageReduction value to set.
	 * @param dateDebut value to set.
	 * @param dateFin value to set.
	 * @param active value to set.
	 * @param restaurantId value to set.
	 */
	void hydrate(Integer id, String libelle, Integer pourcentageReduction, LocalDateTime dateDebut, LocalDateTime dateFin, Boolean active, Integer restaurantId);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Promotion.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Promotion_ {

	public static volatile SingularAttribute<Promotion, Integer> id;

	public static volatile SingularAttribute<Promotion, String> libelle;

	public static volatile SingularAttribute<Promotion, Integer> pourcentageReduction;

	public static volatile SingularAttribute<Promotion, LocalDateTime> dateDebut;

	public static volatile SingularAttribute<Promotion, LocalDateTime> dateFin;

	public static volatile SingularAttribute<Promotion, Boolean> active;

	public static volatile SingularAttribute<Promotion, Restaurant> restaurantRestaurant;

	public static volatile ListAttribute<Promotion, PromotionPlat> promotionPlatsPromotion;

	public static final String ID = "id";

	public static final String LIBELLE = "libelle";

	public static final String POURCENTAGE_REDUCTION = "pourcentageReduction";

	public static final String DATE_DEBUT = "dateDebut";

	public static final String DATE_FIN = "dateFin";

	public static final String ACTIVE = "active";

	public static final String RESTAURANT_RESTAURANT = "restaurantRestaurant";

	public static final String PROMOTION_PLATS_PROMOTION = "promotionPlatsPromotion";
}

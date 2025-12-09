////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import java.math.BigDecimal;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Plat.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Plat_ {

	public static volatile SingularAttribute<Plat, Integer> id;

	public static volatile SingularAttribute<Plat, String> nom;

	public static volatile SingularAttribute<Plat, String> description;

	public static volatile SingularAttribute<Plat, BigDecimal> prix;

	public static volatile SingularAttribute<Plat, Boolean> disponible;

	public static volatile SingularAttribute<Plat, CategoriePlat> categoriePlatCategoriePlat;

	public static volatile SingularAttribute<Plat, Restaurant> restaurantRestaurant;

	public static volatile ListAttribute<Plat, LigneCommande> ligneCommandes;

	public static volatile ListAttribute<Plat, MenuPlat> menuPlatsPlat;

	public static volatile ListAttribute<Plat, PromotionPlat> promotionPlatsPlat;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String DESCRIPTION = "description";

	public static final String PRIX = "prix";

	public static final String DISPONIBLE = "disponible";

	public static final String CATEGORIE_PLAT_CATEGORIE_PLAT = "categoriePlatCategoriePlat";

	public static final String RESTAURANT_RESTAURANT = "restaurantRestaurant";

	public static final String LIGNE_COMMANDES = "ligneCommandes";

	public static final String MENU_PLATS_PLAT = "menuPlatsPlat";

	public static final String PROMOTION_PLATS_PLAT = "promotionPlatsPlat";
}

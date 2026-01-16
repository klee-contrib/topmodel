////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Restaurant.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Restaurant_ {

	public static volatile SingularAttribute<Restaurant, Integer> id;

	public static volatile SingularAttribute<Restaurant, String> nom;

	public static volatile SingularAttribute<Restaurant, String> adresse;

	public static volatile SingularAttribute<Restaurant, String> telephone;

	public static volatile ListAttribute<Restaurant, Menu> menus;

	public static volatile ListAttribute<Restaurant, Plat> plats;

	public static volatile ListAttribute<Restaurant, Promotion> promotions;

	public static volatile ListAttribute<Restaurant, AvisClient> avisClients;

	public static volatile ListAttribute<Restaurant, TableRestaurant> tables;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String ADRESSE = "adresse";

	public static final String TELEPHONE = "telephone";

	public static final String MENUS = "menus";

	public static final String PLATS = "plats";

	public static final String PROMOTIONS = "promotions";

	public static final String AVIS_CLIENTS = "avisClients";

	public static final String TABLES = "tables";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

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

	public static volatile ListAttribute<Restaurant, TableClient> tableClientsRestaurant;

	public static volatile ListAttribute<Restaurant, Plat> platsRestaurant;

	public static volatile ListAttribute<Restaurant, AvisClient> avisClientsRestaurant;

	public static volatile ListAttribute<Restaurant, Menu> menusRestaurant;

	public static volatile ListAttribute<Restaurant, Reservation> reservationsRestaurant;

	public static volatile ListAttribute<Restaurant, Promotion> promotionsRestaurant;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String ADRESSE = "adresse";

	public static final String TELEPHONE = "telephone";

	public static final String TABLE_CLIENTS_RESTAURANT = "tableClientsRestaurant";

	public static final String PLATS_RESTAURANT = "platsRestaurant";

	public static final String AVIS_CLIENTS_RESTAURANT = "avisClientsRestaurant";

	public static final String MENUS_RESTAURANT = "menusRestaurant";

	public static final String RESERVATIONS_RESTAURANT = "reservationsRestaurant";

	public static final String PROMOTIONS_RESTAURANT = "promotionsRestaurant";
}

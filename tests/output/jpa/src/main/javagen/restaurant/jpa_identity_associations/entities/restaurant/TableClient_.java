////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(TableClient.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableClient_ {

	public static volatile SingularAttribute<TableClient, Integer> id;

	public static volatile SingularAttribute<TableClient, String> numero;

	public static volatile SingularAttribute<TableClient, Integer> capacite;

	public static volatile SingularAttribute<TableClient, Boolean> disponible;

	public static volatile SingularAttribute<TableClient, Restaurant> restaurantRestaurant;

	public static volatile ListAttribute<TableClient, Commande> commandes;

	public static volatile ListAttribute<TableClient, Reservation> reservationsTable;

	public static final String ID = "id";

	public static final String NUMERO = "numero";

	public static final String CAPACITE = "capacite";

	public static final String DISPONIBLE = "disponible";

	public static final String RESTAURANT_RESTAURANT = "restaurantRestaurant";

	public static final String COMMANDES = "commandes";

	public static final String RESERVATIONS_TABLE = "reservationsTable";
}

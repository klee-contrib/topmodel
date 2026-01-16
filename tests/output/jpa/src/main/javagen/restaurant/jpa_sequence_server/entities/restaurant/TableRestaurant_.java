////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(TableRestaurant.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableRestaurant_ {

	public static volatile SingularAttribute<TableRestaurant, Integer> id;

	public static volatile SingularAttribute<TableRestaurant, String> numero;

	public static volatile SingularAttribute<TableRestaurant, Integer> capacite;

	public static volatile SingularAttribute<TableRestaurant, Boolean> disponible;

	public static volatile SingularAttribute<TableRestaurant, Restaurant> restaurant;

	public static final String ID = "id";

	public static final String NUMERO = "numero";

	public static final String CAPACITE = "capacite";

	public static final String DISPONIBLE = "disponible";

	public static final String RESTAURANT = "restaurant";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Table.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Table_ {

	public static volatile SingularAttribute<Table, Integer> id;

	public static volatile SingularAttribute<Table, String> numero;

	public static volatile SingularAttribute<Table, Integer> capacite;

	public static volatile SingularAttribute<Table, Boolean> disponible;

	public static volatile SingularAttribute<Table, Integer> restaurantId;

	public static volatile SingularAttribute<Table, LocalDateTime> dateCreation;

	public static final String ID = "id";

	public static final String NUMERO = "numero";

	public static final String CAPACITE = "capacite";

	public static final String DISPONIBLE = "disponible";

	public static final String RESTAURANT_ID = "restaurantId";

	public static final String DATE_CREATION = "dateCreation";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
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

	public static volatile SingularAttribute<Plat, CategoriePlat> categoriePlat;

	public static volatile SingularAttribute<Plat, Restaurant> restaurant;

	public static volatile SingularAttribute<Plat, Promotion> promotion;

	public static volatile SingularAttribute<Plat, LocalDateTime> dateCreation;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String DESCRIPTION = "description";

	public static final String PRIX = "prix";

	public static final String DISPONIBLE = "disponible";

	public static final String CATEGORIE_PLAT = "categoriePlat";

	public static final String RESTAURANT = "restaurant";

	public static final String PROMOTION = "promotion";

	public static final String DATE_CREATION = "dateCreation";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Employe.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Employe_ extends Personne_ {

	public static volatile SingularAttribute<Employe, String> matricule;

	public static volatile SingularAttribute<Employe, LocalDateTime> dateEmbauche;

	public static volatile SingularAttribute<Employe, BigDecimal> salaire;

	public static volatile SingularAttribute<Employe, Restaurant> restaurantRestaurant;

	public static final String MATRICULE = "matricule";

	public static final String DATE_EMBAUCHE = "dateEmbauche";

	public static final String SALAIRE = "salaire";

	public static final String RESTAURANT_RESTAURANT = "restaurantRestaurant";
}

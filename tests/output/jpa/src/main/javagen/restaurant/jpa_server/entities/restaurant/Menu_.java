////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Menu.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Menu_ {

	public static volatile SingularAttribute<Menu, Integer> id;

	public static volatile SingularAttribute<Menu, String> nom;

	public static volatile SingularAttribute<Menu, String> description;

	public static volatile SingularAttribute<Menu, BigDecimal> prix;

	public static volatile SingularAttribute<Menu, Boolean> disponible;

	public static volatile SingularAttribute<Menu, LocalDateTime> dateDebut;

	public static volatile SingularAttribute<Menu, LocalDateTime> dateFin;

	public static volatile SingularAttribute<Menu, Restaurant> restaurant;

	public static volatile ListAttribute<Menu, MenuPlat> plats;

	public static volatile SingularAttribute<Menu, LocalDateTime> dateCreation;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String DESCRIPTION = "description";

	public static final String PRIX = "prix";

	public static final String DISPONIBLE = "disponible";

	public static final String DATE_DEBUT = "dateDebut";

	public static final String DATE_FIN = "dateFin";

	public static final String RESTAURANT = "restaurant";

	public static final String PLATS = "plats";

	public static final String DATE_CREATION = "dateCreation";
}

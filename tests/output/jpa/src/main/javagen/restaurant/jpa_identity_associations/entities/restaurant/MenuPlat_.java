////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(MenuPlat.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuPlat_ {

	public static volatile SingularAttribute<MenuPlat, Menu> menu;

	public static volatile SingularAttribute<MenuPlat, Plat> plat;

	public static volatile SingularAttribute<MenuPlat, Integer> ordre;

	public static final String MENU = "menu";

	public static final String PLAT = "plat";

	public static final String ORDRE = "ordre";
}

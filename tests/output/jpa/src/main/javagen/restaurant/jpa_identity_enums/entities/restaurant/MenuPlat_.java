////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(MenuPlat.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuPlat_ {

	public static volatile SingularAttribute<MenuPlat, Integer> id;

	public static volatile SingularAttribute<MenuPlat, Integer> ordre;

	public static volatile SingularAttribute<MenuPlat, Menu> menuMenu;

	public static volatile SingularAttribute<MenuPlat, Plat> platPlat;

	public static final String ID = "id";

	public static final String ORDRE = "ordre";

	public static final String MENU_MENU = "menuMenu";

	public static final String PLAT_PLAT = "platPlat";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.math.BigDecimal;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
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

	public static volatile ListAttribute<Plat, LigneCommande> ligneCommandes;

	public static volatile ListAttribute<Plat, PromotionPlat> promotions;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String DESCRIPTION = "description";

	public static final String PRIX = "prix";

	public static final String DISPONIBLE = "disponible";

	public static final String CATEGORIE_PLAT = "categoriePlat";

	public static final String RESTAURANT = "restaurant";

	public static final String LIGNE_COMMANDES = "ligneCommandes";

	public static final String PROMOTIONS = "promotions";
}

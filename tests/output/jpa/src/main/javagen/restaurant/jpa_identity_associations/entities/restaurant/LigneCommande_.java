////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(LigneCommande.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommande_ {

	public static volatile SingularAttribute<LigneCommande, Integer> id;

	public static volatile SingularAttribute<LigneCommande, Integer> quantite;

	public static volatile SingularAttribute<LigneCommande, BigDecimal> prixUnitaire;

	public static volatile SingularAttribute<LigneCommande, BigDecimal> prixTotal;

	public static volatile SingularAttribute<LigneCommande, Commande> commande;

	public static volatile SingularAttribute<LigneCommande, Plat> plat;

	public static final String ID = "id";

	public static final String QUANTITE = "quantite";

	public static final String PRIX_UNITAIRE = "prixUnitaire";

	public static final String PRIX_TOTAL = "prixTotal";

	public static final String COMMANDE = "commande";

	public static final String PLAT = "plat";
}

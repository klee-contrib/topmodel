////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(LigneCommandeHistorique.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeHistorique_ {

	public static volatile SingularAttribute<LigneCommandeHistorique, Integer> id;

	public static volatile SingularAttribute<LigneCommandeHistorique, Integer> quantite;

	public static volatile SingularAttribute<LigneCommandeHistorique, BigDecimal> prixUnitaire;

	public static volatile SingularAttribute<LigneCommandeHistorique, BigDecimal> prixTotal;

	public static volatile SingularAttribute<LigneCommandeHistorique, Plat> plat;

	public static volatile SingularAttribute<LigneCommandeHistorique, CommandeHistorique> commandeHistorique;

	public static final String ID = "id";

	public static final String QUANTITE = "quantite";

	public static final String PRIX_UNITAIRE = "prixUnitaire";

	public static final String PRIX_TOTAL = "prixTotal";

	public static final String PLAT = "plat";

	public static final String COMMANDE_HISTORIQUE = "commandeHistorique";
}

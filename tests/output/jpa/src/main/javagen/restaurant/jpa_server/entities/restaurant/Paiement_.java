////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Paiement.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Paiement_ {

	public static volatile SingularAttribute<Paiement, Facture> facture;

	public static volatile SingularAttribute<Paiement, Integer> swileCardId;

	public static final String FACTURE = "facture";

	public static final String SWILE_CARD_ID = "swileCardId";
}

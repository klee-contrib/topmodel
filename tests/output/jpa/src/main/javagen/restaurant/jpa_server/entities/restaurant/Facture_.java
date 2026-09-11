////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Facture.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Facture_ {

	public static volatile SingularAttribute<Facture, Integer> id;

	public static volatile SingularAttribute<Facture, Commande> commande;

	public static final String ID = "id";

	public static final String COMMANDE = "commande";
}

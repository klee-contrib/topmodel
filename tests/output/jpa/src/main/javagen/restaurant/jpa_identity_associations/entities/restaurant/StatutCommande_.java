////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_identity_associations.enums.restaurant.StatutCommandeCode;

@StaticMetamodel(StatutCommande.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class StatutCommande_ {

	public static volatile SingularAttribute<StatutCommande, StatutCommandeCode> code;

	public static volatile SingularAttribute<StatutCommande, String> libelle;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";
}

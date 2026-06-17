////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Fournisseur.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Fournisseur_ extends Lieu_ {

	public static volatile SingularAttribute<Fournisseur, String> telephone;

	public static volatile SingularAttribute<Fournisseur, Boolean> bio;

	public static final String TELEPHONE = "telephone";

	public static final String BIO = "bio";
}

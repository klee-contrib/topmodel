////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Personne.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Personne_ {

	public static volatile SingularAttribute<Personne, Integer> id;

	public static volatile SingularAttribute<Personne, String> nom;

	public static volatile SingularAttribute<Personne, String> prenom;

	public static volatile SingularAttribute<Personne, String> departementCode;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String PRENOM = "prenom";

	public static final String DEPARTEMENT_CODE = "departementCode";
}

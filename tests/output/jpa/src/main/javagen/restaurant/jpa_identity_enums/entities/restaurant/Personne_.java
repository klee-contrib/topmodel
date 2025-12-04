////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.entities.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Personne.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Personne_ {

	public static volatile SingularAttribute<Personne, Integer> id;

	public static volatile SingularAttribute<Personne, String> nom;

	public static volatile SingularAttribute<Personne, String> prenom;

	public static volatile SingularAttribute<Personne, String> email;

	public static volatile SingularAttribute<Personne, String> telephone;

	public static volatile SingularAttribute<Personne, LocalDateTime> dateNaissance;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String PRENOM = "prenom";

	public static final String EMAIL = "email";

	public static final String TELEPHONE = "telephone";

	public static final String DATE_NAISSANCE = "dateNaissance";
}

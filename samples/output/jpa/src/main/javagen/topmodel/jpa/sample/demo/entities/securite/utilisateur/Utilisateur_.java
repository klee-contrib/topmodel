////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.utilisateur;

import jakarta.annotation.Generated;
import jakarta.persistence.StaticMetaModel;

@StaticMetaModel(Utilisateur.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Utilisateur_ {

	public static volatile SingularAttribute<Utilisateur, Integer> id;

	public static volatile SingularAttribute<Utilisateur, String> nom;

	public static volatile SingularAttribute<Utilisateur, String> prenom;

	public static volatile SingularAttribute<Utilisateur, String> email;

	public static volatile SingularAttribute<Utilisateur, LocalDate> dateNaissance;

	public static volatile SingularAttribute<Utilisateur, String> adresse;

	public static volatile SingularAttribute<Utilisateur, Boolean> actif;

	public static volatile SingularAttribute<Utilisateur, Profil> profil;

	public static volatile SingularAttribute<Utilisateur, TypeUtilisateur> typeUtilisateur;

	public static volatile SingularAttribute<Utilisateur, LocalDateTime> dateCreation;

	public static volatile SingularAttribute<Utilisateur, LocalDateTime> dateModification;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String PRENOM = "prenom";

	public static final String EMAIL = "email";

	public static final String DATE_NAISSANCE = "dateNaissance";

	public static final String ADRESSE = "adresse";

	public static final String ACTIF = "actif";

	public static final String PROFIL = "profil";

	public static final String TYPE_UTILISATEUR = "typeUtilisateur";

	public static final String DATE_CREATION = "dateCreation";

	public static final String DATE_MODIFICATION = "dateModification";
}

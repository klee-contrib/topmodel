////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

import jakarta.annotation.Generated;
import jakarta.persistence.StaticMetaModel;

@StaticMetaModel(Profil.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Profil_ {

	public static volatile SingularAttribute<Profil, Integer> id;

	public static volatile SingularAttribute<Profil, String> libelle;

	public static volatile ListAttribute<Profil, ProfilDroit> profilDroits;

	public static volatile ListAttribute<Profil, Utilisateur> utilisateurs;

	public static volatile SingularAttribute<Profil, LocalDateTime> dateCreation;

	public static volatile SingularAttribute<Profil, LocalDateTime> dateModification;

	public static final String ID = "id";

	public static final String LIBELLE = "libelle";

	public static final String PROFIL_DROITS = "profilDroits";

	public static final String UTILISATEURS = "utilisateurs";

	public static final String DATE_CREATION = "dateCreation";

	public static final String DATE_MODIFICATION = "dateModification";
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(AvisClient.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class AvisClient_ {

	public static volatile SingularAttribute<AvisClient, Integer> id;

	public static volatile SingularAttribute<AvisClient, Integer> note;

	public static volatile SingularAttribute<AvisClient, String> commentaire;

	public static volatile SingularAttribute<AvisClient, LocalDateTime> dateAvis;

	public static volatile SingularAttribute<AvisClient, Boolean> approuve;

	public static volatile SingularAttribute<AvisClient, Integer> nombreVues;

	public static volatile SingularAttribute<AvisClient, Client> client;

	public static volatile SingularAttribute<AvisClient, Restaurant> restaurant;

	public static final String ID = "id";

	public static final String NOTE = "note";

	public static final String COMMENTAIRE = "commentaire";

	public static final String DATE_AVIS = "dateAvis";

	public static final String APPROUVE = "approuve";

	public static final String NOMBRE_VUES = "nombreVues";

	public static final String CLIENT = "client";

	public static final String RESTAURANT = "restaurant";
}

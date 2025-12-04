////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Client.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Client_ {

	public static volatile SingularAttribute<Client, Integer> id;

	public static volatile SingularAttribute<Client, String> nom;

	public static volatile SingularAttribute<Client, String> prenom;

	public static volatile SingularAttribute<Client, String> telephone;

	public static volatile SingularAttribute<Client, String> email;

	public static volatile ListAttribute<Client, Commande> commandes;

	public static volatile ListAttribute<Client, AvisClient> avisClientsClient;

	public static volatile ListAttribute<Client, Reservation> reservationsClient;

	public static final String ID = "id";

	public static final String NOM = "nom";

	public static final String PRENOM = "prenom";

	public static final String TELEPHONE = "telephone";

	public static final String EMAIL = "email";

	public static final String COMMANDES = "commandes";

	public static final String AVIS_CLIENTS_CLIENT = "avisClientsClient";

	public static final String RESERVATIONS_CLIENT = "reservationsClient";
}

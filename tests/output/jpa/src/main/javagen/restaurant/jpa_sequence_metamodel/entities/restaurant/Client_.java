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
public class Client_ extends Personne_ {

	public static volatile SingularAttribute<Client, String> email;

	public static volatile ListAttribute<Client, AvisClient> avisClients;

	public static final String EMAIL = "email";

	public static final String AVIS_CLIENTS = "avisClients";
}

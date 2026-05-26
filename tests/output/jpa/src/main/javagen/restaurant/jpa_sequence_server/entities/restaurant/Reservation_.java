////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(Reservation.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Reservation_ {

	public static volatile SingularAttribute<Reservation, Integer> id;

	public static volatile SingularAttribute<Reservation, LocalDateTime> dateReservation;

	public static volatile SingularAttribute<Reservation, Integer> nombrePersonnes;

	public static volatile SingularAttribute<Reservation, String> commentaire;

	public static volatile SingularAttribute<Reservation, Boolean> confirmee;

	public static volatile SingularAttribute<Reservation, Client> client;

	public static volatile SingularAttribute<Reservation, Integer> tableId;

	public static volatile SingularAttribute<Reservation, Restaurant> restaurant;

	public static volatile SingularAttribute<Reservation, LocalDateTime> dateCreation;

	public static final String ID = "id";

	public static final String DATE_RESERVATION = "dateReservation";

	public static final String NOMBRE_PERSONNES = "nombrePersonnes";

	public static final String COMMENTAIRE = "commentaire";

	public static final String CONFIRMEE = "confirmee";

	public static final String CLIENT = "client";

	public static final String TABLE_ID = "tableId";

	public static final String RESTAURANT = "restaurant";

	public static final String DATE_CREATION = "dateCreation";
}

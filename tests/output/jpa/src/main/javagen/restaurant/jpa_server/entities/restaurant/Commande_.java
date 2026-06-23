////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_server.enums.restaurant.StatutCommande;

@StaticMetamodel(Commande.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Commande_ {

	public static volatile SingularAttribute<Commande, Integer> id;

	public static volatile SingularAttribute<Commande, LocalDateTime> dateCommande;

	public static volatile SingularAttribute<Commande, LocalDateTime> dateLivraison;

	public static volatile SingularAttribute<Commande, BigDecimal> montantTotal;

	public static volatile SingularAttribute<Commande, Client> client;

	public static volatile SingularAttribute<Commande, Integer> tableId;

	public static volatile SingularAttribute<Commande, Reservation> reservation;

	public static volatile SingularAttribute<Commande, StatutCommande> statutCommande;

	public static volatile SingularAttribute<Commande, AvisClient> avisClient;

	public static volatile ListAttribute<Commande, LigneCommande> lignes;

	public static volatile SingularAttribute<Commande, LocalDateTime> dateCreation;

	public static final String ID = "id";

	public static final String DATE_COMMANDE = "dateCommande";

	public static final String DATE_LIVRAISON = "dateLivraison";

	public static final String MONTANT_TOTAL = "montantTotal";

	public static final String CLIENT = "client";

	public static final String TABLE_ID = "tableId";

	public static final String RESERVATION = "reservation";

	public static final String STATUT_COMMANDE = "statutCommande";

	public static final String AVIS_CLIENT = "avisClient";

	public static final String LIGNES = "lignes";

	public static final String DATE_CREATION = "dateCreation";
}

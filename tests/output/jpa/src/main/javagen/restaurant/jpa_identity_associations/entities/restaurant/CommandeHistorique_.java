////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.ListAttribute;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

@StaticMetamodel(CommandeHistorique.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique_ {

	public static volatile SingularAttribute<CommandeHistorique, Integer> id;

	public static volatile SingularAttribute<CommandeHistorique, LocalDateTime> dateCommande;

	public static volatile SingularAttribute<CommandeHistorique, LocalDateTime> dateLivraison;

	public static volatile SingularAttribute<CommandeHistorique, BigDecimal> montantTotal;

	public static volatile SingularAttribute<CommandeHistorique, Client> client;

	public static volatile SingularAttribute<CommandeHistorique, TableRestaurant> table;

	public static volatile SingularAttribute<CommandeHistorique, Reservation> reservation;

	public static volatile SingularAttribute<CommandeHistorique, StatutCommande> statutCommande;

	public static volatile SingularAttribute<CommandeHistorique, AvisClient> avisClient;

	public static volatile ListAttribute<CommandeHistorique, LigneCommandeHistorique> lignes;

	public static final String ID = "id";

	public static final String DATE_COMMANDE = "dateCommande";

	public static final String DATE_LIVRAISON = "dateLivraison";

	public static final String MONTANT_TOTAL = "montantTotal";

	public static final String CLIENT = "client";

	public static final String TABLE = "table";

	public static final String RESERVATION = "reservation";

	public static final String STATUT_COMMANDE = "statutCommande";

	public static final String AVIS_CLIENT = "avisClient";

	public static final String LIGNES = "lignes";
}

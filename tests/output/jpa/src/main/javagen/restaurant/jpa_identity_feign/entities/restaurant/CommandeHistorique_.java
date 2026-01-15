////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_identity_feign.enums.restaurant.StatutCommande;

@StaticMetamodel(CommandeHistorique.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique_ {

	public static volatile SingularAttribute<CommandeHistorique, Integer> id;

	public static volatile SingularAttribute<CommandeHistorique, LocalDateTime> dateCommande;

	public static volatile SingularAttribute<CommandeHistorique, LocalDateTime> dateLivraison;

	public static volatile SingularAttribute<CommandeHistorique, BigDecimal> montantTotal;

	public static volatile SingularAttribute<CommandeHistorique, Integer> clientId;

	public static volatile SingularAttribute<CommandeHistorique, Integer> tableId;

	public static volatile SingularAttribute<CommandeHistorique, Integer> reservationId;

	public static volatile SingularAttribute<CommandeHistorique, StatutCommande> statutCommandeCode;

	public static volatile SingularAttribute<CommandeHistorique, Integer> avisClientId;

	public static final String ID = "id";

	public static final String DATE_COMMANDE = "dateCommande";

	public static final String DATE_LIVRAISON = "dateLivraison";

	public static final String MONTANT_TOTAL = "montantTotal";

	public static final String CLIENT_ID = "clientId";

	public static final String TABLE_ID = "tableId";

	public static final String RESERVATION_ID = "reservationId";

	public static final String STATUT_COMMANDE_CODE = "statutCommandeCode";

	public static final String AVIS_CLIENT_ID = "avisClientId";
}

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

@StaticMetamodel(CommandeExport.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeExport_ {

	public static volatile SingularAttribute<CommandeExport, Integer> id;

	public static volatile SingularAttribute<CommandeExport, LocalDateTime> dateCommande;

	public static volatile SingularAttribute<CommandeExport, LocalDateTime> dateLivraison;

	public static volatile SingularAttribute<CommandeExport, BigDecimal> montantTotal;

	public static volatile SingularAttribute<CommandeExport, Client> client;

	public static volatile SingularAttribute<CommandeExport, TableClient> tableClient;

	public static volatile SingularAttribute<CommandeExport, StatutCommande> statutCommande;

	public static volatile ListAttribute<CommandeExport, LigneCommande> ligneCommandes;

	public static final String ID = "id";

	public static final String DATE_COMMANDE = "dateCommande";

	public static final String DATE_LIVRAISON = "dateLivraison";

	public static final String MONTANT_TOTAL = "montantTotal";

	public static final String CLIENT = "client";

	public static final String TABLE_CLIENT = "tableClient";

	public static final String STATUT_COMMANDE = "statutCommande";

	public static final String LIGNE_COMMANDES = "ligneCommandes";
}

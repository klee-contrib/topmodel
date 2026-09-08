////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'un paiement.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record PaiementItem(
	@Column("id") @NotNull Integer factureId,
	@Column("swi_id") @NotNull Integer swileCardId,
	@Column("com_date_commande") @NotNull LocalDateTime dateCommande,
	@Column("com_montant_total") @NotNull BigDecimal montantTotal
) {
}

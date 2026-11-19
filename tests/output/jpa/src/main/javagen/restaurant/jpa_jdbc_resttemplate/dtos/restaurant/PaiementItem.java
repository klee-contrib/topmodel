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
 *
 * @param factureId Facture associée au paiement.
 *                  Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Paiement#getFacture() Paiement#getFacture()}
 * @param swileCardId Carte Swile utilisée pour le paiement.
 *                    Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Paiement#getSwileCardId() Paiement#getSwileCardId()}
 * @param dateCommande Date et heure de la commande.
 *                     Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
 * @param montantTotal Montant total de la commande.
 *                     Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record PaiementItem(
	@Column("id") @NotNull Integer factureId,
	@Column("swi_id") @NotNull Integer swileCardId,
	@Column("com_date_commande") @NotNull LocalDateTime dateCommande,
	@Column("com_montant_total") @NotNull BigDecimal montantTotal
) {
}

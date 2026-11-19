////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'un paiement.
 *
 * @param factureId Facture associée au paiement.
 *                  Alias of {@link restaurant.jpa_server.entities.restaurant.Paiement#getFacture() Paiement#getFacture()}
 * @param swileCardId Carte Swile utilisée pour le paiement.
 *                    Alias of {@link restaurant.jpa_server.entities.restaurant.Paiement#getSwileCardId() Paiement#getSwileCardId()}
 * @param dateCommande Date et heure de la commande.
 *                     Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
 * @param montantTotal Montant total de la commande.
 *                     Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record PaiementItem(
	@NotNull Integer factureId,
	@NotNull Integer swileCardId,
	@NotNull LocalDateTime dateCommande,
	@NotNull BigDecimal montantTotal
) {
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'un paiement.
 *
 * @param factureId Facture associée au paiement.
 *                  Alias de {@link restaurant.jpa_feign.entities.restaurant.Paiement#getFacture() Paiement#getFacture()}
 * @param swileCardId Carte Swile utilisée pour le paiement.
 *                    Alias de {@link restaurant.jpa_feign.entities.restaurant.Paiement#getSwileCardId() Paiement#getSwileCardId()}
 * @param dateCommande Date et heure de la commande.
 *                     Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
 * @param montantTotal Montant total de la commande.
 *                     Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record PaiementItem(
	@NotNull Integer factureId,
	@NotNull Integer swileCardId,
	@NotNull LocalDateTime dateCommande,
	@NotNull BigDecimal montantTotal
) {

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.PaiementItem PaiementItem}.
	 */
	public enum Fields {
		FACTURE_ID(Integer.class),
		SWILE_CARD_ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

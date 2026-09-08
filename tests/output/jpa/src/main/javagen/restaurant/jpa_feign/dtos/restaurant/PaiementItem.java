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
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record PaiementItem(
	@NotNull Integer factureId,
	@NotNull Integer swileCardId,
	@NotNull LocalDateTime dateCommande,
	@NotNull BigDecimal montantTotal
) {

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.PaiementItem PaiementItem}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

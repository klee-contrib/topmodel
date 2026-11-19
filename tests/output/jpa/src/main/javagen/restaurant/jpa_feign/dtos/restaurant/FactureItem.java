////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_feign.enums.restaurant.TypeFactureCode;

/**
 * Détail d'une facture.
 *
 * @param id Identifiant de la facture.
 *           Alias of {@link restaurant.jpa_feign.entities.restaurant.Facture#getId() Facture#getId()}
 * @param commandeId Commande associée à la facture.
 *                   Alias of {@link restaurant.jpa_feign.entities.restaurant.Facture#getCommande() Facture#getCommande()}
 * @param typeFactureCode type de facture.
 *                        Alias of {@link restaurant.jpa_feign.entities.restaurant.Facture#getTypeFactureCode() Facture#getTypeFactureCode()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@NotNull Integer id,
	@NotNull Integer commandeId,
	TypeFactureCode typeFactureCode
) {

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.FactureItem FactureItem}.
	 */
	public enum Fields {
		ID(Integer.class),
		COMMANDE_ID(Integer.class),
		TYPE_FACTURE_CODE(TypeFactureCode.class);

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

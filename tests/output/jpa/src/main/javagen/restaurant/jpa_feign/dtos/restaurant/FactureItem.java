////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'une facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@NotNull Integer id,
	@NotNull Integer commandeId
) {

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.FactureItem FactureItem}.
	 */
	public enum Fields {
		ID(Integer.class),
		COMMANDE_ID(Integer.class);

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

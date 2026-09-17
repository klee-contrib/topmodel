////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_server.enums.restaurant.TypeFactureCode;

/**
 * Détail d'une facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@NotNull Integer id,
	@NotNull Integer commandeId,
	TypeFactureCode typeFactureCode
) {
}

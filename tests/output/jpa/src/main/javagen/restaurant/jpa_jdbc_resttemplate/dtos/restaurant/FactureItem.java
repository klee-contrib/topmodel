////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'une facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@Column("id") @NotNull Integer id,
	@Column("com_id") @NotNull Integer commandeId
) {
}

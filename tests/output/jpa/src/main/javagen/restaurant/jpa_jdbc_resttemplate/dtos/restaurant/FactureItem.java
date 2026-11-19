////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une facture.
 *
 * @param id Identifiant de la facture.
 *           Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Facture#getId() Facture#getId()}
 * @param commandeId Commande associée à la facture.
 *                   Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Facture#getCommande() Facture#getCommande()}
 * @param typeFactureCode type de facture.
 *                        Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Facture#getTypeFactureCode() Facture#getTypeFactureCode()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@Column("id") @NotNull Integer id,
	@Column("com_id") @NotNull Integer commandeId,
	@Column("tfa_code") @Size(max = 10) String typeFactureCode
) {
}

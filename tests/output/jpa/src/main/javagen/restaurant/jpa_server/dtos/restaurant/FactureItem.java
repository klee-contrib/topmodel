////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_server.enums.restaurant.TypeFactureCode;

/**
 * Détail d'une facture.
 *
 * @param id Identifiant de la facture.
 *           Alias de {@link restaurant.jpa_server.entities.restaurant.Facture#getId() Facture#getId()}
 * @param commandeId Commande associée à la facture.
 *                   Alias de {@link restaurant.jpa_server.entities.restaurant.Facture#getCommande() Facture#getCommande()}
 * @param typeFactureCode type de facture.
 *                        Alias de {@link restaurant.jpa_server.entities.restaurant.Facture#getTypeFactureCode() Facture#getTypeFactureCode()}
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record FactureItem(
	@NotNull Integer id,
	@NotNull Integer commandeId,
	TypeFactureCode typeFactureCode
) {
}

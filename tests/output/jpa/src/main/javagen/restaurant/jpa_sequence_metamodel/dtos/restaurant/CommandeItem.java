////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

import restaurant.jpa_sequence_metamodel.enums.restaurant.StatutCommandeCode;

/**
 * Détail d'une commande en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface CommandeItem {

	/**
	 * Identifiant de la commande.
	 */
	Integer getId();

	/**
	 * Date et heure de la commande.
	 */
	LocalDateTime getDateCommande();

	/**
	 * Montant total de la commande.
	 */
	BigDecimal getMontantTotal();

	/**
	 * Statut de la commande.
	 */
	StatutCommandeCode getStatutCommandeCode();

	/**
	 * Client ayant passé la commande.
	 */
	Integer getClientId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param dateCommande value to set.
	 * @param montantTotal value to set.
	 * @param statutCommandeCode value to set.
	 * @param clientId value to set.
	 */
	void hydrate(Integer id, LocalDateTime dateCommande, BigDecimal montantTotal, StatutCommandeCode statutCommandeCode, Integer clientId);
}

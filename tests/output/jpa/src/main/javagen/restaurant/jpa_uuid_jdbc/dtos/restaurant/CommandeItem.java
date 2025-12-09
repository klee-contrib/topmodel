////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

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
	String getStatutCommandeCode();

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
	void hydrate(Integer id, LocalDateTime dateCommande, BigDecimal montantTotal, String statutCommandeCode, Integer clientId);
}

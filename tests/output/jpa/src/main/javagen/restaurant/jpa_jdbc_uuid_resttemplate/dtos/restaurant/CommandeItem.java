////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.StatutCommande;

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
	StatutCommande getStatutCommande();

	/**
	 * Client ayant passé la commande.
	 */
	Integer getClientId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param dateCommande value to set.
	 * @param montantTotal value to set.
	 * @param statutCommande value to set.
	 * @param clientId value to set.
	 */
	void hydrate(Integer id, LocalDateTime dateCommande, BigDecimal montantTotal, StatutCommande statutCommande, Integer clientId);
}

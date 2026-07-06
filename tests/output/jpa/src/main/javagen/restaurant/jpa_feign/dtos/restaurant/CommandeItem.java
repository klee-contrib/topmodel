////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

import restaurant.jpa_feign.enums.restaurant.StatutCommande;

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
	 * Identifiant de la commande.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Date et heure de la commande.
	 * @param dateCommande value to set.
	 */
	void setDateCommande(LocalDateTime dateCommande);

	/**
	 * Montant total de la commande.
	 * @param montantTotal value to set.
	 */
	void setMontantTotal(BigDecimal montantTotal);

	/**
	 * Statut de la commande.
	 * @param statutCommande value to set.
	 */
	void setStatutCommande(StatutCommande statutCommande);

	/**
	 * Client ayant passé la commande.
	 * @param clientId value to set.
	 */
	void setClientId(Integer clientId);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

/**
 * Détail d'une ligne de commande en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface LigneCommandeItem {

	/**
	 * Identifiant de la ligne.
	 */
	Integer getId();

	/**
	 * Quantité commandée.
	 */
	Integer getQuantite();

	/**
	 * Prix unitaire au moment de la commande.
	 */
	BigDecimal getPrixUnitaire();

	/**
	 * Prix total de la ligne.
	 */
	BigDecimal getPrixTotal();

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	Integer getCommandeId();

	/**
	 * Plat commandé.
	 */
	Integer getPlatId();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param quantite value to set.
	 * @param prixUnitaire value to set.
	 * @param prixTotal value to set.
	 * @param commandeId value to set.
	 * @param platId value to set.
	 */
	void hydrate(Integer id, Integer quantite, BigDecimal prixUnitaire, BigDecimal prixTotal, Integer commandeId, Integer platId);
}

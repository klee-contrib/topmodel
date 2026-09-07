////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

/**
 * Paiement.
 */
@Table(name = "paiement")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Paiement {

	/**
	 * Facture associée au paiement.
	 */
	@Id
	private Integer facture;

	/**
	 * Carte Swile utilisée pour le paiement.
	 */
	@Id
	private Integer swileCardId;

	/**
	 * Getter for facture.
	 *
	 * @return value of {@link #facture facture}.
	 */
	public Integer getFacture() {
		return this.facture;
	}

	/**
	 * Getter for swileCardId.
	 *
	 * @return value of {@link #swileCardId swileCardId}.
	 */
	public Integer getSwileCardId() {
		return this.swileCardId;
	}

	/**
	 * Set the value of {@link #facture facture}.
	 * @param facture value to set.
	 */
	public void setFacture(Integer facture) {
		this.facture = facture;
	}

	/**
	 * Set the value of {@link #swileCardId swileCardId}.
	 * @param swileCardId value to set.
	 */
	public void setSwileCardId(Integer swileCardId) {
		this.swileCardId = swileCardId;
	}
}

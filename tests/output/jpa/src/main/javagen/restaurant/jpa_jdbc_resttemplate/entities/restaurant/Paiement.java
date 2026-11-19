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
	 * Accesseur de 'facture'.
	 *
	 * @return Valeur de {@link #facture facture}.
	 */
	public Integer getFacture() {
		return this.facture;
	}

	/**
	 * Accesseur de 'swileCardId'.
	 *
	 * @return Valeur de {@link #swileCardId swileCardId}.
	 */
	public Integer getSwileCardId() {
		return this.swileCardId;
	}

	/**
	 * Définit la valeur de {@link #facture facture}.
	 * @param facture Valeur à définir.
	 */
	public void setFacture(Integer facture) {
		this.facture = facture;
	}

	/**
	 * Définit la valeur de {@link #swileCardId swileCardId}.
	 * @param swileCardId Valeur à définir.
	 */
	public void setSwileCardId(Integer swileCardId) {
		this.swileCardId = swileCardId;
	}
}

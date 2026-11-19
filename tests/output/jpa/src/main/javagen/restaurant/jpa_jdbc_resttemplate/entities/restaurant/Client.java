////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

/**
 * Client du restaurant.
 */
@Table(name = "client")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Client extends Personne {

	/**
	 * Adresse email du client.
	 */
	@Column("cli_email")
	private String email;

	/**
	 * Carte Swile du client.
	 */
	@Column("swi_id")
	private Integer swileCard;

	/**
	 * Accesseur de 'email'.
	 *
	 * @return Valeur de {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Accesseur de 'swileCard'.
	 *
	 * @return Valeur de {@link #swileCard swileCard}.
	 */
	public Integer getSwileCard() {
		return this.swileCard;
	}

	/**
	 * Définit la valeur de {@link #email email}.
	 * @param email Valeur à définir.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Définit la valeur de {@link #swileCard swileCard}.
	 * @param swileCard Valeur à définir.
	 */
	public void setSwileCard(Integer swileCard) {
		this.swileCard = swileCard;
	}
}

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
	 * Getter for email.
	 *
	 * @return value of {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for swileCard.
	 *
	 * @return value of {@link #swileCard swileCard}.
	 */
	public Integer getSwileCard() {
		return this.swileCard;
	}

	/**
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #swileCard swileCard}.
	 * @param swileCard value to set.
	 */
	public void setSwileCard(Integer swileCard) {
		this.swileCard = swileCard;
	}
}

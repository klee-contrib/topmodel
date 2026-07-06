////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

/**
 * Restaurant.
 */
@Table(name = "fournisseur")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Fournisseur extends Lieu {

	/**
	 * Numéro de téléphone.
	 */
	@Column("frn_telephone")
	private String telephone;

	/**
	 * Si le fournisseur fait du bio.
	 */
	@Column("frn_bio")
	private Boolean bio;

	/**
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Getter for bio.
	 *
	 * @return value of {@link #bio bio}.
	 */
	public Boolean getBio() {
		return this.bio;
	}

	/**
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Set the value of {@link #bio bio}.
	 * @param bio value to set.
	 */
	public void setBio(Boolean bio) {
		this.bio = bio;
	}
}

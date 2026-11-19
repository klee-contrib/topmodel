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
	private String telephone = "xx.xx.xx.xx.xx";

	/**
	 * Si le fournisseur fait du bio.
	 */
	@Column("frn_bio")
	private Boolean bio;

	/**
	 * Accesseur de 'telephone'.
	 *
	 * @return Valeur de {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Accesseur de 'bio'.
	 *
	 * @return Valeur de {@link #bio bio}.
	 */
	public Boolean getBio() {
		return this.bio;
	}

	/**
	 * Définit la valeur de {@link #telephone telephone}.
	 * @param telephone Valeur à définir.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Définit la valeur de {@link #bio bio}.
	 * @param bio Valeur à définir.
	 */
	public void setBio(Boolean bio) {
		this.bio = bio;
	}
}

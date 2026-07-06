////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

/**
 * Restaurant.
 */
@Entity
@DiscriminatorValue("FOURNISSEUR")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Fournisseur extends Lieu {

	/**
	 * Numéro de téléphone.
	 */
	@Column(name = "FRN_TELEPHONE", length = 20, columnDefinition = "varchar")
	private String telephone;

	/**
	 * Si le fournisseur fait du bio.
	 */
	@Column(name = "FRN_BIO", columnDefinition = "boolean")
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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Fournisseur Fournisseur}.
	 */
	public enum Fields {
		TELEPHONE(String.class),
		BIO(Boolean.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

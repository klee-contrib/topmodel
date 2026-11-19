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
	@Column(name = "frn_telephone", length = 20, columnDefinition = "varchar")
	private String telephone = "xx.xx.xx.xx.xx";

	/**
	 * Si le fournisseur fait du bio.
	 */
	@Column(name = "frn_bio", columnDefinition = "boolean")
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Fournisseur Fournisseur}.
	 */
	public enum Fields {
		TELEPHONE(String.class),
		BIO(Boolean.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

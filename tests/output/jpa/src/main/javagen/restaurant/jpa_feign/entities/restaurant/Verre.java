////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;

/**
 * Verre.
 */
@Entity
@Table(name = "VERRE")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Verre extends Vaisselle {

	/**
	 * Si le verre est à pied ou non.
	 */
	@Column(name = "VRR_A_PIED", nullable = false, columnDefinition = "boolean")
	private Boolean aPied;

	/**
	 * Getter for aPied.
	 *
	 * @return value of {@link #aPied aPied}.
	 */
	public Boolean getAPied() {
		return this.aPied;
	}

	/**
	 * Set the value of {@link #aPied aPied}.
	 * @param aPied value to set.
	 */
	public void setAPied(Boolean aPied) {
		this.aPied = aPied;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Verre Verre}.
	 */
	public enum Fields {
		A_PIED(Boolean.class);

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

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
@Table(name = "verre")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Verre extends Vaisselle {

	/**
	 * Si le verre est à pied ou non.
	 */
	@Column(name = "vrr_a_pied", nullable = false, columnDefinition = "boolean")
	private Boolean aPied;

	/**
	 * Accesseur de 'aPied'.
	 *
	 * @return Valeur de {@link #aPied aPied}.
	 */
	public Boolean getAPied() {
		return this.aPied;
	}

	/**
	 * Définit la valeur de {@link #aPied aPied}.
	 * @param aPied Valeur à définir.
	 */
	public void setAPied(Boolean aPied) {
		this.aPied = aPied;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Verre Verre}.
	 */
	public enum Fields {
		A_PIED(Boolean.class);

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

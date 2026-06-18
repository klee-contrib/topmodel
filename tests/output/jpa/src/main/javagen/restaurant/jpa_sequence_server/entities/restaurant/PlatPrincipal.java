////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

/**
 * Plat principal.
 */
@Entity
@DiscriminatorValue("PRINCIPAL")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatPrincipal extends Plat {

	/**
	 * Si le plat est végétarien.
	 */
	@Column(name = "PPR_VEGETARIEN", nullable = false, columnDefinition = "boolean")
	private Boolean vegetarien;

	/**
	 * Getter for vegetarien.
	 *
	 * @return value of {@link #vegetarien vegetarien}.
	 */
	public Boolean getVegetarien() {
		return this.vegetarien;
	}

	/**
	 * Set the value of {@link #vegetarien vegetarien}.
	 * @param vegetarien value to set.
	 */
	public void setVegetarien(Boolean vegetarien) {
		this.vegetarien = vegetarien;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.PlatPrincipal PlatPrincipal}.
	 */
	public enum Fields {
		VEGETARIEN(Boolean.class);

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

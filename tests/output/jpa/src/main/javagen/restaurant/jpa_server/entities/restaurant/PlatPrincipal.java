////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

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
	@Column(name = "ppr_vegetarien", nullable = false, columnDefinition = "boolean")
	private Boolean vegetarien;

	/**
	 * Accesseur de 'vegetarien'.
	 *
	 * @return Valeur de {@link #vegetarien vegetarien}.
	 */
	public Boolean getVegetarien() {
		return this.vegetarien;
	}

	/**
	 * Définit la valeur de {@link #vegetarien vegetarien}.
	 * @param vegetarien Valeur à définir.
	 */
	public void setVegetarien(Boolean vegetarien) {
		this.vegetarien = vegetarien;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.PlatPrincipal PlatPrincipal}.
	 */
	public enum Fields {
		VEGETARIEN(Boolean.class);

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

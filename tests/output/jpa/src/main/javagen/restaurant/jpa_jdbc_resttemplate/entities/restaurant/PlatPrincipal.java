////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Plat principal.
 */
@Table(name = "plat_principal")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatPrincipal extends Plat {

	/**
	 * Si le plat est végétarien.
	 */
	@NotNull
	@Column("ppr_vegetarien")
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
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Verre.
 */
@Table(name = "verre")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Verre extends Vaisselle {

	/**
	 * Si le verre est à pied ou non.
	 */
	@NotNull
	@Column("vrr_a_pied")
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
}

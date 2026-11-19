////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Assiette.
 */
@Table(name = "assiette")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Assiette extends Vaisselle {

	/**
	 * Taille de l'assiette.
	 */
	@NotNull
	@Column("ast_taille")
	private Integer taille;

	/**
	 * Getter for taille.
	 *
	 * @return value of {@link #taille taille}.
	 */
	public Integer getTaille() {
		return this.taille;
	}

	/**
	 * Set the value of {@link #taille taille}.
	 * @param taille value to set.
	 */
	public void setTaille(Integer taille) {
		this.taille = taille;
	}
}

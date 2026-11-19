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
	 * Accesseur de 'taille'.
	 *
	 * @return Valeur de {@link #taille taille}.
	 */
	public Integer getTaille() {
		return this.taille;
	}

	/**
	 * Définit la valeur de {@link #taille taille}.
	 * @param taille Valeur à définir.
	 */
	public void setTaille(Integer taille) {
		this.taille = taille;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;

/**
 * Assiette.
 */
@Entity
@Table(name = "ASSIETTE")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Assiette extends Vaisselle {

	/**
	 * Taille de l'assiette.
	 */
	@Column(name = "AST_TAILLE", nullable = false, columnDefinition = "int")
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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Assiette Assiette}.
	 */
	public enum Fields {
		TAILLE(Integer.class);

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

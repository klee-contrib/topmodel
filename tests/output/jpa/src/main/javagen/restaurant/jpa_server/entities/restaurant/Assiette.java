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
@Table(name = "assiette")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Assiette extends Vaisselle {

	/**
	 * Taille de l'assiette.
	 */
	@Column(name = "ast_taille", nullable = false, columnDefinition = "int")
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Assiette Assiette}.
	 */
	public enum Fields {
		TAILLE(Integer.class);

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

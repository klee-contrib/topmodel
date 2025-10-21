////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.common;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Classe pour contenir les traductions en base de données.
 */
@Entity
@Table(name = "TRADUCTION")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Traduction {

	/**
	 * Clé de traduction.
	 */
	@Id
	@Column(name = "TRD_RESOURCE_KEY", nullable = false, length = 100, columnDefinition = "varchar")
	private String resourceKey;

	/**
	 * Valeur.
	 */
	@Column(name = "TRD_LABEL", nullable = false, length = 100, columnDefinition = "varchar")
	private String label;

	/**
	 * Getter for resourceKey.
	 *
	 * @return value of {@link #resourceKey resourceKey}.
	 */
	public String getResourceKey() {
		return this.resourceKey;
	}

	/**
	 * Getter for label.
	 *
	 * @return value of {@link #label label}.
	 */
	public String getLabel() {
		return this.label;
	}

	/**
	 * Set the value of {@link #resourceKey resourceKey}.
	 * @param resourceKey value to set.
	 */
	public void setResourceKey(String resourceKey) {
		this.resourceKey = resourceKey;
	}

	/**
	 * Set the value of {@link #label label}.
	 * @param label value to set.
	 */
	public void setLabel(String label) {
		this.label = label;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.common.Traduction Traduction}.
	 */
	public enum Fields {
		RESOURCE_KEY(String.class),
		LABEL(String.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.common;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Table pour stocker les traductions en SQL.
 */
@Entity
@Table(name = "TRANSLATION")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Translation {

	/**
	 * Clé de traduction.
	 */
	@Id
	@Column(name = "TRA_RESOURCE_KEY", nullable = false, length = 100, columnDefinition = "varchar")
	private String resourceKey;

	/**
	 * Valeur de la clé de traduction.
	 */
	@Column(name = "TRA_VALUE", nullable = false, length = 100, columnDefinition = "varchar")
	private String value;

	/**
	 * Getter for resourceKey.
	 *
	 * @return value of {@link #resourceKey resourceKey}.
	 */
	public String getResourceKey() {
		return this.resourceKey;
	}

	/**
	 * Getter for value.
	 *
	 * @return value of {@link #value value}.
	 */
	public String getValue() {
		return this.value;
	}

	/**
	 * Set the value of {@link #resourceKey resourceKey}.
	 * @param resourceKey value to set.
	 */
	public void setResourceKey(String resourceKey) {
		this.resourceKey = resourceKey;
	}

	/**
	 * Set the value of {@link #value value}.
	 * @param value value to set.
	 */
	public void setValue(String value) {
		this.value = value;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.common.Translation Translation}.
	 */
	public enum Fields {
		RESOURCE_KEY(String.class),
		VALUE(String.class);

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

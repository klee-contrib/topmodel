////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.common;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Table pour stocker les traductions en SQL.
 */
@Table(name = "translation")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Translation {

	/**
	 * Clé de traduction.
	 */
	@Id
	@Column("tra_resource_key")
	private String resourceKey;

	/**
	 * Valeur de la clé de traduction.
	 */
	@NotNull
	@Column("tra_value")
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
}

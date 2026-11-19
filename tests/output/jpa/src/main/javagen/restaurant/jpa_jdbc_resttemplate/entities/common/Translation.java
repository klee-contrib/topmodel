////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.common;

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
	private String resourceKey;

	/**
	 * Valeur de la clé de traduction.
	 */
	@NotNull
	@Column("tra_value")
	private String value;

	/**
	 * Langue de traduction.
	 */
	@Id
	private String lang;

	/**
	 * Accesseur de 'resourceKey'.
	 *
	 * @return Valeur de {@link #resourceKey resourceKey}.
	 */
	public String getResourceKey() {
		return this.resourceKey;
	}

	/**
	 * Accesseur de 'value'.
	 *
	 * @return Valeur de {@link #value value}.
	 */
	public String getValue() {
		return this.value;
	}

	/**
	 * Accesseur de 'lang'.
	 *
	 * @return Valeur de {@link #lang lang}.
	 */
	public String getLang() {
		return this.lang;
	}

	/**
	 * Définit la valeur de {@link #resourceKey resourceKey}.
	 * @param resourceKey Valeur à définir.
	 */
	public void setResourceKey(String resourceKey) {
		this.resourceKey = resourceKey;
	}

	/**
	 * Définit la valeur de {@link #value value}.
	 * @param value Valeur à définir.
	 */
	public void setValue(String value) {
		this.value = value;
	}

	/**
	 * Définit la valeur de {@link #lang lang}.
	 * @param lang Valeur à définir.
	 */
	public void setLang(String lang) {
		this.lang = lang;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.common;

import java.util.Objects;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.Table;

/**
 * Table pour stocker les traductions en SQL.
 */
@Entity
@Table(name = "translation")
@IdClass(Translation.TranslationId.class)
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
	@Column(name = "tra_value", nullable = false, length = 100, columnDefinition = "varchar")
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.common.Translation Translation}.
	 */
	public enum Fields {
		RESOURCE_KEY(String.class),
		VALUE(String.class),
		LANG(String.class);

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

	public static class TranslationId {

		@Column(name = "tra_resource_key", nullable = false, length = 100, columnDefinition = "varchar")
		private String resourceKey;

		@Column(name = "tra_lang", nullable = false, length = 100, columnDefinition = "varchar")
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
		 * Définit la valeur de {@link #resourceKey resourceKey}.
		 * @param resourceKey Valeur à définir.
		 */
		public void setResourceKey(String resourceKey) {
			this.resourceKey = resourceKey;
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
		 * Définit la valeur de {@link #lang lang}.
		 * @param lang Valeur à définir.
		 */
		public void setLang(String lang) {
			this.lang = lang;
		}

		public boolean equals(Object o) {
			if (o == this) {
				return true;
			}

			if (o == null) {
				return false;
			}

			if (this.getClass() != o.getClass()) {
				return false;
			}

			TranslationId oId = (TranslationId) o;

			return Objects.equals(this.resourceKey, oId.resourceKey)
					&& Objects.equals(this.lang, oId.lang);
		}

		@Override
		public int hashCode() {
			return Objects.hash(resourceKey, lang);
		}
	}
}

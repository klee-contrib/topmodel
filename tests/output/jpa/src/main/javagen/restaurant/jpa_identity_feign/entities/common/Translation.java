////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.common;

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
@Table(name = "TRANSLATION")
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
	@Column(name = "TRA_VALUE", nullable = false, length = 100, columnDefinition = "varchar")
	private String value;

	/**
	 * Langue de traduction.
	 */
	@Id
	private String lang;

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
	 * Getter for lang.
	 *
	 * @return value of {@link #lang lang}.
	 */
	public String getLang() {
		return this.lang;
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
	 * Set the value of {@link #lang lang}.
	 * @param lang value to set.
	 */
	public void setLang(String lang) {
		this.lang = lang;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.common.Translation Translation}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}

	public static class TranslationId {

		@Column(name = "TRA_RESOURCE_KEY", nullable = false, length = 100, columnDefinition = "varchar")
		private String resourceKey;

		@Column(name = "TRA_LANG", nullable = false, length = 100, columnDefinition = "varchar")
		private String lang;

		/**
		 * Getter for resourceKey.
		 *
		 * @return value of {@link #resourceKey resourceKey}.
		 */
		public String getResourceKey() {
			return this.resourceKey;
		}

		/**
		 * Set the value of {@link #resourceKey resourceKey}.
		 * @param resourceKey value to set.
		 */
		public void setResourceKey(String resourceKey) {
			this.resourceKey = resourceKey;
		}

		/**
		 * Getter for lang.
		 *
		 * @return value of {@link #lang lang}.
		 */
		public String getLang() {
			return this.lang;
		}

		/**
		 * Set the value of {@link #lang lang}.
		 * @param lang value to set.
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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

/**
 * Boisson.
 */
@Entity
@DiscriminatorValue("BOISSON")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatBoisson extends Plat {

	/**
	 * Volume de la boisson.
	 */
	@Column(name = "pbo_volume", nullable = false, columnDefinition = "int")
	private Integer volume;

	/**
	 * Accesseur de 'volume'.
	 *
	 * @return Valeur de {@link #volume volume}.
	 */
	public Integer getVolume() {
		return this.volume;
	}

	/**
	 * Définit la valeur de {@link #volume volume}.
	 * @param volume Valeur à définir.
	 */
	public void setVolume(Integer volume) {
		this.volume = volume;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.PlatBoisson PlatBoisson}.
	 */
	public enum Fields {
		VOLUME(Integer.class);

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

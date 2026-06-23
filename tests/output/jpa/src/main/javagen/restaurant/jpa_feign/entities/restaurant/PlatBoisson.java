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
	@Column(name = "PBO_VOLUME", nullable = false, columnDefinition = "int")
	private Integer volume;

	/**
	 * Getter for volume.
	 *
	 * @return value of {@link #volume volume}.
	 */
	public Integer getVolume() {
		return this.volume;
	}

	/**
	 * Set the value of {@link #volume volume}.
	 * @param volume value to set.
	 */
	public void setVolume(Integer volume) {
		this.volume = volume;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.PlatBoisson PlatBoisson}.
	 */
	public enum Fields {
		VOLUME(Integer.class);

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

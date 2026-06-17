////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Boisson.
 */
@Table(name = "plat_boisson")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatBoisson extends Plat {

	/**
	 * Volume de la boisson.
	 */
	@NotNull
	@Column("volume")
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
}

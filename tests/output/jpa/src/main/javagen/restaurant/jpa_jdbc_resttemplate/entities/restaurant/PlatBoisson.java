////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

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
	@Column("pbo_volume")
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
}

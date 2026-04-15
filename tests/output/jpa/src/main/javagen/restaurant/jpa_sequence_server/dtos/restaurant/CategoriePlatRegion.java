////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_sequence_server.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Région.
	 */
	@NotNull
	private RegionCode regionCode;

	/**
	 * Catégorie de plat.
	 */
	@NotNull
	private CategoriePlatCode categoriePlatCode;

	/**
	 * No arg constructor.
	 */
	public CategoriePlatRegion() {
		// No arg constructor
	}

	/**
	 * Getter for regionCode.
	 *
	 * @return value of {@link #regionCode regionCode}.
	 */
	public RegionCode getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Getter for categoriePlatCode.
	 *
	 * @return value of {@link #categoriePlatCode categoriePlatCode}.
	 */
	public CategoriePlatCode getCategoriePlatCode() {
		return this.categoriePlatCode;
	}
}

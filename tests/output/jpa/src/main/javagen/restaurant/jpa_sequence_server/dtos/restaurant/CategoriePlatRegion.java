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

	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.IDF, CategoriePlatCode.DESSERT);

	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.IDF, CategoriePlatCode.ENTREE);

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
	 * All args constructor for 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlatCode Catégorie de plat.
	 */
	private CategoriePlatRegion(RegionCode regionCode, CategoriePlatCode categoriePlatCode) {
		this.regionCode = regionCode;
		this.categoriePlatCode = categoriePlatCode;
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

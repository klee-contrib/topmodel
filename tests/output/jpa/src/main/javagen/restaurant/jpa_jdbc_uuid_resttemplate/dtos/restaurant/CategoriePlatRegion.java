////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion implements Serializable {

	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.Idf, CategoriePlatCode.Dessert);

	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.Idf, CategoriePlatCode.Entree);

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Région.
	 */
	@NotNull
	@Size(max = 10)
	@Column("reg_code")
	private String regionCode;

	/**
	 * Catégorie de plat.
	 */
	@NotNull
	@Size(max = 10)
	@Column("cat_code")
	private String categoriePlatCode;

	/**
	 * All args constructor for 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlatCode Catégorie de plat.
	 */
	private CategoriePlatRegion(String regionCode, String categoriePlatCode) {
		this.regionCode = regionCode;
		this.categoriePlatCode = categoriePlatCode;
	}

	/**
	 * Getter for regionCode.
	 *
	 * @return value of {@link #regionCode regionCode}.
	 */
	public String getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Getter for categoriePlatCode.
	 *
	 * @return value of {@link #categoriePlatCode categoriePlatCode}.
	 */
	public String getCategoriePlatCode() {
		return this.categoriePlatCode;
	}
}

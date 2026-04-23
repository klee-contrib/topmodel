////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Transient;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Table(name = "categorie_plat_region")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion {

	@Transient
	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.Idf, "DESSERT");

	@Transient
	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.Idf, "ENTREE");

	/**
	 * Région.
	 */
	@Id
	private String regionCode;

	/**
	 * Catégorie de plat.
	 */
	@Id
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

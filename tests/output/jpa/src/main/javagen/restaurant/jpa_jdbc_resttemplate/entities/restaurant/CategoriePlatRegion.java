////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Transient;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_resttemplate.enums.restaurant.RegionCode;

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
	 * Liste de toutes les valeurs de l'énumération CategoriePlatRegion.
	 */
	public static final List<CategoriePlatRegion> VALUES = List.of(IDF_ENTREE, IDF_DESSERT);

	/**
	 * Région.
	 */
	@Id
	private String regionCode;

	/**
	 * Catégorie de plat.
	 */
	@Id
	private String categoriePlat;

	/**
	 * All args constructor for 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlat Catégorie de plat.
	 */
	private CategoriePlatRegion(String regionCode, String categoriePlat) {
		this.regionCode = regionCode;
		this.categoriePlat = categoriePlat;
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
	 * Getter for categoriePlat.
	 *
	 * @return value of {@link #categoriePlat categoriePlat}.
	 */
	public String getCategoriePlat() {
		return this.categoriePlat;
	}
}

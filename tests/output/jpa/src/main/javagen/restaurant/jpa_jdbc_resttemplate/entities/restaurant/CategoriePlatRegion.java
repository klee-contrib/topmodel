////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_resttemplate.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Table(name = "categorie_plat_region")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion {

	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.Idf, CategoriePlatCode.Dessert);

	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.Idf, CategoriePlatCode.Entree);

	/**
	 * Liste de toutes les valeurs de l'énumération 'CategoriePlatRegion'.
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
	 * Constructeur tout arguments pour 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlat Catégorie de plat.
	 */
	private CategoriePlatRegion(String regionCode, String categoriePlat) {
		this.regionCode = regionCode;
		this.categoriePlat = categoriePlat;
	}

	/**
	 * Accesseur de 'regionCode'.
	 *
	 * @return Valeur de {@link #regionCode regionCode}.
	 */
	public String getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Accesseur de 'categoriePlat'.
	 *
	 * @return Valeur de {@link #categoriePlat categoriePlat}.
	 */
	public String getCategoriePlat() {
		return this.categoriePlat;
	}
}

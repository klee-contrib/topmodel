////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

/**
 * Détail d'un plat en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PlatItem {

	/**
	 * Identifiant du plat.
	 */
	Integer getId();

	/**
	 * Nom du plat.
	 */
	String getNom();

	/**
	 * Prix du plat.
	 */
	BigDecimal getPrix();

	/**
	 * Indique si le plat est disponible.
	 */
	Boolean getDisponible();

	/**
	 * Catégorie du plat.
	 */
	String getCategoriePlatCodeCategoriePlat();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prix value to set.
	 * @param disponible value to set.
	 * @param categoriePlatCodeCategoriePlat value to set.
	 */
	void hydrate(Integer id, String nom, BigDecimal prix, Boolean disponible, String categoriePlatCodeCategoriePlat);
}

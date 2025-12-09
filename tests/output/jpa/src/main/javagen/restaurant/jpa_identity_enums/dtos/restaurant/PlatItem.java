////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

import restaurant.jpa_identity_enums.enums.restaurant.CategoriePlat;

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
	CategoriePlat getCategoriePlatCodeCategoriePlat();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prix value to set.
	 * @param disponible value to set.
	 * @param categoriePlatCodeCategoriePlat value to set.
	 */
	void hydrate(Integer id, String nom, BigDecimal prix, Boolean disponible, CategoriePlat categoriePlatCodeCategoriePlat);
}

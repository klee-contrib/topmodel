////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

import restaurant.jpa_feign.enums.restaurant.CategoriePlatCode;

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
	CategoriePlatCode getCategoriePlatCode();

	/**
	 * Identifiant du plat.
	 * @param id value to set.
	 */
	void setId(Integer id);

	/**
	 * Nom du plat.
	 * @param nom value to set.
	 */
	void setNom(String nom);

	/**
	 * Prix du plat.
	 * @param prix value to set.
	 */
	void setPrix(BigDecimal prix);

	/**
	 * Indique si le plat est disponible.
	 * @param disponible value to set.
	 */
	void setDisponible(Boolean disponible);

	/**
	 * Catégorie du plat.
	 * @param categoriePlatCode value to set.
	 */
	void setCategoriePlatCode(CategoriePlatCode categoriePlatCode);
}

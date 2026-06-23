////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

import restaurant.jpa_server.enums.restaurant.CategoriePlatCode;

/**
 * Détail d'un plat en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PlatItemReadonly {

	/**
	 * Identifiant du plat.
	 */
	Integer getId();

	/**
	 * Nom du plat.
	 */
	String getNom();

	/**
	 * Catégorie du plat.
	 */
	CategoriePlatCode getCategoriePlatCode();

	/**
	 * Prix du plat.
	 */
	BigDecimal getPrix();

	/**
	 * Indique si le plat est disponible.
	 */
	Boolean getDisponible();

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
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;

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
	String getCategoriePlatCode();

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

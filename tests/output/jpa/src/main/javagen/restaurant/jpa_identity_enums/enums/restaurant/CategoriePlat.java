////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.enums.restaurant;

import jakarta.annotation.Generated;

/**
 * Enumération des valeurs possibles de la classe CategoriePlat.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public enum CategoriePlat {
	/**
	 * Entrée.
	 */
	ENTREE("restaurant.categoriePlat.values.Entree"),

	/**
	 * Plat principal.
	 */
	PLAT("restaurant.categoriePlat.values.Plat"),

	/**
	 * Dessert.
	 */
	DESSERT("restaurant.categoriePlat.values.Dessert"),

	/**
	 * Boisson.
	 */
	BOISSON("restaurant.categoriePlat.values.Boisson"),

	;

	/**
	 * Libelle.
	 */
	private final String libelle;

	/**
	 * Enum values constructor.
	 */
	CategoriePlat(final String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Getter for libelle.
	 */
	public String getLibelle() {
		return this.libelle;
	}
}

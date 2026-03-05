////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.enums.restaurant;

import jakarta.annotation.Generated;

/**
 * Enumération des valeurs possibles de la classe StatutCommande.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public enum StatutCommande {
	/**
	 * En attente.
	 */
	EN_ATT("En attente"),

	/**
	 * En préparation.
	 */
	EN_PREP("En préparation"),

	/**
	 * Prête.
	 */
	PRETE("Prête"),

	/**
	 * Servie.
	 */
	SERVIE("Servie"),

	/**
	 * Annulée.
	 */
	ANNULE("Annulée"),

	;

	/**
	 * Libelle.
	 */
	private final String libelle;

	/**
	 * Enum values constructor.
	 */
	StatutCommande(final String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Getter for libelle.
	 */
	public String getLibelle() {
		return this.libelle;
	}
}

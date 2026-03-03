////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.enums.restaurant;

import jakarta.annotation.Generated;

/**
 * Enumération des valeurs possibles de la classe StatutCommande.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public enum StatutCommande {
	/**
	 * En attente.
	 */
	EN_ATT("restaurant.statutCommande.values.EnAttente"),

	/**
	 * En préparation.
	 */
	EN_PREP("restaurant.statutCommande.values.EnPreparation"),

	/**
	 * Prête.
	 */
	PRETE("restaurant.statutCommande.values.Prete"),

	/**
	 * Servie.
	 */
	SERVIE("restaurant.statutCommande.values.Servie"),

	/**
	 * Annulée.
	 */
	ANNULE("restaurant.statutCommande.values.Annulee"),

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

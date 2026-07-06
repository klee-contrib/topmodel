////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.enums.restaurant;

import jakarta.annotation.Generated;

/**
 * Statut d'une commande.
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
	ANNULE("Annulée");

	/**
	 * Libellé du statut.
	 */
	private String libelle;

	/**
	 * All args constructor for 'StatutCommande'.
	 * @param libelle Libellé du statut.
	 */
	private StatutCommande(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}
}

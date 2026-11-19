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
	 * Annulée.
	 */
	ANNULE("Annulée"),

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
	SERVIE("Servie");

	/**
	 * Libellé du statut.
	 */
	private String libelle;

	/**
	 * Constructeur tout arguments pour 'StatutCommande'.
	 * @param libelle Libellé du statut.
	 */
	private StatutCommande(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Accesseur de 'libelle'.
	 *
	 * @return Valeur de {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant;

import jakarta.annotation.Generated;

/**
 * Statut d'une commande.
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
	ANNULE("restaurant.statutCommande.values.Annulee");

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

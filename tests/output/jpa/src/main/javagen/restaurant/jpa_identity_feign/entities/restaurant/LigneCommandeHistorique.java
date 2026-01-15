////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Ligne de commande pour historique avec préservation des clés primaires.
 */
@Entity
@Table(name = "LIGNE_COMMANDE_HISTORIQUE")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeHistorique {

	/**
	 * Identifiant de la ligne.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@Id
	@Column(name = "LIG_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Quantité commandée.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@Column(name = "LIG_QUANTITE", nullable = false, columnDefinition = "int")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@Column(name = "LIG_PRIX_UNITAIRE", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@Column(name = "LIG_PRIX_TOTAL", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixTotal;

	/**
	 * Plat commandé.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@Column(name = "PLA_ID", nullable = false, columnDefinition = "int")
	private Integer platId;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@Column(name = "COM_ID", nullable = false, columnDefinition = "int")
	private Integer commandeHistoriqueId;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for quantite.
	 *
	 * @return value of {@link #quantite quantite}.
	 */
	public Integer getQuantite() {
		return this.quantite;
	}

	/**
	 * Getter for prixUnitaire.
	 *
	 * @return value of {@link #prixUnitaire prixUnitaire}.
	 */
	public BigDecimal getPrixUnitaire() {
		return this.prixUnitaire;
	}

	/**
	 * Getter for prixTotal.
	 *
	 * @return value of {@link #prixTotal prixTotal}.
	 */
	public BigDecimal getPrixTotal() {
		return this.prixTotal;
	}

	/**
	 * Getter for platId.
	 *
	 * @return value of {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Getter for commandeHistoriqueId.
	 *
	 * @return value of {@link #commandeHistoriqueId commandeHistoriqueId}.
	 */
	public Integer getCommandeHistoriqueId() {
		return this.commandeHistoriqueId;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #quantite quantite}.
	 * @param quantite value to set.
	 */
	public void setQuantite(Integer quantite) {
		this.quantite = quantite;
	}

	/**
	 * Set the value of {@link #prixUnitaire prixUnitaire}.
	 * @param prixUnitaire value to set.
	 */
	public void setPrixUnitaire(BigDecimal prixUnitaire) {
		this.prixUnitaire = prixUnitaire;
	}

	/**
	 * Set the value of {@link #prixTotal prixTotal}.
	 * @param prixTotal value to set.
	 */
	public void setPrixTotal(BigDecimal prixTotal) {
		this.prixTotal = prixTotal;
	}

	/**
	 * Set the value of {@link #platId platId}.
	 * @param platId value to set.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Set the value of {@link #commandeHistoriqueId commandeHistoriqueId}.
	 * @param commandeHistoriqueId value to set.
	 */
	public void setCommandeHistoriqueId(Integer commandeHistoriqueId) {
		this.commandeHistoriqueId = commandeHistoriqueId;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommandeHistorique LigneCommandeHistorique}.
	 */
	public enum Fields {
		ID(Integer.class),
		QUANTITE(Integer.class),
		PRIX_UNITAIRE(BigDecimal.class),
		PRIX_TOTAL(BigDecimal.class),
		PLAT_ID(Integer.class),
		COMMANDE_HISTORIQUE_ID(Integer.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

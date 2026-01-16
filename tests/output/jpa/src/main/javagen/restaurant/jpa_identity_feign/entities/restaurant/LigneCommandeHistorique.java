////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
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
	@JoinColumn(name = "PLA_ID", referencedColumnName = "PLA_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
	private Plat plat;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@JoinColumn(name = "COM_ID", referencedColumnName = "COM_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CommandeHistorique.class)
	private CommandeHistorique commandeHistorique;

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
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
	}

	/**
	 * Getter for commandeHistorique.
	 *
	 * @return value of {@link #commandeHistorique commandeHistorique}.
	 */
	public CommandeHistorique getCommandeHistorique() {
		return this.commandeHistorique;
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
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Set the value of {@link #commandeHistorique commandeHistorique}.
	 * @param commandeHistorique value to set.
	 */
	public void setCommandeHistorique(CommandeHistorique commandeHistorique) {
		this.commandeHistorique = commandeHistorique;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommandeHistorique LigneCommandeHistorique}.
	 */
	public enum Fields {
		ID(Integer.class),
		QUANTITE(Integer.class),
		PRIX_UNITAIRE(BigDecimal.class),
		PRIX_TOTAL(BigDecimal.class),
		PLAT(Plat.class),
		COMMANDE_HISTORIQUE(CommandeHistorique.class);

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

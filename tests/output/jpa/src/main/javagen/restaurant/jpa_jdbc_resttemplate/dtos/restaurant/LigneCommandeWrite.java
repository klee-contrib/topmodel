////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Détail d'une ligne de commande en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Quantité commandée.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@NotNull
	@Column("lig_quantite")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@NotNull
	@Column("lig_prix_unitaire")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@NotNull
	@Column("lig_prix_total")
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getCommande() LigneCommande#getCommande()}
	 */
	@NotNull
	@Column("com_id")
	private Integer commandeId;

	/**
	 * Plat commandé.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@NotNull
	@Column("pla_id")
	private Integer platId;

	/**
	 * Accesseur de 'quantite'.
	 *
	 * @return Valeur de {@link #quantite quantite}.
	 */
	public Integer getQuantite() {
		return this.quantite;
	}

	/**
	 * Accesseur de 'prixUnitaire'.
	 *
	 * @return Valeur de {@link #prixUnitaire prixUnitaire}.
	 */
	public BigDecimal getPrixUnitaire() {
		return this.prixUnitaire;
	}

	/**
	 * Accesseur de 'prixTotal'.
	 *
	 * @return Valeur de {@link #prixTotal prixTotal}.
	 */
	public BigDecimal getPrixTotal() {
		return this.prixTotal;
	}

	/**
	 * Accesseur de 'commandeId'.
	 *
	 * @return Valeur de {@link #commandeId commandeId}.
	 */
	public Integer getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Accesseur de 'platId'.
	 *
	 * @return Valeur de {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Définit la valeur de {@link #quantite quantite}.
	 * @param quantite Valeur à définir.
	 */
	public void setQuantite(Integer quantite) {
		this.quantite = quantite;
	}

	/**
	 * Définit la valeur de {@link #prixUnitaire prixUnitaire}.
	 * @param prixUnitaire Valeur à définir.
	 */
	public void setPrixUnitaire(BigDecimal prixUnitaire) {
		this.prixUnitaire = prixUnitaire;
	}

	/**
	 * Définit la valeur de {@link #prixTotal prixTotal}.
	 * @param prixTotal Valeur à définir.
	 */
	public void setPrixTotal(BigDecimal prixTotal) {
		this.prixTotal = prixTotal;
	}

	/**
	 * Définit la valeur de {@link #commandeId commandeId}.
	 * @param commandeId Valeur à définir.
	 */
	public void setCommandeId(Integer commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Définit la valeur de {@link #platId platId}.
	 * @param platId Valeur à définir.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}
}

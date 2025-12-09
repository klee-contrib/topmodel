////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import java.math.BigDecimal;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Ligne d'une commande.
 */
@Table(name = "ligne_commande")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommande {

	/**
	 * Identifiant de la ligne.
	 */
	@Id
	@Column("lig_id")
	private Integer id;

	/**
	 * Quantité commandée.
	 */
	@NotNull
	@Column("lig_quantite")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 */
	@NotNull
	@Column("lig_prix_unitaire")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 */
	@NotNull
	@Column("lig_prix_total")
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@Column("com_id")
	private Integer commandeId;

	/**
	 * Plat commandé.
	 */
	@Column("pla_id")
	private Integer platId;

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
	 * Getter for commandeId.
	 *
	 * @return value of {@link #commandeId commandeId}.
	 */
	public Integer getCommandeId() {
		return this.commandeId;
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
	 * Set the value of {@link #commandeId commandeId}.
	 * @param commandeId value to set.
	 */
	public void setCommandeId(Integer commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Set the value of {@link #platId platId}.
	 * @param platId value to set.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}
}

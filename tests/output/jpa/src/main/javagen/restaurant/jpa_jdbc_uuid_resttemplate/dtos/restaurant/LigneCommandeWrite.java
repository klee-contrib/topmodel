////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande;
import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.RestaurantMappers;

/**
 * Détail d'une ligne de commande en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Quantité commandée.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@NotNull
	@Column("lig_quantite")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@NotNull
	@Column("lig_prix_unitaire")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@NotNull
	@Column("lig_prix_total")
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande#getCommandeId() LigneCommande#getCommandeId()}
	 */
	@NotNull
	@Column("com_id")
	private Integer commandeId;

	/**
	 * Plat commandé.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.LigneCommande#getPlatId() LigneCommande#getPlatId()}
	 */
	@NotNull
	@Column("pla_id")
	private Integer platId;

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

	/**
	 * Mappe 'LigneCommandeWrite' vers 'LigneCommande'.
	 * @param target Instance pré-existante de 'LigneCommande'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'LigneCommande'.
	 */
	public LigneCommande toLigneCommande(LigneCommande target) {
		return RestaurantMappers.toLigneCommande(this, target);
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Ligne de commande pour historique avec préservation des clés primaires.
 */
@Table(name = "ligne_commande_historique")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeHistorique {

	/**
	 * Identifiant de la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@Id
	@Column("lig_id")
	private Integer id;

	/**
	 * Quantité commandée.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@NotNull
	@Column("lig_quantite")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@NotNull
	@Column("lig_prix_unitaire")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@NotNull
	@Column("lig_prix_total")
	private BigDecimal prixTotal;

	/**
	 * Plat commandé.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@NotNull
	@Column("pla_id")
	private Integer platId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getDateCreation() LigneCommande#getDateCreation()}
	 */
	@NotNull
	@CreatedDate
	@Column("lig_date_creation")
	private LocalDateTime dateCreation;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@NotNull
	@Column("com_id")
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
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
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
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link #commandeHistoriqueId commandeHistoriqueId}.
	 * @param commandeHistoriqueId value to set.
	 */
	public void setCommandeHistoriqueId(Integer commandeHistoriqueId) {
		this.commandeHistoriqueId = commandeHistoriqueId;
	}
}

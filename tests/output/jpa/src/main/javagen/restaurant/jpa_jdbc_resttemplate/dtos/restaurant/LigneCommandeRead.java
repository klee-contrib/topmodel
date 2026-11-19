////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;

/**
 * Détail d'une ligne de commande en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@NotNull
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
	 * Commande à laquelle appartient la ligne.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getCommande() LigneCommande#getCommande()}
	 */
	@NotNull
	@Column("com_id")
	private Integer commandeId;

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
	@PastOrPresent
	@Column("lig_date_creation")
	private LocalDateTime dateCreation;

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
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
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

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

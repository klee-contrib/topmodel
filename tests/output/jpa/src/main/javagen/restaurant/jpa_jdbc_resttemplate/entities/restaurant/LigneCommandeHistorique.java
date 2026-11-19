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
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@Id
	@Column("lig_id")
	private Integer id;

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
	 * Plat commandé.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@NotNull
	@Column("pla_id")
	private Integer platId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.LigneCommande#getDateCreation() LigneCommande#getDateCreation()}
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
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Accesseur de 'platId'.
	 *
	 * @return Valeur de {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Accesseur de 'commandeHistoriqueId'.
	 *
	 * @return Valeur de {@link #commandeHistoriqueId commandeHistoriqueId}.
	 */
	public Integer getCommandeHistoriqueId() {
		return this.commandeHistoriqueId;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Définit la valeur de {@link #platId platId}.
	 * @param platId Valeur à définir.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #commandeHistoriqueId commandeHistoriqueId}.
	 * @param commandeHistoriqueId Valeur à définir.
	 */
	public void setCommandeHistoriqueId(Integer commandeHistoriqueId) {
		this.commandeHistoriqueId = commandeHistoriqueId;
	}
}

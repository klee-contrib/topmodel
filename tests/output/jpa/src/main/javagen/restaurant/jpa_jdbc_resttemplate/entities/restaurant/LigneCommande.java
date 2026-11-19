////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

/**
 * Ligne d'une commande.
 */
@Table(name = "ligne_commande")
@EntityListeners(AuditingEntityListener.class)
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
	@NotNull
	@Column("com_id")
	private Integer commande;

	/**
	 * Plat commandé.
	 */
	@NotNull
	@Column("pla_id")
	private Integer plat;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
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
	 * Getter for commande.
	 *
	 * @return value of {@link #commande commande}.
	 */
	public Integer getCommande() {
		return this.commande;
	}

	/**
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Integer getPlat() {
		return this.plat;
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
	 * Set the value of {@link #commande commande}.
	 * @param commande value to set.
	 */
	public void setCommande(Integer commande) {
		this.commande = commande;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Integer plat) {
		this.plat = plat;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

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
	 * Accesseur de 'commande'.
	 *
	 * @return Valeur de {@link #commande commande}.
	 */
	public Integer getCommande() {
		return this.commande;
	}

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Integer getPlat() {
		return this.plat;
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
	 * Définit la valeur de {@link #commande commande}.
	 * @param commande Valeur à définir.
	 */
	public void setCommande(Integer commande) {
		this.commande = commande;
	}

	/**
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Integer plat) {
		this.plat = plat;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

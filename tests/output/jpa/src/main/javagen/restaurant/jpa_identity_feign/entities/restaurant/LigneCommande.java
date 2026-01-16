////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Ligne d'une commande.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "LIGNE_COMMANDE", uniqueConstraints = {@UniqueConstraint(columnNames = {"COM_ID", "PLA_ID"})})
public class LigneCommande {

	/**
	 * Identifiant de la ligne.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "LIG_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Quantité commandée.
	 */
	@Column(name = "LIG_QUANTITE", nullable = false, columnDefinition = "int")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 */
	@Column(name = "LIG_PRIX_UNITAIRE", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 */
	@Column(name = "LIG_PRIX_TOTAL", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@JoinColumn(name = "COM_ID", referencedColumnName = "COM_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Commande.class)
	private Commande commande;

	/**
	 * Plat commandé.
	 */
	@JoinColumn(name = "PLA_ID", referencedColumnName = "PLA_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
	private Plat plat;

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
	public Commande getCommande() {
		return this.commande;
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
	public void setCommande(Commande commande) {
		this.commande = commande;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.LigneCommande LigneCommande}.
	 */
	public enum Fields {
		ID(Integer.class),
		QUANTITE(Integer.class),
		PRIX_UNITAIRE(BigDecimal.class),
		PRIX_TOTAL(BigDecimal.class),
		COMMANDE(Commande.class),
		PLAT(Plat.class);

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

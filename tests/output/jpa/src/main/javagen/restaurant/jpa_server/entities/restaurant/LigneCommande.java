////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
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
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "ligne_commande",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"com_id", "pla_id"})
	}
)
public class LigneCommande {

	/**
	 * Identifiant de la ligne.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "lig_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Quantité commandée.
	 */
	@Column(name = "lig_quantite", nullable = false, columnDefinition = "int")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 */
	@Column(name = "lig_prix_unitaire", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 */
	@Column(name = "lig_prix_total", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@JoinColumn(name = "com_id", referencedColumnName = "com_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Commande.class)
	private Commande commande;

	/**
	 * Plat commandé.
	 */
	@JoinColumn(name = "pla_id", referencedColumnName = "pla_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
	private Plat plat;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "lig_date_creation", nullable = false, columnDefinition = "timestamp")
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
	public Commande getCommande() {
		return this.commande;
	}

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Plat getPlat() {
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
	public void setCommande(Commande commande) {
		this.commande = commande;
	}

	/**
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.LigneCommande LigneCommande}.
	 */
	public enum Fields {
		ID(Integer.class),
		QUANTITE(Integer.class),
		PRIX_UNITAIRE(BigDecimal.class),
		PRIX_TOTAL(BigDecimal.class),
		COMMANDE(Commande.class),
		PLAT(Plat.class),
		DATE_CREATION(LocalDateTime.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

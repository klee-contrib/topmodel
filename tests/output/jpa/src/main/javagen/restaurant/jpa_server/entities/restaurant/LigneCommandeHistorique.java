////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Ligne de commande pour historique avec préservation des clés primaires.
 */
@Entity
@Table(name = "ligne_commande_historique")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeHistorique {

	/**
	 * Identifiant de la ligne.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@Id
	@Column(name = "lig_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Quantité commandée.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@Column(name = "lig_quantite", nullable = false, columnDefinition = "int")
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@Column(name = "lig_prix_unitaire", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@Column(name = "lig_prix_total", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prixTotal;

	/**
	 * Plat commandé.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@Column(name = "pla_id", nullable = false, columnDefinition = "int")
	private Integer platId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getDateCreation() LigneCommande#getDateCreation()}
	 */
	@CreatedDate
	@Column(name = "lig_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Commande à laquelle appartient la ligne.
	 */
	@Column(name = "com_id", nullable = false, columnDefinition = "int")
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.LigneCommandeHistorique LigneCommandeHistorique}.
	 */
	public enum Fields {
		ID(Integer.class),
		QUANTITE(Integer.class),
		PRIX_UNITAIRE(BigDecimal.class),
		PRIX_TOTAL(BigDecimal.class),
		PLAT_ID(Integer.class),
		DATE_CREATION(LocalDateTime.class),
		COMMANDE_HISTORIQUE_ID(Integer.class);

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

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

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.StatutCommande;

/**
 * Commande pour historique avec préservation des clés primaires.
 */
@Table(name = "commande_historique")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique {

	/**
	 * Identifiant de la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@Id
	@Column("com_id")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	@Column("com_date_commande")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column("com_date_livraison")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	@Column("com_montant_total")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	@Column("per_id")
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column("tab_id")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	@Column("rev_id")
	private Integer reservationId;

	/**
	 * Statut de la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	@Column("stc_code")
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	@Column("avi_id")
	private Integer avisClientId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@CreatedDate
	@Column("com_date_creation")
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
	 * Accesseur de 'dateCommande'.
	 *
	 * @return Valeur de {@link #dateCommande dateCommande}.
	 */
	public LocalDateTime getDateCommande() {
		return this.dateCommande;
	}

	/**
	 * Accesseur de 'dateLivraison'.
	 *
	 * @return Valeur de {@link #dateLivraison dateLivraison}.
	 */
	public LocalDateTime getDateLivraison() {
		return this.dateLivraison;
	}

	/**
	 * Accesseur de 'montantTotal'.
	 *
	 * @return Valeur de {@link #montantTotal montantTotal}.
	 */
	public BigDecimal getMontantTotal() {
		return this.montantTotal;
	}

	/**
	 * Accesseur de 'clientId'.
	 *
	 * @return Valeur de {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Accesseur de 'tableId'.
	 *
	 * @return Valeur de {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
	}

	/**
	 * Accesseur de 'reservationId'.
	 *
	 * @return Valeur de {@link #reservationId reservationId}.
	 */
	public Integer getReservationId() {
		return this.reservationId;
	}

	/**
	 * Accesseur de 'statutCommande'.
	 *
	 * @return Valeur de {@link #statutCommande statutCommande}.
	 */
	public StatutCommande getStatutCommande() {
		return this.statutCommande;
	}

	/**
	 * Accesseur de 'avisClientId'.
	 *
	 * @return Valeur de {@link #avisClientId avisClientId}.
	 */
	public Integer getAvisClientId() {
		return this.avisClientId;
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
	 * Définit la valeur de {@link #dateCommande dateCommande}.
	 * @param dateCommande Valeur à définir.
	 */
	public void setDateCommande(LocalDateTime dateCommande) {
		this.dateCommande = dateCommande;
	}

	/**
	 * Définit la valeur de {@link #dateLivraison dateLivraison}.
	 * @param dateLivraison Valeur à définir.
	 */
	public void setDateLivraison(LocalDateTime dateLivraison) {
		this.dateLivraison = dateLivraison;
	}

	/**
	 * Définit la valeur de {@link #montantTotal montantTotal}.
	 * @param montantTotal Valeur à définir.
	 */
	public void setMontantTotal(BigDecimal montantTotal) {
		this.montantTotal = montantTotal;
	}

	/**
	 * Définit la valeur de {@link #clientId clientId}.
	 * @param clientId Valeur à définir.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Définit la valeur de {@link #reservationId reservationId}.
	 * @param reservationId Valeur à définir.
	 */
	public void setReservationId(Integer reservationId) {
		this.reservationId = reservationId;
	}

	/**
	 * Définit la valeur de {@link #statutCommande statutCommande}.
	 * @param statutCommande Valeur à définir.
	 */
	public void setStatutCommande(StatutCommande statutCommande) {
		this.statutCommande = statutCommande;
	}

	/**
	 * Définit la valeur de {@link #avisClientId avisClientId}.
	 * @param avisClientId Valeur à définir.
	 */
	public void setAvisClientId(Integer avisClientId) {
		this.avisClientId = avisClientId;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

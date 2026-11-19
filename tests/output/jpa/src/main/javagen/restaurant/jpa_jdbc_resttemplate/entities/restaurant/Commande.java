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

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.StatutCommande;

/**
 * Commande d'un client.
 */
@Table(name = "commande")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Commande {

	/**
	 * Identifiant de la commande.
	 */
	@Id
	@Column("com_id")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 */
	@NotNull
	@Column("com_date_commande")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 */
	@Column("com_date_livraison")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 */
	@NotNull
	@Column("com_montant_total")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 */
	@NotNull
	@Column("per_id")
	private Integer client;

	/**
	 * Table associée à la commande.
	 */
	@Column("tab_id")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 */
	@Column("rev_id")
	private Integer reservation;

	/**
	 * Statut de la commande.
	 */
	@NotNull
	@Column("stc_code")
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 */
	@Column("avi_id")
	private Integer avisClient;

	/**
	 * Date de création de l'enregistrement.
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
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public Integer getClient() {
		return this.client;
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
	 * Accesseur de 'reservation'.
	 *
	 * @return Valeur de {@link #reservation reservation}.
	 */
	public Integer getReservation() {
		return this.reservation;
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
	 * Accesseur de 'avisClient'.
	 *
	 * @return Valeur de {@link #avisClient avisClient}.
	 */
	public Integer getAvisClient() {
		return this.avisClient;
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
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(Integer client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Définit la valeur de {@link #reservation reservation}.
	 * @param reservation Valeur à définir.
	 */
	public void setReservation(Integer reservation) {
		this.reservation = reservation;
	}

	/**
	 * Définit la valeur de {@link #statutCommande statutCommande}.
	 * @param statutCommande Valeur à définir.
	 */
	public void setStatutCommande(StatutCommande statutCommande) {
		this.statutCommande = statutCommande;
	}

	/**
	 * Définit la valeur de {@link #avisClient avisClient}.
	 * @param avisClient Valeur à définir.
	 */
	public void setAvisClient(Integer avisClient) {
		this.avisClient = avisClient;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

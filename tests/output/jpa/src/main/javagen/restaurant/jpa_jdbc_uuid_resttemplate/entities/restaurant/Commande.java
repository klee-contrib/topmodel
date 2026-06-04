////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

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

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.StatutCommande;

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
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for dateCommande.
	 *
	 * @return value of {@link #dateCommande dateCommande}.
	 */
	public LocalDateTime getDateCommande() {
		return this.dateCommande;
	}

	/**
	 * Getter for dateLivraison.
	 *
	 * @return value of {@link #dateLivraison dateLivraison}.
	 */
	public LocalDateTime getDateLivraison() {
		return this.dateLivraison;
	}

	/**
	 * Getter for montantTotal.
	 *
	 * @return value of {@link #montantTotal montantTotal}.
	 */
	public BigDecimal getMontantTotal() {
		return this.montantTotal;
	}

	/**
	 * Getter for client.
	 *
	 * @return value of {@link #client client}.
	 */
	public Integer getClient() {
		return this.client;
	}

	/**
	 * Getter for tableId.
	 *
	 * @return value of {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
	}

	/**
	 * Getter for reservation.
	 *
	 * @return value of {@link #reservation reservation}.
	 */
	public Integer getReservation() {
		return this.reservation;
	}

	/**
	 * Getter for statutCommande.
	 *
	 * @return value of {@link #statutCommande statutCommande}.
	 */
	public StatutCommande getStatutCommande() {
		return this.statutCommande;
	}

	/**
	 * Getter for avisClient.
	 *
	 * @return value of {@link #avisClient avisClient}.
	 */
	public Integer getAvisClient() {
		return this.avisClient;
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
	 * Set the value of {@link #dateCommande dateCommande}.
	 * @param dateCommande value to set.
	 */
	public void setDateCommande(LocalDateTime dateCommande) {
		this.dateCommande = dateCommande;
	}

	/**
	 * Set the value of {@link #dateLivraison dateLivraison}.
	 * @param dateLivraison value to set.
	 */
	public void setDateLivraison(LocalDateTime dateLivraison) {
		this.dateLivraison = dateLivraison;
	}

	/**
	 * Set the value of {@link #montantTotal montantTotal}.
	 * @param montantTotal value to set.
	 */
	public void setMontantTotal(BigDecimal montantTotal) {
		this.montantTotal = montantTotal;
	}

	/**
	 * Set the value of {@link #client client}.
	 * @param client value to set.
	 */
	public void setClient(Integer client) {
		this.client = client;
	}

	/**
	 * Set the value of {@link #tableId tableId}.
	 * @param tableId value to set.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Set the value of {@link #reservation reservation}.
	 * @param reservation value to set.
	 */
	public void setReservation(Integer reservation) {
		this.reservation = reservation;
	}

	/**
	 * Set the value of {@link #statutCommande statutCommande}.
	 * @param statutCommande value to set.
	 */
	public void setStatutCommande(StatutCommande statutCommande) {
		this.statutCommande = statutCommande;
	}

	/**
	 * Set the value of {@link #avisClient avisClient}.
	 * @param avisClient value to set.
	 */
	public void setAvisClient(Integer avisClient) {
		this.avisClient = avisClient;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

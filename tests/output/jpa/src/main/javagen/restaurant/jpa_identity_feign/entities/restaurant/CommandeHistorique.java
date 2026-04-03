////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import restaurant.jpa_identity_feign.enums.restaurant.StatutCommande;

/**
 * Commande pour historique avec préservation des clés primaires.
 */
@Entity
@Table(name = "COMMANDE_HISTORIQUE")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique {

	/**
	 * Identifiant de la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@Id
	@Column(columnDefinition = "int", name = "COM_ID", nullable = false)
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@Column(columnDefinition = "timestamp", name = "COM_DATE_COMMANDE", nullable = false)
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column(columnDefinition = "timestamp", name = "COM_DATE_LIVRAISON")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@Column(columnDefinition = "decimal", name = "COM_MONTANT_TOTAL", nullable = false, scale = 2)
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@Column(columnDefinition = "int", name = "PER_ID", nullable = false)
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column(columnDefinition = "int", name = "TAB_ID")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	@Column(columnDefinition = "int", name = "REV_ID")
	private Integer reservationId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@Enumerated(EnumType.STRING)
	@Column(columnDefinition = "varchar", length = 10, name = "STC_CODE", nullable = false)
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	@Column(columnDefinition = "int", name = "AVI_ID")
	private Integer avisClientId;

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
	 * Getter for clientId.
	 *
	 * @return value of {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
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
	 * Getter for reservationId.
	 *
	 * @return value of {@link #reservationId reservationId}.
	 */
	public Integer getReservationId() {
		return this.reservationId;
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
	 * Getter for avisClientId.
	 *
	 * @return value of {@link #avisClientId avisClientId}.
	 */
	public Integer getAvisClientId() {
		return this.avisClientId;
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
	 * Set the value of {@link #clientId clientId}.
	 * @param clientId value to set.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Set the value of {@link #tableId tableId}.
	 * @param tableId value to set.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Set the value of {@link #reservationId reservationId}.
	 * @param reservationId value to set.
	 */
	public void setReservationId(Integer reservationId) {
		this.reservationId = reservationId;
	}

	/**
	 * Set the value of {@link #statutCommande statutCommande}.
	 * @param statutCommande value to set.
	 */
	public void setStatutCommande(StatutCommande statutCommande) {
		this.statutCommande = statutCommande;
	}

	/**
	 * Set the value of {@link #avisClientId avisClientId}.
	 * @param avisClientId value to set.
	 */
	public void setAvisClientId(Integer avisClientId) {
		this.avisClientId = avisClientId;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.CommandeHistorique CommandeHistorique}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		CLIENT_ID(Integer.class),
		TABLE_ID(Integer.class),
		RESERVATION_ID(Integer.class),
		STATUT_COMMANDE(StatutCommande.class),
		AVIS_CLIENT_ID(Integer.class);

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

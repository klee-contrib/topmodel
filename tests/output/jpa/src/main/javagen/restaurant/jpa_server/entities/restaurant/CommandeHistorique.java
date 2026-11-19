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
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import restaurant.jpa_server.enums.restaurant.StatutCommande;

/**
 * Commande pour historique avec préservation des clés primaires.
 */
@Entity
@Table(name = "commande_historique")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique {

	/**
	 * Identifiant de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@Id
	@Column(name = "com_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@Column(name = "com_date_commande", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column(name = "com_date_livraison", columnDefinition = "timestamp")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@Column(name = "com_montant_total", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@Column(name = "per_id", nullable = false, columnDefinition = "int")
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column(name = "tab_id", columnDefinition = "int")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	@Column(name = "rev_id", columnDefinition = "int")
	private Integer reservationId;

	/**
	 * Statut de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "stc_code", nullable = false, length = 10, columnDefinition = "varchar")
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	@Column(name = "avi_id", columnDefinition = "int")
	private Integer avisClientId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@CreatedDate
	@Column(name = "com_date_creation", nullable = false, columnDefinition = "timestamp")
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.CommandeHistorique CommandeHistorique}.
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
		AVIS_CLIENT_ID(Integer.class),
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

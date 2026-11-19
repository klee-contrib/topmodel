////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;

import restaurant.jpa_server.enums.restaurant.StatutCommande;

/**
 * Commande d'un client.
 */
@Entity
@Table(name = "commande")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Commande {

	/**
	 * Identifiant de la commande.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "com_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 */
	@Column(name = "com_date_commande", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 */
	@Column(name = "com_date_livraison", columnDefinition = "timestamp")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 */
	@Column(name = "com_montant_total", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 */
	@JoinColumn(name = "per_id", referencedColumnName = "per_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Table associée à la commande.
	 */
	@Column(name = "tab_id", columnDefinition = "int")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 */
	@JoinColumn(name = "rev_id", referencedColumnName = "rev_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Reservation.class)
	private Reservation reservation;

	/**
	 * Statut de la commande.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "stc_code", nullable = false, length = 10, columnDefinition = "varchar")
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 */
	@JoinColumn(name = "avi_id", referencedColumnName = "avi_id", unique = true)
	@OneToOne(fetch = FetchType.LAZY, optional = true, cascade = CascadeType.ALL)
	private AvisClient avisClient;

	/**
	 * Association réciproque de LigneCommande.Commande.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "commande")
	private List<LigneCommande> lignes;

	/**
	 * Date de création de l'enregistrement.
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
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public Client getClient() {
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
	public Reservation getReservation() {
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
	public AvisClient getAvisClient() {
		return this.avisClient;
	}

	/**
	 * Accesseur de 'lignes'.
	 *
	 * @return Valeur de {@link #lignes lignes}.
	 */
	public List<LigneCommande> getLignes() {
		if (this.lignes == null) {
			this.lignes = new ArrayList<>();
		}
		return this.lignes;
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
	public void setClient(Client client) {
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
	public void setReservation(Reservation reservation) {
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
	public void setAvisClient(AvisClient avisClient) {
		this.avisClient = avisClient;
	}

	/**
	 * Définit la valeur de {@link #lignes lignes}.
	 * @param lignes Valeur à définir.
	 */
	public void setLignes(List<LigneCommande> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Commande Commande}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		CLIENT(Client.class),
		TABLE_ID(Integer.class),
		RESERVATION(Reservation.class),
		STATUT_COMMANDE(StatutCommande.class),
		AVIS_CLIENT(AvisClient.class),
		LIGNES(List.class),
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

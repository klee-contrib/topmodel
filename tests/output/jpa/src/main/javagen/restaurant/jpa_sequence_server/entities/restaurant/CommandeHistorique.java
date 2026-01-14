////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;

import restaurant.jpa_sequence_server.enums.restaurant.StatutCommandeCode;

/**
 * Commande pour historique avec préservation des clés primaires.
 */
@Entity
@Table(name = "COMMANDE_HISTORIQUE")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeHistorique {

	/**
	 * Identifiant de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@Id
	@Column(name = "COM_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@Column(name = "COM_DATE_COMMANDE", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column(name = "COM_DATE_LIVRAISON", columnDefinition = "timestamp")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@Column(name = "COM_MONTANT_TOTAL", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@JoinColumn(name = "PER_ID", referencedColumnName = "PER_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getTable() Commande#getTable()}
	 */
	@JoinColumn(name = "TAB_ID", referencedColumnName = "TAB_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TableRestaurant.class)
	private TableRestaurant table;

	/**
	 * Réservation associée à la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	@JoinColumn(name = "REV_ID", referencedColumnName = "REV_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Reservation.class)
	private Reservation reservation;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@JoinColumn(name = "STC_CODE", referencedColumnName = "STC_CODE")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = StatutCommande.class)
	private StatutCommande statutCommande = new StatutCommande(StatutCommandeCode.EN_ATT);

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	@JoinColumn(name = "AVI_ID", referencedColumnName = "AVI_ID", unique = true)
	@OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, optional = true)
	private AvisClient avisClient;

	/**
	 * Association réciproque de LigneCommandeHistorique.CommandeHistorique.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "commandeHistorique")
	private List<LigneCommandeHistorique> lignes;

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
	public Client getClient() {
		return this.client;
	}

	/**
	 * Getter for table.
	 *
	 * @return value of {@link #table table}.
	 */
	public TableRestaurant getTable() {
		return this.table;
	}

	/**
	 * Getter for reservation.
	 *
	 * @return value of {@link #reservation reservation}.
	 */
	public Reservation getReservation() {
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
	public AvisClient getAvisClient() {
		return this.avisClient;
	}

	/**
	 * Getter for lignes.
	 *
	 * @return value of {@link #lignes lignes}.
	 */
	public List<LigneCommandeHistorique> getLignes() {
		if (this.lignes == null) {
			this.lignes = new ArrayList<>();
		}
		return this.lignes;
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
	public void setClient(Client client) {
		this.client = client;
	}

	/**
	 * Set the value of {@link #table table}.
	 * @param table value to set.
	 */
	public void setTable(TableRestaurant table) {
		this.table = table;
	}

	/**
	 * Set the value of {@link #reservation reservation}.
	 * @param reservation value to set.
	 */
	public void setReservation(Reservation reservation) {
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
	public void setAvisClient(AvisClient avisClient) {
		this.avisClient = avisClient;
	}

	/**
	 * Set the value of {@link #lignes lignes}.
	 * @param lignes value to set.
	 */
	public void setLignes(List<LigneCommandeHistorique> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.CommandeHistorique CommandeHistorique}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		CLIENT(Client.class),
		TABLE(TableRestaurant.class),
		RESERVATION(Reservation.class),
		STATUT_COMMANDE(StatutCommande.class),
		AVIS_CLIENT(AvisClient.class),
		LIGNES(List.class);

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

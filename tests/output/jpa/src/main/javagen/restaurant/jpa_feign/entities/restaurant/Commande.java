////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

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

import restaurant.jpa_feign.enums.restaurant.StatutCommande;

/**
 * Commande d'un client.
 */
@Entity
@Table(name = "COMMANDE")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Commande {

	/**
	 * Identifiant de la commande.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "COM_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 */
	@Column(name = "COM_DATE_COMMANDE", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 */
	@Column(name = "COM_DATE_LIVRAISON", columnDefinition = "timestamp")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 */
	@Column(name = "COM_MONTANT_TOTAL", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 */
	@JoinColumn(name = "PER_ID", referencedColumnName = "PER_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
	private Client client;

	/**
	 * Table associée à la commande.
	 */
	@Column(name = "TAB_ID", columnDefinition = "int")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 */
	@JoinColumn(name = "REV_ID", referencedColumnName = "REV_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Reservation.class)
	private Reservation reservation;

	/**
	 * Statut de la commande.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "STC_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 */
	@JoinColumn(name = "AVI_ID", referencedColumnName = "AVI_ID", unique = true)
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
	@Column(name = "COM_DATE_CREATION", nullable = false, columnDefinition = "timestamp")
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
	public Client getClient() {
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
	public List<LigneCommande> getLignes() {
		if (this.lignes == null) {
			this.lignes = new ArrayList<>();
		}
		return this.lignes;
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
	public void setClient(Client client) {
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
	public void setLignes(List<LigneCommande> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Commande#lignes lignes}.
	 * @param ligneCommande value to add to commande.
	 */
	void addLigneCommande(LigneCommande ligneCommande) {
		this.lignes.add(ligneCommande);
		ligneCommande.setCommande(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Commande#lignes lignes}.
	 * @param ligneCommande ligneCommande value to remove.
	 */
	void removeLigneCommande(LigneCommande ligneCommande) {
		this.lignes.remove(ligneCommande);
		ligneCommande.setCommande(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Commande Commande}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.Valid;

import restaurant.jpa_server.enums.restaurant.StatutCommande;

/**
 * Détail d'une commande en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private BigDecimal montantTotal;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	private Integer tableId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private Integer avisClientId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@Valid
	@NotNull
	private ClientRead client;

	/**
	 * Réservation.
	 */
	@Valid
	private ReservationRead reservation;

	/**
	 * Association réciproque de LigneCommande.Commande.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@Valid
	@NotNull
	private List<LigneCommandeRead> lignes;

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
	 * Getter for tableId.
	 *
	 * @return value of {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
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
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for client.
	 *
	 * @return value of {@link #client client}.
	 */
	public ClientRead getClient() {
		return this.client;
	}

	/**
	 * Getter for reservation.
	 *
	 * @return value of {@link #reservation reservation}.
	 */
	public ReservationRead getReservation() {
		return this.reservation;
	}

	/**
	 * Getter for lignes.
	 *
	 * @return value of {@link #lignes lignes}.
	 */
	public List<LigneCommandeRead> getLignes() {
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
	 * Set the value of {@link #tableId tableId}.
	 * @param tableId value to set.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
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
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link #client client}.
	 * @param client value to set.
	 */
	public void setClient(ClientRead client) {
		this.client = client;
	}

	/**
	 * Set the value of {@link #reservation reservation}.
	 * @param reservation value to set.
	 */
	public void setReservation(ReservationRead reservation) {
		this.reservation = reservation;
	}

	/**
	 * Set the value of {@link #lignes lignes}.
	 * @param lignes value to set.
	 */
	public void setLignes(List<LigneCommandeRead> lignes) {
		this.lignes = lignes;
	}
}

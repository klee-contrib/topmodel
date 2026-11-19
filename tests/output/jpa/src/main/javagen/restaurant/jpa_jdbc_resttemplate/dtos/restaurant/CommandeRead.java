////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.Valid;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.StatutCommande;

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
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	@Column("com_id")
	private Integer commandeId;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	@Column("com_date_commande")
	private LocalDateTime commandeDateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column("com_date_livraison")
	private LocalDateTime commandeDateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	@Column("com_montant_total")
	private BigDecimal commandeMontantTotal;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column("tab_id")
	private Integer commandeTableId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	@Column("stc_code")
	private StatutCommande commandeStatutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	@Column("avi_id")
	private Integer commandeAvisClientId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	@Column("com_date_creation")
	private LocalDateTime commandeDateCreation;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getClient() Commande#getClient()}
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
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@Valid
	@NotNull
	private List<LigneCommandeRead> lignes;

	/**
	 * Getter for commandeId.
	 *
	 * @return value of {@link #commandeId commandeId}.
	 */
	public Integer getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Getter for commandeDateCommande.
	 *
	 * @return value of {@link #commandeDateCommande commandeDateCommande}.
	 */
	public LocalDateTime getCommandeDateCommande() {
		return this.commandeDateCommande;
	}

	/**
	 * Getter for commandeDateLivraison.
	 *
	 * @return value of {@link #commandeDateLivraison commandeDateLivraison}.
	 */
	public LocalDateTime getCommandeDateLivraison() {
		return this.commandeDateLivraison;
	}

	/**
	 * Getter for commandeMontantTotal.
	 *
	 * @return value of {@link #commandeMontantTotal commandeMontantTotal}.
	 */
	public BigDecimal getCommandeMontantTotal() {
		return this.commandeMontantTotal;
	}

	/**
	 * Getter for commandeTableId.
	 *
	 * @return value of {@link #commandeTableId commandeTableId}.
	 */
	public Integer getCommandeTableId() {
		return this.commandeTableId;
	}

	/**
	 * Getter for commandeStatutCommande.
	 *
	 * @return value of {@link #commandeStatutCommande commandeStatutCommande}.
	 */
	public StatutCommande getCommandeStatutCommande() {
		return this.commandeStatutCommande;
	}

	/**
	 * Getter for commandeAvisClientId.
	 *
	 * @return value of {@link #commandeAvisClientId commandeAvisClientId}.
	 */
	public Integer getCommandeAvisClientId() {
		return this.commandeAvisClientId;
	}

	/**
	 * Getter for commandeDateCreation.
	 *
	 * @return value of {@link #commandeDateCreation commandeDateCreation}.
	 */
	public LocalDateTime getCommandeDateCreation() {
		return this.commandeDateCreation;
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
	 * Set the value of {@link #commandeId commandeId}.
	 * @param commandeId value to set.
	 */
	public void setCommandeId(Integer commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Set the value of {@link #commandeDateCommande commandeDateCommande}.
	 * @param commandeDateCommande value to set.
	 */
	public void setCommandeDateCommande(LocalDateTime commandeDateCommande) {
		this.commandeDateCommande = commandeDateCommande;
	}

	/**
	 * Set the value of {@link #commandeDateLivraison commandeDateLivraison}.
	 * @param commandeDateLivraison value to set.
	 */
	public void setCommandeDateLivraison(LocalDateTime commandeDateLivraison) {
		this.commandeDateLivraison = commandeDateLivraison;
	}

	/**
	 * Set the value of {@link #commandeMontantTotal commandeMontantTotal}.
	 * @param commandeMontantTotal value to set.
	 */
	public void setCommandeMontantTotal(BigDecimal commandeMontantTotal) {
		this.commandeMontantTotal = commandeMontantTotal;
	}

	/**
	 * Set the value of {@link #commandeTableId commandeTableId}.
	 * @param commandeTableId value to set.
	 */
	public void setCommandeTableId(Integer commandeTableId) {
		this.commandeTableId = commandeTableId;
	}

	/**
	 * Set the value of {@link #commandeStatutCommande commandeStatutCommande}.
	 * @param commandeStatutCommande value to set.
	 */
	public void setCommandeStatutCommande(StatutCommande commandeStatutCommande) {
		this.commandeStatutCommande = commandeStatutCommande;
	}

	/**
	 * Set the value of {@link #commandeAvisClientId commandeAvisClientId}.
	 * @param commandeAvisClientId value to set.
	 */
	public void setCommandeAvisClientId(Integer commandeAvisClientId) {
		this.commandeAvisClientId = commandeAvisClientId;
	}

	/**
	 * Set the value of {@link #commandeDateCreation commandeDateCreation}.
	 * @param commandeDateCreation value to set.
	 */
	public void setCommandeDateCreation(LocalDateTime commandeDateCreation) {
		this.commandeDateCreation = commandeDateCreation;
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

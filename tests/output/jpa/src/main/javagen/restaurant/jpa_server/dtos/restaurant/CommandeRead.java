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
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private Integer commandeId;

	/**
	 * Date et heure de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime commandeDateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime commandeDateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private BigDecimal commandeMontantTotal;

	/**
	 * Table associée à la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	private Integer commandeTableId;

	/**
	 * Statut de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommande commandeStatutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private Integer commandeAvisClientId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime commandeDateCreation;

	/**
	 * Client ayant passé la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getClient() Commande#getClient()}
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
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@Valid
	@NotNull
	private List<LigneCommandeRead> lignes;

	/**
	 * Accesseur de 'commandeId'.
	 *
	 * @return Valeur de {@link #commandeId commandeId}.
	 */
	public Integer getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Accesseur de 'commandeDateCommande'.
	 *
	 * @return Valeur de {@link #commandeDateCommande commandeDateCommande}.
	 */
	public LocalDateTime getCommandeDateCommande() {
		return this.commandeDateCommande;
	}

	/**
	 * Accesseur de 'commandeDateLivraison'.
	 *
	 * @return Valeur de {@link #commandeDateLivraison commandeDateLivraison}.
	 */
	public LocalDateTime getCommandeDateLivraison() {
		return this.commandeDateLivraison;
	}

	/**
	 * Accesseur de 'commandeMontantTotal'.
	 *
	 * @return Valeur de {@link #commandeMontantTotal commandeMontantTotal}.
	 */
	public BigDecimal getCommandeMontantTotal() {
		return this.commandeMontantTotal;
	}

	/**
	 * Accesseur de 'commandeTableId'.
	 *
	 * @return Valeur de {@link #commandeTableId commandeTableId}.
	 */
	public Integer getCommandeTableId() {
		return this.commandeTableId;
	}

	/**
	 * Accesseur de 'commandeStatutCommande'.
	 *
	 * @return Valeur de {@link #commandeStatutCommande commandeStatutCommande}.
	 */
	public StatutCommande getCommandeStatutCommande() {
		return this.commandeStatutCommande;
	}

	/**
	 * Accesseur de 'commandeAvisClientId'.
	 *
	 * @return Valeur de {@link #commandeAvisClientId commandeAvisClientId}.
	 */
	public Integer getCommandeAvisClientId() {
		return this.commandeAvisClientId;
	}

	/**
	 * Accesseur de 'commandeDateCreation'.
	 *
	 * @return Valeur de {@link #commandeDateCreation commandeDateCreation}.
	 */
	public LocalDateTime getCommandeDateCreation() {
		return this.commandeDateCreation;
	}

	/**
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public ClientRead getClient() {
		return this.client;
	}

	/**
	 * Accesseur de 'reservation'.
	 *
	 * @return Valeur de {@link #reservation reservation}.
	 */
	public ReservationRead getReservation() {
		return this.reservation;
	}

	/**
	 * Accesseur de 'lignes'.
	 *
	 * @return Valeur de {@link #lignes lignes}.
	 */
	public List<LigneCommandeRead> getLignes() {
		return this.lignes;
	}

	/**
	 * Définit la valeur de {@link #commandeId commandeId}.
	 * @param commandeId Valeur à définir.
	 */
	public void setCommandeId(Integer commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Définit la valeur de {@link #commandeDateCommande commandeDateCommande}.
	 * @param commandeDateCommande Valeur à définir.
	 */
	public void setCommandeDateCommande(LocalDateTime commandeDateCommande) {
		this.commandeDateCommande = commandeDateCommande;
	}

	/**
	 * Définit la valeur de {@link #commandeDateLivraison commandeDateLivraison}.
	 * @param commandeDateLivraison Valeur à définir.
	 */
	public void setCommandeDateLivraison(LocalDateTime commandeDateLivraison) {
		this.commandeDateLivraison = commandeDateLivraison;
	}

	/**
	 * Définit la valeur de {@link #commandeMontantTotal commandeMontantTotal}.
	 * @param commandeMontantTotal Valeur à définir.
	 */
	public void setCommandeMontantTotal(BigDecimal commandeMontantTotal) {
		this.commandeMontantTotal = commandeMontantTotal;
	}

	/**
	 * Définit la valeur de {@link #commandeTableId commandeTableId}.
	 * @param commandeTableId Valeur à définir.
	 */
	public void setCommandeTableId(Integer commandeTableId) {
		this.commandeTableId = commandeTableId;
	}

	/**
	 * Définit la valeur de {@link #commandeStatutCommande commandeStatutCommande}.
	 * @param commandeStatutCommande Valeur à définir.
	 */
	public void setCommandeStatutCommande(StatutCommande commandeStatutCommande) {
		this.commandeStatutCommande = commandeStatutCommande;
	}

	/**
	 * Définit la valeur de {@link #commandeAvisClientId commandeAvisClientId}.
	 * @param commandeAvisClientId Valeur à définir.
	 */
	public void setCommandeAvisClientId(Integer commandeAvisClientId) {
		this.commandeAvisClientId = commandeAvisClientId;
	}

	/**
	 * Définit la valeur de {@link #commandeDateCreation commandeDateCreation}.
	 * @param commandeDateCreation Valeur à définir.
	 */
	public void setCommandeDateCreation(LocalDateTime commandeDateCreation) {
		this.commandeDateCreation = commandeDateCreation;
	}

	/**
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(ClientRead client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #reservation reservation}.
	 * @param reservation Valeur à définir.
	 */
	public void setReservation(ReservationRead reservation) {
		this.reservation = reservation;
	}

	/**
	 * Définit la valeur de {@link #lignes lignes}.
	 * @param lignes Valeur à définir.
	 */
	public void setLignes(List<LigneCommandeRead> lignes) {
		this.lignes = lignes;
	}
}

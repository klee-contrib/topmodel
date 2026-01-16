////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.Valid;

import restaurant.jpa_sequence_server.entities.restaurant.Commande;
import restaurant.jpa_sequence_server.entities.restaurant.RestaurantMappers;
import restaurant.jpa_sequence_server.enums.restaurant.StatutCommandeCode;

/**
 * Détail d'une commande en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getTable() Commande#getTable()}
	 */
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	private Integer reservationId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommandeCode statutCommandeCode = StatutCommandeCode.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private Integer avisClientId;

	/**
	 * Association réciproque de LigneCommande.CommandeId.
	 * Alias of {@link restaurant.jpa_sequence_server.dtos.restaurant.CommandeRead#getLignes() CommandeRead#getLignes()}
	 */
	@Valid
	@NotNull
	private List<LigneCommandeWrite> lignes;

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
	 * Getter for statutCommandeCode.
	 *
	 * @return value of {@link #statutCommandeCode statutCommandeCode}.
	 */
	public StatutCommandeCode getStatutCommandeCode() {
		return this.statutCommandeCode;
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
	 * Getter for lignes.
	 *
	 * @return value of {@link #lignes lignes}.
	 */
	public List<LigneCommandeWrite> getLignes() {
		return this.lignes;
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
	 * Set the value of {@link #statutCommandeCode statutCommandeCode}.
	 * @param statutCommandeCode value to set.
	 */
	public void setStatutCommandeCode(StatutCommandeCode statutCommandeCode) {
		this.statutCommandeCode = statutCommandeCode;
	}

	/**
	 * Set the value of {@link #avisClientId avisClientId}.
	 * @param avisClientId value to set.
	 */
	public void setAvisClientId(Integer avisClientId) {
		this.avisClientId = avisClientId;
	}

	/**
	 * Set the value of {@link #lignes lignes}.
	 * @param lignes value to set.
	 */
	public void setLignes(List<LigneCommandeWrite> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Mappe 'CommandeWrite' vers 'Commande'.
	 * @param target Instance pré-existante de 'Commande'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Commande'.
	 */
	public Commande toCommande(Commande target) {
		return RestaurantMappers.toCommande(this, target);
	}
}

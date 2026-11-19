////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.Valid;

import restaurant.jpa_feign.enums.restaurant.StatutCommande;

/**
 * Détail d'une commande en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Date et heure de la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private BigDecimal montantTotal;

	/**
	 * Table associée à la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	private Integer tableId;

	/**
	 * Statut de la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommande statutCommande = StatutCommande.EN_ATT;

	/**
	 * Avis laissé par le client sur la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private Integer avisClientId;

	/**
	 * Client ayant passé la commande.
	 * Alias de {@link restaurant.jpa_feign.dtos.restaurant.CommandeRead#getClient() CommandeRead#getClient()}
	 */
	@Valid
	@NotNull
	private ClientWrite client;

	/**
	 * Réservation.
	 * Alias de {@link restaurant.jpa_feign.dtos.restaurant.CommandeRead#getReservation() CommandeRead#getReservation()}
	 */
	@Valid
	private ReservationWrite reservation;

	/**
	 * Association réciproque de LigneCommande.Commande.
	 * Alias de {@link restaurant.jpa_feign.dtos.restaurant.CommandeRead#getLignes() CommandeRead#getLignes()}
	 */
	@Valid
	@NotNull
	private List<LigneCommandeWrite> lignes;

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
	 * Accesseur de 'tableId'.
	 *
	 * @return Valeur de {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
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
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public ClientWrite getClient() {
		return this.client;
	}

	/**
	 * Accesseur de 'reservation'.
	 *
	 * @return Valeur de {@link #reservation reservation}.
	 */
	public ReservationWrite getReservation() {
		return this.reservation;
	}

	/**
	 * Accesseur de 'lignes'.
	 *
	 * @return Valeur de {@link #lignes lignes}.
	 */
	public List<LigneCommandeWrite> getLignes() {
		return this.lignes;
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
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
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
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(ClientWrite client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #reservation reservation}.
	 * @param reservation Valeur à définir.
	 */
	public void setReservation(ReservationWrite reservation) {
		this.reservation = reservation;
	}

	/**
	 * Définit la valeur de {@link #lignes lignes}.
	 * @param lignes Valeur à définir.
	 */
	public void setLignes(List<LigneCommandeWrite> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.CommandeWrite CommandeWrite}.
	 */
	public enum Fields {
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		TABLE_ID(Integer.class),
		STATUT_COMMANDE(StatutCommande.class),
		AVIS_CLIENT_ID(Integer.class),
		CLIENT(ClientWrite.class),
		RESERVATION(ReservationWrite.class),
		LIGNES(List.class);

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

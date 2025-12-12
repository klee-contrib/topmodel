////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

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
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column("com_date_livraison")
	private LocalDateTime dateLivraison;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getClientId() Commande#getClientId()}
	 */
	@NotNull
	@Column("per_id")
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column("tab_id")
	private Integer tableId;

	/**
	 * Réservation associée à la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getReservationId() Commande#getReservationId()}
	 */
	@Column("rev_id")
	private Integer reservationId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getStatutCommandeCode() Commande#getStatutCommandeCode()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("stc_code")
	private String statutCommandeCode = "EnAttente";

	/**
	 * Association réciproque de LigneCommande.CommandeId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getLigneCommandes() Commande#getLigneCommandes()}
	 */
	@NotNull
	@Column("lig_id")
	private List<Integer> ligneCommandes;

	/**
	 * Getter for dateLivraison.
	 *
	 * @return value of {@link #dateLivraison dateLivraison}.
	 */
	public LocalDateTime getDateLivraison() {
		return this.dateLivraison;
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
	public String getStatutCommandeCode() {
		return this.statutCommandeCode;
	}

	/**
	 * Getter for ligneCommandes.
	 *
	 * @return value of {@link #ligneCommandes ligneCommandes}.
	 */
	public List<Integer> getLigneCommandes() {
		return this.ligneCommandes;
	}

	/**
	 * Set the value of {@link #dateLivraison dateLivraison}.
	 * @param dateLivraison value to set.
	 */
	public void setDateLivraison(LocalDateTime dateLivraison) {
		this.dateLivraison = dateLivraison;
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
	public void setStatutCommandeCode(String statutCommandeCode) {
		this.statutCommandeCode = statutCommandeCode;
	}

	/**
	 * Set the value of {@link #ligneCommandes ligneCommandes}.
	 * @param ligneCommandes value to set.
	 */
	public void setLigneCommandes(List<Integer> ligneCommandes) {
		this.ligneCommandes = ligneCommandes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeWrite CommandeWrite}.
	 */
	public enum Fields {
		DATE_LIVRAISON(LocalDateTime.class),
		CLIENT_ID(Integer.class),
		TABLE_ID(Integer.class),
		RESERVATION_ID(Integer.class),
		STATUT_COMMANDE_CODE(String.class),
		LIGNE_COMMANDES(List.class);

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

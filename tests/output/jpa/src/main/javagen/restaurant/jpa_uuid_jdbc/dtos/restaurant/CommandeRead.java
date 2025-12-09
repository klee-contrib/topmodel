////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

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
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	@Column("com_id")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	@Column("com_date_commande")
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column("com_date_livraison")
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	@Column("com_montant_total")
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getClientId() Commande#getClientId()}
	 */
	@NotNull
	@Column("cli_id")
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getTableClientId() Commande#getTableClientId()}
	 */
	@Column("tab_id")
	private Integer tableClientId;

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
	 * Getter for clientId.
	 *
	 * @return value of {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Getter for tableClientId.
	 *
	 * @return value of {@link #tableClientId tableClientId}.
	 */
	public Integer getTableClientId() {
		return this.tableClientId;
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
	 * Set the value of {@link #clientId clientId}.
	 * @param clientId value to set.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Set the value of {@link #tableClientId tableClientId}.
	 * @param tableClientId value to set.
	 */
	public void setTableClientId(Integer tableClientId) {
		this.tableClientId = tableClientId;
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
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeRead CommandeRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		CLIENT_ID(Integer.class),
		TABLE_CLIENT_ID(Integer.class),
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

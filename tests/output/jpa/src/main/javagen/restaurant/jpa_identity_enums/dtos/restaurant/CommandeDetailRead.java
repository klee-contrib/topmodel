////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.Valid;

import restaurant.jpa_identity_enums.enums.restaurant.StatutCommande;

/**
 * Détail complet d'une commande avec ses lignes.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeDetailRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime dateCommande;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime dateLivraison;

	/**
	 * Montant total de la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private BigDecimal montantTotal;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getTableClient() Commande#getTableClient()}
	 */
	private Integer tableClientId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommande statutCommandeCode = StatutCommande.EN_ATT;

	/**
	 * Association réciproque de LigneCommande.CommandeId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Commande#getLigneCommandes() Commande#getLigneCommandes()}
	 */
	@NotNull
	private List<Integer> ligneCommandes;

	/**
	 * Liste des lignes de commande.
	 */
	@Valid
	@NotNull
	private List<LigneCommandeItem> lignes;

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
	public StatutCommande getStatutCommandeCode() {
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
	 * Getter for lignes.
	 *
	 * @return value of {@link #lignes lignes}.
	 */
	public List<LigneCommandeItem> getLignes() {
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
	public void setStatutCommandeCode(StatutCommande statutCommandeCode) {
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
	 * Set the value of {@link #lignes lignes}.
	 * @param lignes value to set.
	 */
	public void setLignes(List<LigneCommandeItem> lignes) {
		this.lignes = lignes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.CommandeDetailRead CommandeDetailRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class),
		DATE_LIVRAISON(LocalDateTime.class),
		MONTANT_TOTAL(BigDecimal.class),
		CLIENT_ID(Integer.class),
		TABLE_CLIENT_ID(Integer.class),
		STATUT_COMMANDE_CODE(StatutCommande.class),
		LIGNE_COMMANDES(List.class),
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

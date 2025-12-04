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
 * Client avec la liste de ses commandes.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientAvecCommandes implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getId() Client#getId()}
	 */
	@NotNull
	@Column("cli_id")
	private Integer id;

	/**
	 * Nom du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getNom() Client#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("cli_nom")
	private String nom;

	/**
	 * Prénom du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getPrenom() Client#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("cli_prenom")
	private String prenom;

	/**
	 * Numéro de téléphone du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getTelephone() Client#getTelephone()}
	 */
	@Size(max = 20)
	@Column("cli_telephone")
	private String telephone;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	@Column("cli_email")
	private String email;

	/**
	 * Association réciproque de Commande.ClientId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getCommandes() Client#getCommandes()}
	 */
	@NotNull
	@Column("com_id")
	private List<Integer> commandes;

	/**
	 * Association réciproque de AvisClient.ClientIdClient.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getAvisClientsClient() Client#getAvisClientsClient()}
	 */
	@NotNull
	@Column("avi_id_client")
	private List<Integer> avisClientsClient;

	/**
	 * Association réciproque de Reservation.ClientIdClient.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getReservationsClient() Client#getReservationsClient()}
	 */
	@NotNull
	@Column("rev_id_client")
	private List<Integer> reservationsClient;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	@Column("com_id")
	private List<Integer> commandeId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	@Column("com_date_commande")
	private List<LocalDateTime> commandeDateCommande;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	@Column("com_date_livraison")
	private List<LocalDateTime> commandeDateLivraison;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	@Column("com_montant_total")
	private List<BigDecimal> commandeMontantTotal;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getClientId() Commande#getClientId()}
	 */
	@NotNull
	@Column("cli_id")
	private List<Integer> commandeClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getTableClientId() Commande#getTableClientId()}
	 */
	@Column("tab_id")
	private List<Integer> commandeTableClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getStatutCommandeCode() Commande#getStatutCommandeCode()}
	 */
	@NotNull
	@Column("stc_code")
	private List<String> commandeStatutCommandeCode;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getLigneCommandes() Commande#getLigneCommandes()}
	 */
	@NotNull
	@Column("lig_id")
	private List<List<Integer>> commandeLigneCommandes;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for prenom.
	 *
	 * @return value of {@link #prenom prenom}.
	 */
	public String getPrenom() {
		return this.prenom;
	}

	/**
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for commandes.
	 *
	 * @return value of {@link #commandes commandes}.
	 */
	public List<Integer> getCommandes() {
		return this.commandes;
	}

	/**
	 * Getter for avisClientsClient.
	 *
	 * @return value of {@link #avisClientsClient avisClientsClient}.
	 */
	public List<Integer> getAvisClientsClient() {
		return this.avisClientsClient;
	}

	/**
	 * Getter for reservationsClient.
	 *
	 * @return value of {@link #reservationsClient reservationsClient}.
	 */
	public List<Integer> getReservationsClient() {
		return this.reservationsClient;
	}

	/**
	 * Getter for commandeId.
	 *
	 * @return value of {@link #commandeId commandeId}.
	 */
	public List<Integer> getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Getter for commandeDateCommande.
	 *
	 * @return value of {@link #commandeDateCommande commandeDateCommande}.
	 */
	public List<LocalDateTime> getCommandeDateCommande() {
		return this.commandeDateCommande;
	}

	/**
	 * Getter for commandeDateLivraison.
	 *
	 * @return value of {@link #commandeDateLivraison commandeDateLivraison}.
	 */
	public List<LocalDateTime> getCommandeDateLivraison() {
		return this.commandeDateLivraison;
	}

	/**
	 * Getter for commandeMontantTotal.
	 *
	 * @return value of {@link #commandeMontantTotal commandeMontantTotal}.
	 */
	public List<BigDecimal> getCommandeMontantTotal() {
		return this.commandeMontantTotal;
	}

	/**
	 * Getter for commandeClientId.
	 *
	 * @return value of {@link #commandeClientId commandeClientId}.
	 */
	public List<Integer> getCommandeClientId() {
		return this.commandeClientId;
	}

	/**
	 * Getter for commandeTableClientId.
	 *
	 * @return value of {@link #commandeTableClientId commandeTableClientId}.
	 */
	public List<Integer> getCommandeTableClientId() {
		return this.commandeTableClientId;
	}

	/**
	 * Getter for commandeStatutCommandeCode.
	 *
	 * @return value of {@link #commandeStatutCommandeCode commandeStatutCommandeCode}.
	 */
	public List<String> getCommandeStatutCommandeCode() {
		return this.commandeStatutCommandeCode;
	}

	/**
	 * Getter for commandeLigneCommandes.
	 *
	 * @return value of {@link #commandeLigneCommandes commandeLigneCommandes}.
	 */
	public List<List<Integer>> getCommandeLigneCommandes() {
		return this.commandeLigneCommandes;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #prenom prenom}.
	 * @param prenom value to set.
	 */
	public void setPrenom(String prenom) {
		this.prenom = prenom;
	}

	/**
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #commandes commandes}.
	 * @param commandes value to set.
	 */
	public void setCommandes(List<Integer> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #avisClientsClient avisClientsClient}.
	 * @param avisClientsClient value to set.
	 */
	public void setAvisClientsClient(List<Integer> avisClientsClient) {
		this.avisClientsClient = avisClientsClient;
	}

	/**
	 * Set the value of {@link #reservationsClient reservationsClient}.
	 * @param reservationsClient value to set.
	 */
	public void setReservationsClient(List<Integer> reservationsClient) {
		this.reservationsClient = reservationsClient;
	}

	/**
	 * Set the value of {@link #commandeId commandeId}.
	 * @param commandeId value to set.
	 */
	public void setCommandeId(List<Integer> commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Set the value of {@link #commandeDateCommande commandeDateCommande}.
	 * @param commandeDateCommande value to set.
	 */
	public void setCommandeDateCommande(List<LocalDateTime> commandeDateCommande) {
		this.commandeDateCommande = commandeDateCommande;
	}

	/**
	 * Set the value of {@link #commandeDateLivraison commandeDateLivraison}.
	 * @param commandeDateLivraison value to set.
	 */
	public void setCommandeDateLivraison(List<LocalDateTime> commandeDateLivraison) {
		this.commandeDateLivraison = commandeDateLivraison;
	}

	/**
	 * Set the value of {@link #commandeMontantTotal commandeMontantTotal}.
	 * @param commandeMontantTotal value to set.
	 */
	public void setCommandeMontantTotal(List<BigDecimal> commandeMontantTotal) {
		this.commandeMontantTotal = commandeMontantTotal;
	}

	/**
	 * Set the value of {@link #commandeClientId commandeClientId}.
	 * @param commandeClientId value to set.
	 */
	public void setCommandeClientId(List<Integer> commandeClientId) {
		this.commandeClientId = commandeClientId;
	}

	/**
	 * Set the value of {@link #commandeTableClientId commandeTableClientId}.
	 * @param commandeTableClientId value to set.
	 */
	public void setCommandeTableClientId(List<Integer> commandeTableClientId) {
		this.commandeTableClientId = commandeTableClientId;
	}

	/**
	 * Set the value of {@link #commandeStatutCommandeCode commandeStatutCommandeCode}.
	 * @param commandeStatutCommandeCode value to set.
	 */
	public void setCommandeStatutCommandeCode(List<String> commandeStatutCommandeCode) {
		this.commandeStatutCommandeCode = commandeStatutCommandeCode;
	}

	/**
	 * Set the value of {@link #commandeLigneCommandes commandeLigneCommandes}.
	 * @param commandeLigneCommandes value to set.
	 */
	public void setCommandeLigneCommandes(List<List<Integer>> commandeLigneCommandes) {
		this.commandeLigneCommandes = commandeLigneCommandes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientAvecCommandes ClientAvecCommandes}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		TELEPHONE(String.class),
		EMAIL(String.class),
		COMMANDES(List.class),
		AVIS_CLIENTS_CLIENT(List.class),
		RESERVATIONS_CLIENT(List.class),
		COMMANDE_ID(List.class),
		COMMANDE_DATE_COMMANDE(List.class),
		COMMANDE_DATE_LIVRAISON(List.class),
		COMMANDE_MONTANT_TOTAL(List.class),
		COMMANDE_CLIENT_ID(List.class),
		COMMANDE_TABLE_CLIENT_ID(List.class),
		COMMANDE_STATUT_COMMANDE_CODE(List.class),
		COMMANDE_LIGNE_COMMANDES(List.class);

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

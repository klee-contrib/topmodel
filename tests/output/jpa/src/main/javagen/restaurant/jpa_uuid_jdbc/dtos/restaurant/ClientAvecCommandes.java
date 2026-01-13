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
	 * Identifiant de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	@Column("per_id")
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_nom")
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_prenom")
	private String prenom;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	@Column("cli_email")
	private String email;

	/**
	 * Association réciproque de AvisClient.ClientId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	@Column("avi_id")
	private List<Integer> avisClients;

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
	@Column("per_id")
	private List<Integer> commandeClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	@Column("tab_id")
	private List<Integer> commandeTableId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getReservationId() Commande#getReservationId()}
	 */
	@Column("rev_id")
	private List<Integer> commandeReservationId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getStatutCommandeCode() Commande#getStatutCommandeCode()}
	 */
	@NotNull
	@Column("stc_code")
	private List<String> commandeStatutCommandeCode;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getAvisClientId() Commande#getAvisClientId()}
	 */
	@Column("avi_id")
	private List<Integer> commandeAvisClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@NotNull
	@Column("lig_id")
	private List<List<Integer>> commandeLignes;

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
	 * Getter for email.
	 *
	 * @return value of {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for avisClients.
	 *
	 * @return value of {@link #avisClients avisClients}.
	 */
	public List<Integer> getAvisClients() {
		return this.avisClients;
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
	 * Getter for commandeTableId.
	 *
	 * @return value of {@link #commandeTableId commandeTableId}.
	 */
	public List<Integer> getCommandeTableId() {
		return this.commandeTableId;
	}

	/**
	 * Getter for commandeReservationId.
	 *
	 * @return value of {@link #commandeReservationId commandeReservationId}.
	 */
	public List<Integer> getCommandeReservationId() {
		return this.commandeReservationId;
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
	 * Getter for commandeAvisClientId.
	 *
	 * @return value of {@link #commandeAvisClientId commandeAvisClientId}.
	 */
	public List<Integer> getCommandeAvisClientId() {
		return this.commandeAvisClientId;
	}

	/**
	 * Getter for commandeLignes.
	 *
	 * @return value of {@link #commandeLignes commandeLignes}.
	 */
	public List<List<Integer>> getCommandeLignes() {
		return this.commandeLignes;
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
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
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
	 * Set the value of {@link #commandeTableId commandeTableId}.
	 * @param commandeTableId value to set.
	 */
	public void setCommandeTableId(List<Integer> commandeTableId) {
		this.commandeTableId = commandeTableId;
	}

	/**
	 * Set the value of {@link #commandeReservationId commandeReservationId}.
	 * @param commandeReservationId value to set.
	 */
	public void setCommandeReservationId(List<Integer> commandeReservationId) {
		this.commandeReservationId = commandeReservationId;
	}

	/**
	 * Set the value of {@link #commandeStatutCommandeCode commandeStatutCommandeCode}.
	 * @param commandeStatutCommandeCode value to set.
	 */
	public void setCommandeStatutCommandeCode(List<String> commandeStatutCommandeCode) {
		this.commandeStatutCommandeCode = commandeStatutCommandeCode;
	}

	/**
	 * Set the value of {@link #commandeAvisClientId commandeAvisClientId}.
	 * @param commandeAvisClientId value to set.
	 */
	public void setCommandeAvisClientId(List<Integer> commandeAvisClientId) {
		this.commandeAvisClientId = commandeAvisClientId;
	}

	/**
	 * Set the value of {@link #commandeLignes commandeLignes}.
	 * @param commandeLignes value to set.
	 */
	public void setCommandeLignes(List<List<Integer>> commandeLignes) {
		this.commandeLignes = commandeLignes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientAvecCommandes ClientAvecCommandes}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		EMAIL(String.class),
		AVIS_CLIENTS(List.class),
		COMMANDE_ID(List.class),
		COMMANDE_DATE_COMMANDE(List.class),
		COMMANDE_DATE_LIVRAISON(List.class),
		COMMANDE_MONTANT_TOTAL(List.class),
		COMMANDE_CLIENT_ID(List.class),
		COMMANDE_TABLE_ID(List.class),
		COMMANDE_RESERVATION_ID(List.class),
		COMMANDE_STATUT_COMMANDE_CODE(List.class),
		COMMANDE_AVIS_CLIENT_ID(List.class),
		COMMANDE_LIGNES(List.class);

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

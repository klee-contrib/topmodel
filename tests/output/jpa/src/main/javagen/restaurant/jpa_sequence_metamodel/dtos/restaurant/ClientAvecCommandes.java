////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_sequence_metamodel.enums.restaurant.StatutCommandeCode;

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
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String email;

	/**
	 * Association réciproque de AvisClient.ClientId.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private List<Integer> commandeId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private List<LocalDateTime> commandeDateCommande;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private List<LocalDateTime> commandeDateLivraison;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private List<BigDecimal> commandeMontantTotal;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private List<Integer> commandeClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getTable() Commande#getTable()}
	 */
	private List<Integer> commandeTableId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	private List<Integer> commandeReservationId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private List<StatutCommandeCode> commandeStatutCommandeCode;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Commande#getLigneCommandes() Commande#getLigneCommandes()}
	 */
	@NotNull
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
	public List<StatutCommandeCode> getCommandeStatutCommandeCode() {
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
	public void setCommandeStatutCommandeCode(List<StatutCommandeCode> commandeStatutCommandeCode) {
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
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.dtos.restaurant.ClientAvecCommandes ClientAvecCommandes}.
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

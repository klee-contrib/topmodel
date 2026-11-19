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
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

import restaurant.jpa_feign.enums.restaurant.StatutCommande;

/**
 * Client avec la liste de ses commandes.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientAvecCommandes implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	private String departementCode;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getDateCreation() Personne#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Adresse email du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String email;

	/**
	 * Carte Swile du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Client#getSwileCard() Client#getSwileCard()}
	 */
	private Integer swileCardId;

	/**
	 * Association réciproque de AvisClient.Client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private List<Integer> commandeId;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private List<LocalDateTime> commandeDateCommande;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private List<LocalDateTime> commandeDateLivraison;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private List<BigDecimal> commandeMontantTotal;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private List<Integer> commandeClientId;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	private List<Integer> commandeTableId;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	private List<Integer> commandeReservationId;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private List<StatutCommande> commandeStatutCommande;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private List<Integer> commandeAvisClientId;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@NotNull
	private List<List<Integer>> commandeLignes;

	/**
	 * Liste des commandes du client.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private List<LocalDateTime> commandeDateCreation;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'nom'.
	 *
	 * @return Valeur de {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Accesseur de 'prenom'.
	 *
	 * @return Valeur de {@link #prenom prenom}.
	 */
	public String getPrenom() {
		return this.prenom;
	}

	/**
	 * Accesseur de 'departementCode'.
	 *
	 * @return Valeur de {@link #departementCode departementCode}.
	 */
	public String getDepartementCode() {
		return this.departementCode;
	}

	/**
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Accesseur de 'email'.
	 *
	 * @return Valeur de {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Accesseur de 'swileCardId'.
	 *
	 * @return Valeur de {@link #swileCardId swileCardId}.
	 */
	public Integer getSwileCardId() {
		return this.swileCardId;
	}

	/**
	 * Accesseur de 'avisClients'.
	 *
	 * @return Valeur de {@link #avisClients avisClients}.
	 */
	public List<Integer> getAvisClients() {
		return this.avisClients;
	}

	/**
	 * Accesseur de 'commandeId'.
	 *
	 * @return Valeur de {@link #commandeId commandeId}.
	 */
	public List<Integer> getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Accesseur de 'commandeDateCommande'.
	 *
	 * @return Valeur de {@link #commandeDateCommande commandeDateCommande}.
	 */
	public List<LocalDateTime> getCommandeDateCommande() {
		return this.commandeDateCommande;
	}

	/**
	 * Accesseur de 'commandeDateLivraison'.
	 *
	 * @return Valeur de {@link #commandeDateLivraison commandeDateLivraison}.
	 */
	public List<LocalDateTime> getCommandeDateLivraison() {
		return this.commandeDateLivraison;
	}

	/**
	 * Accesseur de 'commandeMontantTotal'.
	 *
	 * @return Valeur de {@link #commandeMontantTotal commandeMontantTotal}.
	 */
	public List<BigDecimal> getCommandeMontantTotal() {
		return this.commandeMontantTotal;
	}

	/**
	 * Accesseur de 'commandeClientId'.
	 *
	 * @return Valeur de {@link #commandeClientId commandeClientId}.
	 */
	public List<Integer> getCommandeClientId() {
		return this.commandeClientId;
	}

	/**
	 * Accesseur de 'commandeTableId'.
	 *
	 * @return Valeur de {@link #commandeTableId commandeTableId}.
	 */
	public List<Integer> getCommandeTableId() {
		return this.commandeTableId;
	}

	/**
	 * Accesseur de 'commandeReservationId'.
	 *
	 * @return Valeur de {@link #commandeReservationId commandeReservationId}.
	 */
	public List<Integer> getCommandeReservationId() {
		return this.commandeReservationId;
	}

	/**
	 * Accesseur de 'commandeStatutCommande'.
	 *
	 * @return Valeur de {@link #commandeStatutCommande commandeStatutCommande}.
	 */
	public List<StatutCommande> getCommandeStatutCommande() {
		return this.commandeStatutCommande;
	}

	/**
	 * Accesseur de 'commandeAvisClientId'.
	 *
	 * @return Valeur de {@link #commandeAvisClientId commandeAvisClientId}.
	 */
	public List<Integer> getCommandeAvisClientId() {
		return this.commandeAvisClientId;
	}

	/**
	 * Accesseur de 'commandeLignes'.
	 *
	 * @return Valeur de {@link #commandeLignes commandeLignes}.
	 */
	public List<List<Integer>> getCommandeLignes() {
		return this.commandeLignes;
	}

	/**
	 * Accesseur de 'commandeDateCreation'.
	 *
	 * @return Valeur de {@link #commandeDateCreation commandeDateCreation}.
	 */
	public List<LocalDateTime> getCommandeDateCreation() {
		return this.commandeDateCreation;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #nom nom}.
	 * @param nom Valeur à définir.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Définit la valeur de {@link #prenom prenom}.
	 * @param prenom Valeur à définir.
	 */
	public void setPrenom(String prenom) {
		this.prenom = prenom;
	}

	/**
	 * Définit la valeur de {@link #departementCode departementCode}.
	 * @param departementCode Valeur à définir.
	 */
	public void setDepartementCode(String departementCode) {
		this.departementCode = departementCode;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #email email}.
	 * @param email Valeur à définir.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Définit la valeur de {@link #swileCardId swileCardId}.
	 * @param swileCardId Valeur à définir.
	 */
	public void setSwileCardId(Integer swileCardId) {
		this.swileCardId = swileCardId;
	}

	/**
	 * Définit la valeur de {@link #avisClients avisClients}.
	 * @param avisClients Valeur à définir.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Définit la valeur de {@link #commandeId commandeId}.
	 * @param commandeId Valeur à définir.
	 */
	public void setCommandeId(List<Integer> commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Définit la valeur de {@link #commandeDateCommande commandeDateCommande}.
	 * @param commandeDateCommande Valeur à définir.
	 */
	public void setCommandeDateCommande(List<LocalDateTime> commandeDateCommande) {
		this.commandeDateCommande = commandeDateCommande;
	}

	/**
	 * Définit la valeur de {@link #commandeDateLivraison commandeDateLivraison}.
	 * @param commandeDateLivraison Valeur à définir.
	 */
	public void setCommandeDateLivraison(List<LocalDateTime> commandeDateLivraison) {
		this.commandeDateLivraison = commandeDateLivraison;
	}

	/**
	 * Définit la valeur de {@link #commandeMontantTotal commandeMontantTotal}.
	 * @param commandeMontantTotal Valeur à définir.
	 */
	public void setCommandeMontantTotal(List<BigDecimal> commandeMontantTotal) {
		this.commandeMontantTotal = commandeMontantTotal;
	}

	/**
	 * Définit la valeur de {@link #commandeClientId commandeClientId}.
	 * @param commandeClientId Valeur à définir.
	 */
	public void setCommandeClientId(List<Integer> commandeClientId) {
		this.commandeClientId = commandeClientId;
	}

	/**
	 * Définit la valeur de {@link #commandeTableId commandeTableId}.
	 * @param commandeTableId Valeur à définir.
	 */
	public void setCommandeTableId(List<Integer> commandeTableId) {
		this.commandeTableId = commandeTableId;
	}

	/**
	 * Définit la valeur de {@link #commandeReservationId commandeReservationId}.
	 * @param commandeReservationId Valeur à définir.
	 */
	public void setCommandeReservationId(List<Integer> commandeReservationId) {
		this.commandeReservationId = commandeReservationId;
	}

	/**
	 * Définit la valeur de {@link #commandeStatutCommande commandeStatutCommande}.
	 * @param commandeStatutCommande Valeur à définir.
	 */
	public void setCommandeStatutCommande(List<StatutCommande> commandeStatutCommande) {
		this.commandeStatutCommande = commandeStatutCommande;
	}

	/**
	 * Définit la valeur de {@link #commandeAvisClientId commandeAvisClientId}.
	 * @param commandeAvisClientId Valeur à définir.
	 */
	public void setCommandeAvisClientId(List<Integer> commandeAvisClientId) {
		this.commandeAvisClientId = commandeAvisClientId;
	}

	/**
	 * Définit la valeur de {@link #commandeLignes commandeLignes}.
	 * @param commandeLignes Valeur à définir.
	 */
	public void setCommandeLignes(List<List<Integer>> commandeLignes) {
		this.commandeLignes = commandeLignes;
	}

	/**
	 * Définit la valeur de {@link #commandeDateCreation commandeDateCreation}.
	 * @param commandeDateCreation Valeur à définir.
	 */
	public void setCommandeDateCreation(List<LocalDateTime> commandeDateCreation) {
		this.commandeDateCreation = commandeDateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.ClientAvecCommandes ClientAvecCommandes}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		DEPARTEMENT_CODE(String.class),
		DATE_CREATION(LocalDateTime.class),
		EMAIL(String.class),
		SWILE_CARD_ID(Integer.class),
		AVIS_CLIENTS(List.class),
		COMMANDE_ID(List.class),
		COMMANDE_DATE_COMMANDE(List.class),
		COMMANDE_DATE_LIVRAISON(List.class),
		COMMANDE_MONTANT_TOTAL(List.class),
		COMMANDE_CLIENT_ID(List.class),
		COMMANDE_TABLE_ID(List.class),
		COMMANDE_RESERVATION_ID(List.class),
		COMMANDE_STATUT_COMMANDE(List.class),
		COMMANDE_AVIS_CLIENT_ID(List.class),
		COMMANDE_LIGNES(List.class),
		COMMANDE_DATE_CREATION(List.class);

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

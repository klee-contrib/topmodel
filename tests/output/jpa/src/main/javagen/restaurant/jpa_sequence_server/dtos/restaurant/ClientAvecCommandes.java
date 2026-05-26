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
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

import restaurant.jpa_sequence_server.enums.restaurant.StatutCommande;

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
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	private String departementCode;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Personne#getDateCreation() Personne#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String email;

	/**
	 * Association réciproque de AvisClient.Client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private List<Integer> commandeId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private List<LocalDateTime> commandeDateCommande;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private List<LocalDateTime> commandeDateLivraison;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getMontantTotal() Commande#getMontantTotal()}
	 */
	@NotNull
	private List<BigDecimal> commandeMontantTotal;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private List<Integer> commandeClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getTableId() Commande#getTableId()}
	 */
	private List<Integer> commandeTableId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getReservation() Commande#getReservation()}
	 */
	private List<Integer> commandeReservationId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private List<StatutCommande> commandeStatutCommande;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getAvisClient() Commande#getAvisClient()}
	 */
	private List<Integer> commandeAvisClientId;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getLignes() Commande#getLignes()}
	 */
	@NotNull
	private List<List<Integer>> commandeLignes;

	/**
	 * Liste des commandes du client.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Commande#getDateCreation() Commande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private List<LocalDateTime> commandeDateCreation;

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
	 * Getter for departementCode.
	 *
	 * @return value of {@link #departementCode departementCode}.
	 */
	public String getDepartementCode() {
		return this.departementCode;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
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
	 * Getter for commandeStatutCommande.
	 *
	 * @return value of {@link #commandeStatutCommande commandeStatutCommande}.
	 */
	public List<StatutCommande> getCommandeStatutCommande() {
		return this.commandeStatutCommande;
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
	 * Getter for commandeDateCreation.
	 *
	 * @return value of {@link #commandeDateCreation commandeDateCreation}.
	 */
	public List<LocalDateTime> getCommandeDateCreation() {
		return this.commandeDateCreation;
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
	 * Set the value of {@link #departementCode departementCode}.
	 * @param departementCode value to set.
	 */
	public void setDepartementCode(String departementCode) {
		this.departementCode = departementCode;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
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
	 * Set the value of {@link #commandeStatutCommande commandeStatutCommande}.
	 * @param commandeStatutCommande value to set.
	 */
	public void setCommandeStatutCommande(List<StatutCommande> commandeStatutCommande) {
		this.commandeStatutCommande = commandeStatutCommande;
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
	 * Set the value of {@link #commandeDateCreation commandeDateCreation}.
	 * @param commandeDateCreation value to set.
	 */
	public void setCommandeDateCreation(List<LocalDateTime> commandeDateCreation) {
		this.commandeDateCreation = commandeDateCreation;
	}
}

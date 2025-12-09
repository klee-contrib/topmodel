////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_enums.entities.restaurant.Client;
import restaurant.jpa_identity_enums.entities.restaurant.RestaurantMappers;

/**
 * Détail d'un client en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getId() Client#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getNom() Client#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getPrenom() Client#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Numéro de téléphone du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getTelephone() Client#getTelephone()}
	 */
	@Size(max = 20)
	private String telephone;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String email;

	/**
	 * Association réciproque de Commande.ClientId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getCommandes() Client#getCommandes()}
	 */
	@NotNull
	private List<Integer> commandes;

	/**
	 * Association réciproque de AvisClient.ClientIdClient.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getAvisClientsClient() Client#getAvisClientsClient()}
	 */
	@NotNull
	private List<Integer> avisClientsClient;

	/**
	 * Association réciproque de Reservation.ClientIdClient.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getReservationsClient() Client#getReservationsClient()}
	 */
	@NotNull
	private List<Integer> reservationsClient;

	/**
	 * No arg constructor.
	 */
	public ClientRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'ClientRead'.
	 * @param client Instance de 'Client'.
	 *
	 * @return Une nouvelle instance de 'ClientRead'.
	 */
	public ClientRead(Client client) {
		RestaurantMappers.mapClientRead(client, this);
	}

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
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.ClientRead ClientRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		TELEPHONE(String.class),
		EMAIL(String.class),
		COMMANDES(List.class),
		AVIS_CLIENTS_CLIENT(List.class),
		RESERVATIONS_CLIENT(List.class);

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

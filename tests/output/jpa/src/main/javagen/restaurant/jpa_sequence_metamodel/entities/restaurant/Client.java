////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Client du restaurant.
 */
@Entity
@Table(name = "CLIENT")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Client {

	/**
	 * Identifiant du client.
	 */
	@Id
	@Column(name = "CLI_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_CLIENT")
	@SequenceGenerator(sequenceName = "SEQ_CLIENT", name = "SEQ_CLIENT", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du client.
	 */
	@Column(name = "CLI_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Prénom du client.
	 */
	@Column(name = "CLI_PRENOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String prenom;

	/**
	 * Numéro de téléphone du client.
	 */
	@Column(name = "CLI_TELEPHONE", length = 20, columnDefinition = "varchar")
	private String telephone;

	/**
	 * Adresse email du client.
	 */
	@Column(name = "CLI_EMAIL", length = 100, columnDefinition = "varchar")
	private String email;

	/**
	 * Association réciproque de Commande.ClientId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "client")
	private List<Commande> commandes;

	/**
	 * Association réciproque de AvisClient.ClientIdClient.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "clientClient")
	private List<AvisClient> avisClientsClient;

	/**
	 * Association réciproque de Reservation.ClientIdClient.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "clientClient")
	private List<Reservation> reservationsClient;

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
	public List<Commande> getCommandes() {
		if (this.commandes == null) {
			this.commandes = new ArrayList<>();
		}
		return this.commandes;
	}

	/**
	 * Getter for avisClientsClient.
	 *
	 * @return value of {@link #avisClientsClient avisClientsClient}.
	 */
	public List<AvisClient> getAvisClientsClient() {
		if (this.avisClientsClient == null) {
			this.avisClientsClient = new ArrayList<>();
		}
		return this.avisClientsClient;
	}

	/**
	 * Getter for reservationsClient.
	 *
	 * @return value of {@link #reservationsClient reservationsClient}.
	 */
	public List<Reservation> getReservationsClient() {
		if (this.reservationsClient == null) {
			this.reservationsClient = new ArrayList<>();
		}
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
	public void setCommandes(List<Commande> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #avisClientsClient avisClientsClient}.
	 * @param avisClientsClient value to set.
	 */
	public void setAvisClientsClient(List<AvisClient> avisClientsClient) {
		this.avisClientsClient = avisClientsClient;
	}

	/**
	 * Set the value of {@link #reservationsClient reservationsClient}.
	 * @param reservationsClient value to set.
	 */
	public void setReservationsClient(List<Reservation> reservationsClient) {
		this.reservationsClient = reservationsClient;
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#commandes commandes}.
	 * @param commande value to add to client.
	 */
	void addCommande(Commande commande) {
		this.commandes.add(commande);
		commande.setClient(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#avisClientsClient avisClientsClient}.
	 * @param avisClient value to add to clientClient.
	 */
	void addAvisClientClient(AvisClient avisClient) {
		this.avisClientsClient.add(avisClient);
		avisClient.setClientClient(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#reservationsClient reservationsClient}.
	 * @param reservation value to add to clientClient.
	 */
	void addReservationClient(Reservation reservation) {
		this.reservationsClient.add(reservation);
		reservation.setClientClient(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#commandes commandes}.
	 * @param commande commande value to remove.
	 */
	void removeCommande(Commande commande) {
		this.commandes.remove(commande);
		commande.setClient(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#avisClientsClient avisClientsClient}.
	 * @param avisClient avisClient value to remove.
	 */
	void removeAvisClientClient(AvisClient avisClient) {
		this.avisClientsClient.remove(avisClient);
		avisClient.setClientClient(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client#reservationsClient reservationsClient}.
	 * @param reservation reservation value to remove.
	 */
	void removeReservationClient(Reservation reservation) {
		this.reservationsClient.remove(reservation);
		reservation.setClientClient(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Client Client}.
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

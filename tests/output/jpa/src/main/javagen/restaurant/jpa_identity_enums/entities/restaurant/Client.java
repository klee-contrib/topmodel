////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.entities.restaurant;

import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

/**
 * Client du restaurant.
 */
@Entity
@Table(name = "CLIENT")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Client extends Personne {

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
	 * Association réciproque de Reservation.ClientId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "client")
	private List<Reservation> reservations;

	/**
	 * Association réciproque de AvisClient.ClientId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "client")
	private List<AvisClient> avisClients;

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
	 * Getter for reservations.
	 *
	 * @return value of {@link #reservations reservations}.
	 */
	public List<Reservation> getReservations() {
		if (this.reservations == null) {
			this.reservations = new ArrayList<>();
		}
		return this.reservations;
	}

	/**
	 * Getter for avisClients.
	 *
	 * @return value of {@link #avisClients avisClients}.
	 */
	public List<AvisClient> getAvisClients() {
		if (this.avisClients == null) {
			this.avisClients = new ArrayList<>();
		}
		return this.avisClients;
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
	 * Set the value of {@link #reservations reservations}.
	 * @param reservations value to set.
	 */
	public void setReservations(List<Reservation> reservations) {
		this.reservations = reservations;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<AvisClient> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_enums.entities.restaurant.Client#commandes commandes}.
	 * @param commande value to add to client.
	 */
	void addCommande(Commande commande) {
		this.commandes.add(commande);
		commande.setClient(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_enums.entities.restaurant.Client#reservations reservations}.
	 * @param reservation value to add to client.
	 */
	void addReservation(Reservation reservation) {
		this.reservations.add(reservation);
		reservation.setClient(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_enums.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient value to add to client.
	 */
	void addAvisClient(AvisClient avisClient) {
		this.avisClients.add(avisClient);
		avisClient.setClient(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_enums.entities.restaurant.Client#commandes commandes}.
	 * @param commande commande value to remove.
	 */
	void removeCommande(Commande commande) {
		this.commandes.remove(commande);
		commande.setClient(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_enums.entities.restaurant.Client#reservations reservations}.
	 * @param reservation reservation value to remove.
	 */
	void removeReservation(Reservation reservation) {
		this.reservations.remove(reservation);
		reservation.setClient(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_enums.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient avisClient value to remove.
	 */
	void removeAvisClient(AvisClient avisClient) {
		this.avisClients.remove(avisClient);
		avisClient.setClient(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.entities.restaurant.Client Client}.
	 */
	public enum Fields {
		EMAIL(String.class),
		COMMANDES(List.class),
		RESERVATIONS(List.class),
		AVIS_CLIENTS(List.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un client en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

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
	 * Association réciproque de Commande.ClientId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getCommandes() Client#getCommandes()}
	 */
	@NotNull
	@Column("com_id")
	private List<Integer> commandes;

	/**
	 * Association réciproque de Reservation.ClientId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getReservations() Client#getReservations()}
	 */
	@NotNull
	@Column("rev_id")
	private List<Integer> reservations;

	/**
	 * Association réciproque de AvisClient.ClientId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	@Column("avi_id")
	private List<Integer> avisClients;

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
	 * Getter for commandes.
	 *
	 * @return value of {@link #commandes commandes}.
	 */
	public List<Integer> getCommandes() {
		return this.commandes;
	}

	/**
	 * Getter for reservations.
	 *
	 * @return value of {@link #reservations reservations}.
	 */
	public List<Integer> getReservations() {
		return this.reservations;
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
	 * Set the value of {@link #commandes commandes}.
	 * @param commandes value to set.
	 */
	public void setCommandes(List<Integer> commandes) {
		this.commandes = commandes;
	}

	/**
	 * Set the value of {@link #reservations reservations}.
	 * @param reservations value to set.
	 */
	public void setReservations(List<Integer> reservations) {
		this.reservations = reservations;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientWrite ClientWrite}.
	 */
	public enum Fields {
		NOM(String.class),
		PRENOM(String.class),
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

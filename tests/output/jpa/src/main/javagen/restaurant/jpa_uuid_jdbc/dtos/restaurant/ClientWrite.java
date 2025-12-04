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
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientWrite ClientWrite}.
	 */
	public enum Fields {
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

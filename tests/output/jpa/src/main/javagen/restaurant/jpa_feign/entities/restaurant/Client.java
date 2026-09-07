////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

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
@Table(name = "client")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Client extends Personne {

	/**
	 * Adresse email du client.
	 */
	@Column(name = "cli_email", length = 100, columnDefinition = "varchar")
	private String email;

	/**
	 * Carte Swile du client.
	 */
	@Column(name = "swi_id", columnDefinition = "int")
	private Integer swileCard;

	/**
	 * Association réciproque de AvisClient.Client.
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
	 * Getter for swileCard.
	 *
	 * @return value of {@link #swileCard swileCard}.
	 */
	public Integer getSwileCard() {
		return this.swileCard;
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
	 * Set the value of {@link #swileCard swileCard}.
	 * @param swileCard value to set.
	 */
	public void setSwileCard(Integer swileCard) {
		this.swileCard = swileCard;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<AvisClient> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient value to add to client.
	 */
	void addAvisClient(AvisClient avisClient) {
		this.avisClients.add(avisClient);
		avisClient.setClient(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient avisClient value to remove.
	 */
	void removeAvisClient(AvisClient avisClient) {
		this.avisClients.remove(avisClient);
		avisClient.setClient(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Client Client}.
	 */
	public enum Fields {
		EMAIL(String.class),
		SWILE_CARD(Integer.class),
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

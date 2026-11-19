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
	 * Accesseur de 'email'.
	 *
	 * @return Valeur de {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Accesseur de 'swileCard'.
	 *
	 * @return Valeur de {@link #swileCard swileCard}.
	 */
	public Integer getSwileCard() {
		return this.swileCard;
	}

	/**
	 * Accesseur de 'avisClients'.
	 *
	 * @return Valeur de {@link #avisClients avisClients}.
	 */
	public List<AvisClient> getAvisClients() {
		if (this.avisClients == null) {
			this.avisClients = new ArrayList<>();
		}
		return this.avisClients;
	}

	/**
	 * Définit la valeur de {@link #email email}.
	 * @param email Valeur à définir.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Définit la valeur de {@link #swileCard swileCard}.
	 * @param swileCard Valeur à définir.
	 */
	public void setSwileCard(Integer swileCard) {
		this.swileCard = swileCard;
	}

	/**
	 * Définit la valeur de {@link #avisClients avisClients}.
	 * @param avisClients Valeur à définir.
	 */
	public void setAvisClients(List<AvisClient> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient Valeur à ajouter à 'client'.
	 */
	void addAvisClient(AvisClient avisClient) {
		this.avisClients.add(avisClient);
		avisClient.setClient(this);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Client#avisClients avisClients}.
	 * @param avisClient Valeur de 'avisClient' à supprimer.
	 */
	void removeAvisClient(AvisClient avisClient) {
		this.avisClients.remove(avisClient);
		avisClient.setClient(null);
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Client Client}.
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
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

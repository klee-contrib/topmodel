////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_server.enums.restaurant.DepartementCode;

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
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	private String departementCode = DepartementCode.Paris;

	/**
	 * Adresse email du client.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String email;

	/**
	 * Carte Swile du client.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Client#getSwileCard() Client#getSwileCard()}
	 */
	private Integer swileCardId;

	/**
	 * Association réciproque de AvisClient.Client.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
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
	 * Getter for departementCode.
	 *
	 * @return value of {@link #departementCode departementCode}.
	 */
	public String getDepartementCode() {
		return this.departementCode;
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
	 * Getter for swileCardId.
	 *
	 * @return value of {@link #swileCardId swileCardId}.
	 */
	public Integer getSwileCardId() {
		return this.swileCardId;
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
	 * Set the value of {@link #departementCode departementCode}.
	 * @param departementCode value to set.
	 */
	public void setDepartementCode(String departementCode) {
		this.departementCode = departementCode;
	}

	/**
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #swileCardId swileCardId}.
	 * @param swileCardId value to set.
	 */
	public void setSwileCardId(Integer swileCardId) {
		this.swileCardId = swileCardId;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
	}
}

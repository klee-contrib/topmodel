////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.DepartementCode;

/**
 * Détail d'un client en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom de la personne.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_nom")
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_prenom")
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	@Column("dep_code")
	private String departementCode = DepartementCode.Paris;

	/**
	 * Adresse email du client.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	@Column("cli_email")
	private String email;

	/**
	 * Carte Swile du client.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Client#getSwileCard() Client#getSwileCard()}
	 */
	@Column("swi_id")
	private Integer swileCardId;

	/**
	 * Association réciproque de AvisClient.Client.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Client#getAvisClients() Client#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

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
}

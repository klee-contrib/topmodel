////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un employé en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class EmployeRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la personne.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	private String departementCode;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Personne#getDateCreation() Personne#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Numéro de téléphone de l'employé.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getTelephone() Employe#getTelephone()}
	 */
	@Size(max = 20)
	private String telephone;

	/**
	 * Date de naissance.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getDateNaissance() Employe#getDateNaissance()}
	 */
	private LocalDateTime dateNaissance;

	/**
	 * Matricule de l'employé.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getMatricule() Employe#getMatricule()}
	 */
	@NotNull
	@Size(max = 10)
	private String matricule;

	/**
	 * Date d'embauche.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getDateEmbauche() Employe#getDateEmbauche()}
	 */
	@NotNull
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getSalaire() Employe#getSalaire()}
	 */
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 * Alias of {@link restaurant.jpa_feign.entities.restaurant.Employe#getRestaurant() Employe#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

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
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Getter for dateNaissance.
	 *
	 * @return value of {@link #dateNaissance dateNaissance}.
	 */
	public LocalDateTime getDateNaissance() {
		return this.dateNaissance;
	}

	/**
	 * Getter for matricule.
	 *
	 * @return value of {@link #matricule matricule}.
	 */
	public String getMatricule() {
		return this.matricule;
	}

	/**
	 * Getter for dateEmbauche.
	 *
	 * @return value of {@link #dateEmbauche dateEmbauche}.
	 */
	public LocalDateTime getDateEmbauche() {
		return this.dateEmbauche;
	}

	/**
	 * Getter for salaire.
	 *
	 * @return value of {@link #salaire salaire}.
	 */
	public BigDecimal getSalaire() {
		return this.salaire;
	}

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Set the value of {@link #dateNaissance dateNaissance}.
	 * @param dateNaissance value to set.
	 */
	public void setDateNaissance(LocalDateTime dateNaissance) {
		this.dateNaissance = dateNaissance;
	}

	/**
	 * Set the value of {@link #matricule matricule}.
	 * @param matricule value to set.
	 */
	public void setMatricule(String matricule) {
		this.matricule = matricule;
	}

	/**
	 * Set the value of {@link #dateEmbauche dateEmbauche}.
	 * @param dateEmbauche value to set.
	 */
	public void setDateEmbauche(LocalDateTime dateEmbauche) {
		this.dateEmbauche = dateEmbauche;
	}

	/**
	 * Set the value of {@link #salaire salaire}.
	 * @param salaire value to set.
	 */
	public void setSalaire(BigDecimal salaire) {
		this.salaire = salaire;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.EmployeRead EmployeRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		DEPARTEMENT_CODE(String.class),
		DATE_CREATION(LocalDateTime.class),
		TELEPHONE(String.class),
		DATE_NAISSANCE(LocalDateTime.class),
		MATRICULE(String.class),
		DATE_EMBAUCHE(LocalDateTime.class),
		SALAIRE(BigDecimal.class),
		RESTAURANT_ID(Integer.class);

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

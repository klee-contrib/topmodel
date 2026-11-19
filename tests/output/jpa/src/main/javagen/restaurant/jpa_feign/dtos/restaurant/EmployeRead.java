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
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getDepartementCode() Personne#getDepartementCode()}
	 */
	@Size(max = 10)
	private String departementCode;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Personne#getDateCreation() Personne#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Numéro de téléphone de l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getTelephone() Employe#getTelephone()}
	 */
	@Size(max = 20)
	private String telephone;

	/**
	 * Date de naissance.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getDateNaissance() Employe#getDateNaissance()}
	 */
	private LocalDateTime dateNaissance;

	/**
	 * Matricule de l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getMatricule() Employe#getMatricule()}
	 */
	@NotNull
	@Size(max = 10)
	private String matricule;

	/**
	 * Date d'embauche.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getDateEmbauche() Employe#getDateEmbauche()}
	 */
	@NotNull
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getSalaire() Employe#getSalaire()}
	 */
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getRestaurant() Employe#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Accesseur de 'telephone'.
	 *
	 * @return Valeur de {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Accesseur de 'dateNaissance'.
	 *
	 * @return Valeur de {@link #dateNaissance dateNaissance}.
	 */
	public LocalDateTime getDateNaissance() {
		return this.dateNaissance;
	}

	/**
	 * Accesseur de 'matricule'.
	 *
	 * @return Valeur de {@link #matricule matricule}.
	 */
	public String getMatricule() {
		return this.matricule;
	}

	/**
	 * Accesseur de 'dateEmbauche'.
	 *
	 * @return Valeur de {@link #dateEmbauche dateEmbauche}.
	 */
	public LocalDateTime getDateEmbauche() {
		return this.dateEmbauche;
	}

	/**
	 * Accesseur de 'salaire'.
	 *
	 * @return Valeur de {@link #salaire salaire}.
	 */
	public BigDecimal getSalaire() {
		return this.salaire;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #telephone telephone}.
	 * @param telephone Valeur à définir.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Définit la valeur de {@link #dateNaissance dateNaissance}.
	 * @param dateNaissance Valeur à définir.
	 */
	public void setDateNaissance(LocalDateTime dateNaissance) {
		this.dateNaissance = dateNaissance;
	}

	/**
	 * Définit la valeur de {@link #matricule matricule}.
	 * @param matricule Valeur à définir.
	 */
	public void setMatricule(String matricule) {
		this.matricule = matricule;
	}

	/**
	 * Définit la valeur de {@link #dateEmbauche dateEmbauche}.
	 * @param dateEmbauche Valeur à définir.
	 */
	public void setDateEmbauche(LocalDateTime dateEmbauche) {
		this.dateEmbauche = dateEmbauche;
	}

	/**
	 * Définit la valeur de {@link #salaire salaire}.
	 * @param salaire Valeur à définir.
	 */
	public void setSalaire(BigDecimal salaire) {
		this.salaire = salaire;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.EmployeRead EmployeRead}.
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
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

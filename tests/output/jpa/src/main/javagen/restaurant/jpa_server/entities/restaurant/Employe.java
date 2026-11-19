////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Index;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Employé du restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "employe",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"emp_matricule"})
	},
	indexes = {
		@Index(name = "idx_emp_emp_telephone", columnList = "emp_telephone")
	}
)
public class Employe extends Personne implements EmployeBase {

	/**
	 * Numéro de téléphone de l'employé.
	 */
	@Column(name = "emp_telephone", length = 20, columnDefinition = "varchar")
	private String telephone;

	/**
	 * Date de naissance.
	 */
	@Column(name = "emp_date_naissance", columnDefinition = "timestamp")
	private LocalDateTime dateNaissance;

	/**
	 * Matricule de l'employé.
	 */
	@Column(name = "emp_matricule", nullable = false, length = 10, columnDefinition = "varchar")
	private String matricule;

	/**
	 * Date d'embauche.
	 */
	@Column(name = "emp_date_embauche", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 */
	@Column(name = "emp_salaire", scale = 2, columnDefinition = "decimal")
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

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
	 * Accesseur de 'restaurant'.
	 *
	 * @return Valeur de {@link #restaurant restaurant}.
	 */
	public Restaurant getRestaurant() {
		return this.restaurant;
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
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Employe Employe}.
	 */
	public enum Fields {
		TELEPHONE(String.class),
		DATE_NAISSANCE(LocalDateTime.class),
		MATRICULE(String.class),
		DATE_EMBAUCHE(LocalDateTime.class),
		SALAIRE(BigDecimal.class),
		RESTAURANT(Restaurant.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Employé du restaurant.
 */
@Table(name = "employe")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Employe extends Personne implements EmployeBase {

	/**
	 * Numéro de téléphone de l'employé.
	 */
	@Column("emp_telephone")
	private String telephone;

	/**
	 * Date de naissance.
	 */
	@Column("emp_date_naissance")
	private LocalDateTime dateNaissance;

	/**
	 * Matricule de l'employé.
	 */
	@NotNull
	@Column("emp_matricule")
	private String matricule;

	/**
	 * Date d'embauche.
	 */
	@NotNull
	@Column("emp_date_embauche")
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 */
	@Column("emp_salaire")
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurant;

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
	public Integer getRestaurant() {
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
	public void setRestaurant(Integer restaurant) {
		this.restaurant = restaurant;
	}
}

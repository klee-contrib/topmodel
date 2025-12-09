////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

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
public class Employe extends Personne {

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
	@Column("res_id_restaurant")
	private Integer restaurantIdRestaurant;

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
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
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
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}
}

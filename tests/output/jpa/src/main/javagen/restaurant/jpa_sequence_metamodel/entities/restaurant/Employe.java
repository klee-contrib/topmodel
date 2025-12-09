////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Employé du restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "EMPLOYE", uniqueConstraints = {@UniqueConstraint(columnNames = {"EMP_MATRICULE"})})
public class Employe extends Personne {

	/**
	 * Matricule de l'employé.
	 */
	@Column(name = "EMP_MATRICULE", nullable = false, length = 10, columnDefinition = "varchar")
	private String matricule;

	/**
	 * Date d'embauche.
	 */
	@Column(name = "EMP_DATE_EMBAUCHE", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 */
	@Column(name = "EMP_SALAIRE", scale = 2, columnDefinition = "decimal")
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 */
	@JoinColumn(name = "RES_ID_RESTAURANT", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurantRestaurant;

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
	 * Getter for restaurantRestaurant.
	 *
	 * @return value of {@link #restaurantRestaurant restaurantRestaurant}.
	 */
	public Restaurant getRestaurantRestaurant() {
		return this.restaurantRestaurant;
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
	 * Set the value of {@link #restaurantRestaurant restaurantRestaurant}.
	 * @param restaurantRestaurant value to set.
	 */
	public void setRestaurantRestaurant(Restaurant restaurantRestaurant) {
		this.restaurantRestaurant = restaurantRestaurant;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Employe Employe}.
	 */
	public enum Fields {
		MATRICULE(String.class),
		DATE_EMBAUCHE(LocalDateTime.class),
		SALAIRE(BigDecimal.class),
		RESTAURANT_RESTAURANT(Restaurant.class);

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

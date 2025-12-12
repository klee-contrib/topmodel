////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
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
	 * Matricule de l'employé.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Employe#getMatricule() Employe#getMatricule()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("emp_matricule")
	private String matricule;

	/**
	 * Date d'embauche.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Employe#getDateEmbauche() Employe#getDateEmbauche()}
	 */
	@NotNull
	@Column("emp_date_embauche")
	private LocalDateTime dateEmbauche;

	/**
	 * Salaire de l'employé.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Employe#getSalaire() Employe#getSalaire()}
	 */
	@Column("emp_salaire")
	private BigDecimal salaire;

	/**
	 * Restaurant où travaille l'employé.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Employe#getRestaurantId() Employe#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

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
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.EmployeRead EmployeRead}.
	 */
	public enum Fields {
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

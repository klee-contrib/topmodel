////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

/**
 * Détail d'un employé en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class EmployeItem extends PersonneItem implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Matricule de l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getMatricule() Employe#getMatricule()}
	 */
	@NotNull
	@Size(max = 10)
	private String matricule;

	/**
	 * Restaurant où travaille l'employé.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Employe#getRestaurant() Employe#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Liste des autres employés.
	 */
	@Valid
	@NotNull
	private List<EmployeItem> autresEmployes;

	/**
	 * Accesseur de 'matricule'.
	 *
	 * @return Valeur de {@link #matricule matricule}.
	 */
	public String getMatricule() {
		return this.matricule;
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
	 * Accesseur de 'autresEmployes'.
	 *
	 * @return Valeur de {@link #autresEmployes autresEmployes}.
	 */
	public List<EmployeItem> getAutresEmployes() {
		return this.autresEmployes;
	}

	/**
	 * Définit la valeur de {@link #matricule matricule}.
	 * @param matricule Valeur à définir.
	 */
	public void setMatricule(String matricule) {
		this.matricule = matricule;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #autresEmployes autresEmployes}.
	 * @param autresEmployes Valeur à définir.
	 */
	public void setAutresEmployes(List<EmployeItem> autresEmployes) {
		this.autresEmployes = autresEmployes;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.EmployeItem EmployeItem}.
	 */
	public enum Fields {
		MATRICULE(String.class),
		RESTAURANT_ID(Integer.class),
		AUTRES_EMPLOYES(List.class);

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

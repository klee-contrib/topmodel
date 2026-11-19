////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une personne en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class PersonneItem implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de l'employé.
	 */
	private Integer id;

	/**
	 * Nom de l'employé.
	 */
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom de l'employé.
	 */
	@Size(max = 100)
	private String prenom;

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
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.PersonneItem PersonneItem}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Index;
import jakarta.persistence.Table;

/**
 * Prestaire du restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "prestataire",
	indexes = {
		@Index(name = "idx_pst_pst_nom_pst_prenom", columnList = "pst_nom, pst_prenom"),
		@Index(name = "idx_pst_pst_telephone", columnList = "pst_telephone")
	}
)
public class Prestataire implements EmployeBase {

	/**
	 * Identifiant de la personne.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "pst_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Nom de la personne.
	 */
	@Column(name = "pst_nom", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Prénom de la personne.
	 */
	@Column(name = "pst_prenom", nullable = false, length = 100, columnDefinition = "varchar")
	private String prenom;

	/**
	 * Numéro de téléphone de l'employé.
	 */
	@Column(name = "pst_telephone", length = 20, columnDefinition = "varchar")
	private String telephone;

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
	 * Accesseur de 'telephone'.
	 *
	 * @return Valeur de {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
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
	 * Définit la valeur de {@link #telephone telephone}.
	 * @param telephone Valeur à définir.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Prestataire Prestataire}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		TELEPHONE(String.class);

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

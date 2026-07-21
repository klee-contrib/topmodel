////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

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
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
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
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Prestataire Prestataire}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

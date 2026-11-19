////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorColumn;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Inheritance;
import jakarta.persistence.InheritanceType;
import jakarta.persistence.Table;

/**
 * Lieu.
 */
@Entity
@Table(name = "lieu")
@DiscriminatorColumn(name = "lie_discriminator")
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class Lieu {

	/**
	 * Identifiant du restaurant.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "lie_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Nom du restaurant.
	 */
	@Column(name = "lie_nom", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Adresse du restaurant.
	 */
	@Column(name = "lie_adresse", length = 100, columnDefinition = "varchar")
	private String adresse;

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
	 * Accesseur de 'adresse'.
	 *
	 * @return Valeur de {@link #adresse adresse}.
	 */
	public String getAdresse() {
		return this.adresse;
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
	 * Définit la valeur de {@link #adresse adresse}.
	 * @param adresse Valeur à définir.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Lieu Lieu}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		ADRESSE(String.class);

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

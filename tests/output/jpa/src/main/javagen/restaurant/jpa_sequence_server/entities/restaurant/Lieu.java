////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorColumn;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Inheritance;
import jakarta.persistence.InheritanceType;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Lieu.
 */
@Entity
@Table(name = "LIEU")
@DiscriminatorColumn(name = "LIE_DISCRIMINATOR")
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class Lieu {

	/**
	 * Identifiant du restaurant.
	 */
	@Id
	@Column(name = "LIE_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_LIEU")
	@SequenceGenerator(sequenceName = "SEQ_LIEU", name = "SEQ_LIEU", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du restaurant.
	 */
	@Column(name = "LIE_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Adresse du restaurant.
	 */
	@Column(name = "LIE_ADRESSE", length = 100, columnDefinition = "varchar")
	private String adresse;

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
	 * Getter for adresse.
	 *
	 * @return value of {@link #adresse adresse}.
	 */
	public String getAdresse() {
		return this.adresse;
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
	 * Set the value of {@link #adresse adresse}.
	 * @param adresse value to set.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.Lieu Lieu}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

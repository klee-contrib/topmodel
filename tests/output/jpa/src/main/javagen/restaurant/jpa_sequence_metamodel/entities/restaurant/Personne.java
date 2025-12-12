////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Inheritance;
import jakarta.persistence.InheritanceType;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Classe de base représentant une personne.
 */
@Entity
@Table(name = "PERSONNE")
@Inheritance(strategy = InheritanceType.JOINED)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Personne {

	/**
	 * Identifiant de la personne.
	 */
	@Id
	@Column(name = "PER_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PERSONNE")
	@SequenceGenerator(sequenceName = "SEQ_PERSONNE", name = "SEQ_PERSONNE", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom de la personne.
	 */
	@Column(name = "PER_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Prénom de la personne.
	 */
	@Column(name = "PER_PRENOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String prenom;

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
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Personne Personne}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

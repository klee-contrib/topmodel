////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

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

import restaurant.jpa_sequence_server.enums.restaurant.DepartementCode;

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
	@Column(columnDefinition = "int", name = "PER_ID", nullable = false)
	@GeneratedValue(generator = "SEQ_PERSONNE", strategy = GenerationType.SEQUENCE)
	@SequenceGenerator(allocationSize = 50, initialValue = 1000, name = "SEQ_PERSONNE", sequenceName = "SEQ_PERSONNE")
	private Integer id;

	/**
	 * Nom de la personne.
	 */
	@Column(columnDefinition = "varchar", length = 100, name = "PER_NOM", nullable = false)
	private String nom;

	/**
	 * Prénom de la personne.
	 */
	@Column(columnDefinition = "varchar", length = 100, name = "PER_PRENOM", nullable = false)
	private String prenom;

	/**
	 * Département de résidence de la personne.
	 */
	@Column(columnDefinition = "varchar", length = 10, name = "DEP_CODE")
	private String departementCode = DepartementCode.Paris;

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
	 * Getter for departementCode.
	 *
	 * @return value of {@link #departementCode departementCode}.
	 */
	public String getDepartementCode() {
		return this.departementCode;
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
	 * Set the value of {@link #departementCode departementCode}.
	 * @param departementCode value to set.
	 */
	public void setDepartementCode(String departementCode) {
		this.departementCode = departementCode;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.Personne Personne}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		DEPARTEMENT_CODE(String.class);

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

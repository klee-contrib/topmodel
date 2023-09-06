////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

/**
 * Secteur d'application du profil.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "SECTEUR")
public class Secteur {

	/**
	 * Id technique.
	 */
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Id
	@Column(name = "SEC_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Association réciproque de Profil.Secteurs.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	private Profil profil;

	/**
	 * No arg constructor.
	 */
	public Secteur() {
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Secteur#id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Secteur#profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Secteur#id id}.
	 * @param id value to set
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Secteur#profil profil}.
	 * @param profil value to set
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.Secteur Secteur}.
	 */
	public enum Fields  {
        ID(Integer.class), //
        PROFIL(Profil.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}

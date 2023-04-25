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
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "SEC_ID", nullable = false)
	private Long id;

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
	 * Copy constructor.
	 * @param secteur to copy
	 */
	public Secteur(Secteur secteur) {
		if(secteur == null) {
			return;
		}

		this.id = secteur.getId();
		this.profil = secteur.getProfil();
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param profil Association réciproque de Profil.Secteurs
	 */
	public Secteur(Long id, Profil profil) {
		this.id = id;
		this.profil = profil;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Secteur#id id}.
	 */
	public Long getId() {
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
	public void setId(Long id) {
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
        ID(Long.class), //
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

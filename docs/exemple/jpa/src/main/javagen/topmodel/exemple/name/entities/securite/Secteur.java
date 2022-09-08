////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.entities.securite;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

import topmodel.exemple.utils.IFieldEnum;

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
	@SequenceGenerator(name = "SEQ_SECTEUR", sequenceName = "SEQ_SECTEUR", initialValue = 1000)
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_SECTEUR")
	@Column(name = "SEC_ID", nullable = false)
	private long id;

	/**
	 * Association réciproque de {@link topmodel.exemple.name.entities.securite.Profil#secteurs Profil.secteurs}.
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
	 * @param profil Association réciproque de {@link topmodel.exemple.name.entities.securite.Profil#secteurs Profil.secteurs}
	 */
	public Secteur(long id, Profil profil) {
		this.id = id;
		this.profil = profil;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Secteur#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Secteur#profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Secteur#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Secteur#profil profil}.
	 * @param profil value to set
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Equal function comparing Id.
	 */
	public boolean equals(Object o) {
		if(o instanceof Secteur) {
			Secteur secteur = (Secteur) o;
			if(this == secteur)
				return true;

			if(secteur == null || this.getId() == null)
				return false;

			return this.getId().equals(secteur.getId());
		}

		return false;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.entities.securite.Secteur Secteur}.
	 */
	public enum Fields implements IFieldEnum<Secteur> {
        ID, //
        PROFIL
	}
}

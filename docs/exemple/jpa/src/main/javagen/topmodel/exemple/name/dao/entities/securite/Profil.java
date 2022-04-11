////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.securite;

import java.io.Serializable;

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

import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Profil des utilisateurs.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "PROFIL")
public class Profil implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 */
	@Id
	@SequenceGenerator(name = "SEQ_PROFIL", sequenceName = "SEQ_PROFIL",  initialValue = 1000, allocationSize = 1)
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PROFIL")
	@Column(name = "PRO_ID", nullable = false)
	private long id;

	/**
	 * Type de profil.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "CODE", referencedColumnName = "CODE")
	private TypeProfil typeProfil;

	/**
	 * No arg constructor.
	 */
	public Profil() {
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param typeProfil Type de profil
	 */
	public Profil(long id, TypeProfil typeProfil) {
		this.id = id;
		this.typeProfil = typeProfil;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.securite.Profil#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.securite.Profil#typeProfil typeProfil}.
	 */
	protected TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.securite.Profil#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.securite.Profil#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Equal function comparing Id.
	 */
	public boolean equals(Object o) {
		if(o instanceof Profil profil) {
			if(this == profil)
				return true;

			if(profil == null || this.getId() == null)
				return false;

			return this.getId().equals(profil.getId());
		}
		return false;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.dao.entities.securite.Profil Profil}.
	 */
	public enum Fields implements IFieldEnum<Profil> {
        ID, //
        TYPE_PROFIL
	}
}

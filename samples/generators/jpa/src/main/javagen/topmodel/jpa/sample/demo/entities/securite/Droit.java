////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

import topmodel.jpa.sample.demo.enums.securite.DroitCode;

/**
 * Droits de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "DROIT")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class Droit {

	/**
	 * Code du droit.
	 */
	@Id
	@Column(name = "DRO_CODE", nullable = false, length = 3, columnDefinition = "varchar")
	@Enumerated(EnumType.STRING)
	private DroitCode code;

	/**
	 * Libellé du droit.
	 */
	@Column(name = "DRO_LIBELLE", nullable = false, length = 3, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Type de profil pouvant faire l'action.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "TPR_CODE", referencedColumnName = "TPR_CODE")
	private TypeProfil typeProfil;

	/**
	 * No arg constructor.
	 */
	public Droit() {
	}

	public static final Droit CRE = new Droit(DroitCode.CRE);
	public static final Droit MOD = new Droit(DroitCode.MOD);
	public static final Droit SUP = new Droit(DroitCode.SUP);

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public Droit(DroitCode code) {
		this.code = code;
		switch(code) {
		case CRE :
			this.libelle = "securite.droit.values.CRE";
			this.typeProfil = TypeProfil.ADM;
			break;
		case MOD :
			this.libelle = "securite.droit.values.MOD";
			this.typeProfil = null;
			break;
		case SUP :
			this.libelle = "securite.droit.values.SUP";
			this.typeProfil = null;
			break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#code code}.
	 */
	public DroitCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#typeProfil typeProfil}.
	 */
	public TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#code code}.
	 * @param code value to set
	 */
	public void setCode(DroitCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.Droit Droit}.
	 */
	public enum Fields  {
        CODE(DroitCode.class), //
        LIBELLE(String.class), //
        TYPE_PROFIL(TypeProfil.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}

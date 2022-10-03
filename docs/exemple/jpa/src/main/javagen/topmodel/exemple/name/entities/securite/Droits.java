////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.entities.securite;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Enumerated;
import javax.persistence.EnumType;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import topmodel.exemple.name.entities.securite.Droits;
import topmodel.exemple.name.entities.securite.TypeProfil;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Droits de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "DROITS")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Immutable
public class Droits {

	/**
	 * Code du droit.
	 */
	@Id
	@Column(name = "DRO_CODE", nullable = false, updatable = false, length = 3)
	@Enumerated(EnumType.STRING)
	private Droits.Values code;

	/**
	 * Libellé du droit.
	 */
	@Column(name = "DRO_LIBELLE", nullable = false, updatable = false, length = 3)
	private String libelle;

	/**
	 * Type de profil pouvant faire l'action.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "TPR_CODE", referencedColumnName = "DRO_CODE")
	private TypeProfil typeProfil;

	/**
	 * No arg constructor.
	 */
	public Droits() {
	}

	/**
	 * Copy constructor.
	 * @param droits to copy
	 */
	public Droits(Droits droits) {
		if(droits == null) {
			return;
		}

		this.code = droits.getCode();
		this.libelle = droits.getLibelle();
		this.typeProfil = droits.getTypeProfil();
	}

	/**
	 * All arg constructor.
	 * @param code Code du droit
	 * @param libelle Libellé du droit
	 * @param typeProfil Type de profil pouvant faire l'action
	 */
	public Droits(Droits.Values code, String libelle, TypeProfil typeProfil) {
		this.code = code;
		this.libelle = libelle;
		this.typeProfil = typeProfil;
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Droits#code code}.
	 */
	public Droits.Values getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Droits#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Droits#typeProfil typeProfil}.
	 */
	public TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Droits#code code}.
	 * @param code value to set
	 */
	public void setCode(Droits.Values code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Droits#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Droits#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Equal function comparing Code.
	 */
	public boolean equals(Object o) {
		if(o instanceof Droits) {
			Droits droits = (Droits) o;
			if(this == droits)
				return true;

			if(droits == null || this.getCode() == null)
				return false;

			return this.getCode().equals(droits.getCode());
		}

		return false;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.entities.securite.Droits Droits}.
	 */
	public enum Fields implements IFieldEnum<Droits> {
        CODE(Droits.Values.class), //
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

	public enum Values {
		CRE("Créer", TypeProfilCodeAlternatif.POT), //
		MOD("Modifier", null), //
		SUP("Supprimer", null); 

		/**
		 * Libellé du droit.
		 */
		private final String libelle;

		/**
		 * Type de profil pouvant faire l'action.
		 */
		private final TypeProfil.Values typeProfilCode;

		/**
		 * All arg constructor.
		 */
		private Values(String libelle, TypeProfil.Values typeProfilCode) {
			this.libelle = libelle;
			this.typeProfilCode = typeProfilCode;
		}

		/**
		 * Libellé du droit.
		 */
		public String getLibelle(){
			return this.libelle;
		}

		/**
		 * Type de profil pouvant faire l'action.
		 */
		public TypeProfil.Values getTypeProfilCode(){
			return this.typeProfilCode;
		}
	}
}

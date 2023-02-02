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

/**
 * Droits de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "DROITS")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class Droits {

	/**
	 * Code du droit.
	 */
	@Id
	@Column(name = "DRO_CODE", nullable = false, length = 3)
	@Enumerated(EnumType.STRING)
	private Droits.Values code;

	/**
	 * Libellé du droit.
	 */
	@Column(name = "DRO_LIBELLE", nullable = false, length = 3)
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
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#code code}.
	 */
	public Droits.Values getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#typeProfil typeProfil}.
	 */
	public TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#code code}.
	 * @param code value to set
	 */
	public void setCode(Droits.Values code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Droits#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.Droits Droits}.
	 */
	public enum Fields  {
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
		CRE("securite.droits.values.CRE", TypeProfil.Values.ADM), //
		MOD("securite.droits.values.MOD", null), //
		SUP("securite.droits.values.SUP", null); 

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
		 * Méthode permettant de récupérer l'entité correspondant au code.
		 *
		 * @return instance de {@link topmodel.jpa.sample.demo.entities.securite.Droits} correspondant au code courant.
		 */
		public Droits getEntity() {
			return new Droits(this, libelle, typeProfilCode.getEntity());
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
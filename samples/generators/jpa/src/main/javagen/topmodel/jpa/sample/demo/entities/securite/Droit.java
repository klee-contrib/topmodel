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
@Table(name = "DROIT")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class Droit {

	/**
	 * Code du droit.
	 */
	@Id
	@Column(name = "DRO_CODE", nullable = false, length = 3)
	@Enumerated(EnumType.STRING)
	private Droit.Values code;

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
	public Droit() {
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Droit#code code}.
	 */
	public Droit.Values getCode() {
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
	public void setCode(Droit.Values code) {
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
        CODE(Droit.Values.class), //
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
		CRE("securite.droit.values.CRE", TypeProfil.Values.ADM), //
		MOD("securite.droit.values.MOD", null), //
		SUP("securite.droit.values.SUP", null); 

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
		 * @return instance de {@link topmodel.jpa.sample.demo.entities.securite.Droit} correspondant au code courant.
		 */
		public Droit getEntity() {
			Droit entity = new Droit();
			entity.code = this;
			entity.libelle = this.libelle;
			entity.typeProfilCode = this.typeProfilCode;
			return entity;
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

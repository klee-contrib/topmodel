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
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import topmodel.jpa.sample.demo.enums.securite.TypeProfilCode;

/**
 * Type d'utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_PROFIL")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeProfil {

	/**
	 * Code du type d'utilisateur.
	 */
	@Id
	@Column(name = "TPR_CODE", nullable = false, length = 3, columnDefinition = "varchar")
	@Enumerated(EnumType.STRING)
	private TypeProfilCode code;

	/**
	 * Libellé du type d'utilisateur.
	 */
	@Column(name = "TPR_LIBELLE", nullable = false, length = 3, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeProfil() {
	}

	public static final TypeProfil ADM = new TypeProfil(TypeProfilCode.ADM);
	public static final TypeProfil GES = new TypeProfil(TypeProfilCode.GES);

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public TypeProfil(TypeProfilCode code) {
		this.code = code;
		switch(code) {
		case ADM :
			this.libelle = "securite.typeProfil.values.ADM";
			break;
		case GES :
			this.libelle = "securite.typeProfil.values.GES";
			break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#code code}.
	 */
	public TypeProfilCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeProfilCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil TypeProfil}.
	 */
	public enum Fields  {
        CODE(TypeProfilCode.class), //
        LIBELLE(String.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}

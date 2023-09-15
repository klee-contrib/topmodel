////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

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

import topmodel.jpa.sample.demo.enums.securite.profil.TypeDroitCode;

/**
 * Type de droit.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_DROIT")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeDroit {

	/**
	 * Code du type de droit.
	 */
	@Id
	@Column(name = "TDR_CODE", nullable = false, length = 3, columnDefinition = "varchar")
	@Enumerated(EnumType.STRING)
	private TypeDroitCode code;

	/**
	 * Libellé du type de droit.
	 */
	@Column(name = "TDR_LIBELLE", nullable = false, length = 3, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeDroit() {
	}

	public static final TypeDroit ADMIN = new TypeDroit(TypeDroitCode.ADMIN);
	public static final TypeDroit READ = new TypeDroit(TypeDroitCode.READ);
	public static final TypeDroit WRITE = new TypeDroit(TypeDroitCode.WRITE);

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public TypeDroit(TypeDroitCode code) {
		this.code = code;
		switch(code) {
		case ADMIN :
			this.libelle = "securite.profil.typeDroit.values.Admin";
			break;
		case READ :
			this.libelle = "securite.profil.typeDroit.values.Read";
			break;
		case WRITE :
			this.libelle = "securite.profil.typeDroit.values.Write";
			break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit#code code}.
	 */
	public TypeDroitCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeDroitCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit TypeDroit}.
	 */
	public enum Fields  {
        CODE(TypeDroitCode.class), //
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

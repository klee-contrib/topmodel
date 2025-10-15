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
import jakarta.persistence.Transient;

import topmodel.jpa.sample.demo.enums.securite.profil.TypeDroitCode;

/**
 * Type de droit.
 */
@Entity
@Immutable
@Table(name = "TYPE_DROIT")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TypeDroit {

	@Transient
	public static final TypeDroit ADMIN = new TypeDroit(TypeDroitCode.ADMIN);

	@Transient
	public static final TypeDroit READ = new TypeDroit(TypeDroitCode.READ);

	@Transient
	public static final TypeDroit WRITE = new TypeDroit(TypeDroitCode.WRITE);

	/**
	 * Code du type de droit.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "TDR_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private TypeDroitCode code;

	/**
	 * Libellé du type de droit.
	 */
	@Column(name = "TDR_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeDroit() {
		// No arg constructor
	}

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public TypeDroit(TypeDroitCode code) {
		this.code = code;
		switch(code) {
			case ADMIN:
				this.libelle = "securite.profil.typeDroit.values.Admin";
				break;
			case READ:
				this.libelle = "securite.profil.typeDroit.values.Read";
				break;
			case WRITE:
				this.libelle = "securite.profil.typeDroit.values.Write";
				break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public TypeDroitCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit TypeDroit}.
	 */
	public enum Fields {
		CODE(TypeDroitCode.class),
		LIBELLE(String.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.utilisateur;

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

import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

/**
 * Type d'utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_UTILISATEUR")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeUtilisateur {

	/**
	 * Code du type d'utilisateur.
	 */
	@Id
	@Column(name = "TUT_CODE", nullable = false, length = 3, columnDefinition = "varchar")
	@Enumerated(EnumType.STRING)
	private TypeUtilisateurCode code;

	/**
	 * Libellé du type d'utilisateur.
	 */
	@Column(name = "TUT_LIBELLE", nullable = false, length = 3, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeUtilisateur() {
	}

	public static final TypeUtilisateur ADMIN = new TypeUtilisateur(TypeUtilisateurCode.ADMIN);
	public static final TypeUtilisateur CLIENT = new TypeUtilisateur(TypeUtilisateurCode.CLIENT);
	public static final TypeUtilisateur GEST = new TypeUtilisateur(TypeUtilisateurCode.GEST);

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public TypeUtilisateur(TypeUtilisateurCode code) {
		this.code = code;
		switch(code) {
		case ADMIN :
			this.libelle = "securite.utilisateur.typeUtilisateur.values.Admin";
			break;
		case CLIENT :
			this.libelle = "securite.utilisateur.typeUtilisateur.values.Client";
			break;
		case GEST :
			this.libelle = "securite.utilisateur.typeUtilisateur.values.Gestionnaire";
			break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur#code code}.
	 */
	public TypeUtilisateurCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeUtilisateurCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur TypeUtilisateur}.
	 */
	public enum Fields  {
        CODE(TypeUtilisateurCode.class), //
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

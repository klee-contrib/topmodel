////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.utilisateur;

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

import topmodel.jpa.sample.demo.enums.utilisateur.TypeUtilisateurCode;

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

	public static final TypeUtilisateur ADM = new TypeUtilisateur(TypeUtilisateurCode.ADM);
	public static final TypeUtilisateur CLI = new TypeUtilisateur(TypeUtilisateurCode.CLI);
	public static final TypeUtilisateur GES = new TypeUtilisateur(TypeUtilisateurCode.GES);

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public TypeUtilisateur(TypeUtilisateurCode code) {
		this.code = code;
		switch(code) {
		case ADM :
			this.libelle = "utilisateur.typeUtilisateur.values.ADM";
			break;
		case CLI :
			this.libelle = "utilisateur.typeUtilisateur.values.CLI";
			break;
		case GES :
			this.libelle = "utilisateur.typeUtilisateur.values.GES";
			break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur#code code}.
	 */
	public TypeUtilisateurCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeUtilisateurCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur TypeUtilisateur}.
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

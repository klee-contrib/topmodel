////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.securite;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Enumerated;
import javax.persistence.EnumType;
import javax.persistence.Id;
import javax.persistence.Table;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Type d'utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_PROFIL")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Immutable
public class TypeProfil implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Code du type d'utilisateur.
	 */
	@Id
	@Column(name = "CODE", nullable = false, updatable = false, length = 3)
	@Enumerated(EnumType.STRING)
	private TypeProfil.Values code;

	/**
	 * Libellé du type d'utilisateur.
	 */
	@Column(name = "LIBELLE", nullable = false, updatable = false, length = 3)
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeProfil() {
	}

	/**
	 * All arg constructor.
	 * @param code Code du type d'utilisateur
	 * @param libelle Libellé du type d'utilisateur
	 */
	public TypeProfil(TypeProfil.Values code, String libelle) {
		this.code = code;
		this.libelle = libelle;
	}

	/**
	 * Alias constructor.
	 * Ce constructeur permet d'initialiser un objet TypeProfil avec comme paramètres les classes dont les propriétés sont référencées TypeProfil.
	 * A ne pas utiliser pour construire un Dto en plusieurs requêtes.
	 * Voir la <a href="https://klee-contrib.github.io/topmodel/#/generator/jpa?id=constructeurs-par-alias">documentation</a>

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.securite.TypeProfil#code code}.
	 */
	public TypeProfil.Values getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.securite.TypeProfil#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.securite.TypeProfil#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeProfil.Values code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.securite.TypeProfil#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Equal function comparing Code.
	 */
	public boolean equals(Object o) {
		if(o instanceof TypeProfil typeProfil) {
			if(this == typeProfil)
				return true;

			if(typeProfil == null || this.getCode() == null)
				return false;

			return this.getCode().equals(typeProfil.getCode());
		}
		return false;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.dao.entities.securite.TypeProfil TypeProfil}.
	 */
	public enum Fields implements IFieldEnum<TypeProfil> {
        CODE, //
        LIBELLE
	}

	public enum Values {
		ADM("Administrateur"), //
		GES("Gestionnaire"); 

		/**
		 * Libellé du type d'utilisateur.
		 */
		private final String libelle;

		/**
		 * All arg constructor.
		 */
		private Values(String libelle) {
			this.libelle = libelle;
		}

		/**
		 * Libellé du type d'utilisateur.
		 */
		public String getLibelle(){
			return this.libelle;
		}
	}
}

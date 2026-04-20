////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.entities.refs;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;

import hello world.enums.refs.TypeUtilisateurCode;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Type d'utilisateur.
 */
@Entity
@Table(name = "TYPE_UTILISATEUR")
@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TypeUtilisateur {

	/**
	 * Code du type d'utilisateur.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "CODE", nullable = false, length = 3, columnDefinition = "varchar")
	private TypeUtilisateurCode code;

	/**
	 * Libellé du type d'utilisateur.
	 */
	@Column(name = "LIBELLE", nullable = false, length = 15, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public TypeUtilisateurCode getCode() {
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
	 * Set the value of {@link #code code}.
	 * @param code value to set.
	 */
	public void setCode(TypeUtilisateurCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link #libelle libelle}.
	 * @param libelle value to set.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}
}

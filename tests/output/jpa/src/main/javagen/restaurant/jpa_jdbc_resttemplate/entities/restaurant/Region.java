////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Région.
 */
@Table(name = "region")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Region {

	/**
	 * Code de la région.
	 */
	@Id
	@Column("reg_code")
	private String code;

	/**
	 * Libellé de la région.
	 */
	@NotNull
	@Column("reg_libelle")
	private String libelle;

	/**
	 * Nom du responsable de la région.
	 */
	@Column("reg_nom_responsable")
	private String nomResponsable;

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public String getCode() {
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
	 * Getter for nomResponsable.
	 *
	 * @return value of {@link #nomResponsable nomResponsable}.
	 */
	public String getNomResponsable() {
		return this.nomResponsable;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Transient;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.DepartementCode;

/**
 * Département.
 */
@Table(name = "departement")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Departement {

	@Transient
	private static final Departement HAUTS_DE_SEINE = new Departement(DepartementCode.HautsDeSeine);

	@Transient
	private static final Departement PARIS = new Departement(DepartementCode.Paris);

	@Transient
	private static final Departement SEINE_ET_MARNE = new Departement(DepartementCode.SeineEtMarne);

	@Transient
	private static final Departement SEINE_SAINT_DENIS = new Departement(DepartementCode.SeineSaintDenis);

	/**
	 * Code du département.
	 */
	@Id
	@Column("dep_code")
	private String code;

	/**
	 * Libellé du département.
	 */
	@NotNull
	@Column("dep_libelle")
	private String libelle;

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public Departement(String code) {
		this.code = code;
		switch(code) {
			case DepartementCode.HautsDeSeine:
				this.libelle = "restaurant.departement.values.HautsDeSeine";
				break;
			case DepartementCode.Paris:
				this.libelle = "restaurant.departement.values.Paris";
				break;
			case DepartementCode.SeineEtMarne:
				this.libelle = "restaurant.departement.values.SeineEtMarne";
				break;
			case DepartementCode.SeineSaintDenis:
				this.libelle = "restaurant.departement.values.SeineSaintDenis";
				break;
		}
	}

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
}

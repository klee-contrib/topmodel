////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Transient;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.DepartementCode;
import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.RegionCode;

/**
 * Département.
 */
@Table(name = "departement")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Departement {

	@Transient
	public static final Departement HAUTS_DE_SEINE = new Departement(DepartementCode.HautsDeSeine);

	@Transient
	public static final Departement PARIS = new Departement(DepartementCode.Paris);

	@Transient
	public static final Departement SEINE_ET_MARNE = new Departement(DepartementCode.SeineEtMarne);

	@Transient
	public static final Departement SEINE_SAINT_DENIS = new Departement(DepartementCode.SeineSaintDenis);

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
	 * Région associée.
	 */
	@NotNull
	@Column("reg_code")
	@Enumerated(EnumType.STRING)
	private String regionCode;

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public Departement(String code) {
		this.code = code;
		switch(code) {
			case HautsDeSeine:
				this.libelle = "restaurant.departement.values.HautsDeSeine";
				this.regionCode = RegionCode.Idf;
				break;
			case Paris:
				this.libelle = "restaurant.departement.values.Paris";
				this.regionCode = RegionCode.Idf;
				break;
			case SeineEtMarne:
				this.libelle = "restaurant.departement.values.SeineEtMarne";
				this.regionCode = RegionCode.Idf;
				break;
			case SeineSaintDenis:
				this.libelle = "restaurant.departement.values.SeineSaintDenis";
				this.regionCode = RegionCode.Idf;
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

	/**
	 * Getter for regionCode.
	 *
	 * @return value of {@link #regionCode regionCode}.
	 */
	public String getRegionCode() {
		return this.regionCode;
	}
}

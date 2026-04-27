////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
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
	public static final Departement HAUTS_DE_SEINE = new Departement(DepartementCode.HautsDeSeine, "restaurant.departement.values.HautsDeSeine", RegionCode.Idf);

	@Transient
	public static final Departement PARIS = new Departement(DepartementCode.Paris, "restaurant.departement.values.Paris", RegionCode.Idf);

	@Transient
	public static final Departement SEINE_ET_MARNE = new Departement(DepartementCode.SeineEtMarne, "restaurant.departement.values.SeineEtMarne", RegionCode.Idf);

	@Transient
	public static final Departement SEINE_SAINT_DENIS = new Departement(DepartementCode.SeineSaintDenis, "restaurant.departement.values.SeineSaintDenis", RegionCode.Idf);

	/**
	 * Liste de toutes les valeurs de l'énumération Departement.
	 */
	public static final List<Departement> VALUES = List.of(HAUTS_DE_SEINE, PARIS, SEINE_ET_MARNE, SEINE_SAINT_DENIS);

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
	private String regionCode;

	/**
	 * All args constructor for 'Departement'.
	 * @param code Code du département.
	 * @param libelle Libellé du département.
	 * @param regionCode Région associée.
	 */
	private Departement(String code, String libelle, String regionCode) {
		this.code = code;
		this.libelle = libelle;
		this.regionCode = regionCode;
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

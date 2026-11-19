////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.DepartementCode;
import restaurant.jpa_jdbc_resttemplate.enums.restaurant.RegionCode;

/**
 * Département.
 */
@Table(name = "departement")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Departement {

	public static final Departement HAUTS_DE_SEINE = new Departement(DepartementCode.HautsDeSeine, "restaurant.departement.values.HautsDeSeine", RegionCode.Idf);

	public static final Departement PARIS = new Departement(DepartementCode.Paris, "restaurant.departement.values.Paris", RegionCode.Idf);

	public static final Departement SEINE_ET_MARNE = new Departement(DepartementCode.SeineEtMarne, "restaurant.departement.values.SeineEtMarne", RegionCode.Idf);

	public static final Departement SEINE_SAINT_DENIS = new Departement(DepartementCode.SeineSaintDenis, "restaurant.departement.values.SeineSaintDenis", RegionCode.Idf);

	/**
	 * Liste de toutes les valeurs de l'énumération 'Departement'.
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
	 * Constructeur tout arguments pour 'Departement'.
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
	 * Accesseur de 'code'.
	 *
	 * @return Valeur de {@link #code code}.
	 */
	public String getCode() {
		return this.code;
	}

	/**
	 * Accesseur de 'libelle'.
	 *
	 * @return Valeur de {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Accesseur de 'regionCode'.
	 *
	 * @return Valeur de {@link #regionCode regionCode}.
	 */
	public String getRegionCode() {
		return this.regionCode;
	}
}

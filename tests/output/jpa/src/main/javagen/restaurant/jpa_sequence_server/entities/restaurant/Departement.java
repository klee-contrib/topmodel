////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

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

import restaurant.jpa_sequence_server.enums.restaurant.DepartementCode;
import restaurant.jpa_sequence_server.enums.restaurant.RegionCode;

/**
 * Département.
 */
@Entity
@Immutable
@Table(name = "DEPARTEMENT")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
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
	@Column(name = "DEP_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private String code;

	/**
	 * Libellé du département.
	 */
	@Column(name = "DEP_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Région associée.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "REG_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private RegionCode regionCode;

	/**
	 * No arg constructor.
	 */
	public Departement() {
		// No arg constructor
	}

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public Departement(String code) {
		this.code = code;
		switch(code) {
			case DepartementCode.HautsDeSeine:
				this.libelle = "restaurant.departement.values.HautsDeSeine";
				this.regionCode = RegionCode.IDF;
				break;
			case DepartementCode.Paris:
				this.libelle = "restaurant.departement.values.Paris";
				this.regionCode = RegionCode.IDF;
				break;
			case DepartementCode.SeineEtMarne:
				this.libelle = "restaurant.departement.values.SeineEtMarne";
				this.regionCode = RegionCode.IDF;
				break;
			case DepartementCode.SeineSaintDenis:
				this.libelle = "restaurant.departement.values.SeineSaintDenis";
				this.regionCode = RegionCode.IDF;
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
	public RegionCode getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.Departement Departement}.
	 */
	public enum Fields {
		CODE(String.class),
		LIBELLE(String.class),
		REGION_CODE(RegionCode.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.util.List;

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

import restaurant.jpa_server.enums.restaurant.DepartementCode;
import restaurant.jpa_server.enums.restaurant.RegionCode;

/**
 * Département.
 */
@Entity
@Immutable
@Table(name = "departement")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Departement {

	@Transient
	public static final Departement HAUTS_DE_SEINE = new Departement(DepartementCode.HautsDeSeine, "restaurant.departement.values.HautsDeSeine", RegionCode.IDF);

	@Transient
	public static final Departement PARIS = new Departement(DepartementCode.Paris, "restaurant.departement.values.Paris", RegionCode.IDF);

	@Transient
	public static final Departement SEINE_ET_MARNE = new Departement(DepartementCode.SeineEtMarne, "restaurant.departement.values.SeineEtMarne", RegionCode.IDF);

	@Transient
	public static final Departement SEINE_SAINT_DENIS = new Departement(DepartementCode.SeineSaintDenis, "restaurant.departement.values.SeineSaintDenis", RegionCode.IDF);

	/**
	 * Liste de toutes les valeurs de l'énumération Departement.
	 */
	public static final List<Departement> VALUES = List.of(HAUTS_DE_SEINE, PARIS, SEINE_ET_MARNE, SEINE_SAINT_DENIS);

	/**
	 * Code du département.
	 */
	@Id
	@Column(name = "dep_code", nullable = false, length = 10, columnDefinition = "varchar")
	private String code;

	/**
	 * Libellé du département.
	 */
	@Column(name = "dep_libelle", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Région associée.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "reg_code", nullable = false, length = 10, columnDefinition = "varchar")
	private RegionCode regionCode;

	/**
	 * No arg constructor.
	 */
	public Departement() {
		// No arg constructor
	}

	/**
	 * All args constructor for 'Departement'.
	 * @param code Code du département.
	 * @param libelle Libellé du département.
	 * @param regionCode Région associée.
	 */
	private Departement(String code, String libelle, RegionCode regionCode) {
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
	public RegionCode getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Retourne la valeur de l'énumération pour la clé spécifiée.
	 * @param code La clé de l'énumération pour laquelle obtenir la valeur.
	 *
	 * @return La valeur de l'énumération correspondant à la clé 'Code'.
	 */
	public static Departement getValue(String code) {
		return switch (code) {
			case DepartementCode.HautsDeSeine -> HAUTS_DE_SEINE;
			case DepartementCode.Paris -> PARIS;
			case DepartementCode.SeineEtMarne -> SEINE_ET_MARNE;
			case DepartementCode.SeineSaintDenis -> SEINE_SAINT_DENIS;
			default -> throw new IllegalArgumentException("Clé d'énumération inconnue : " + code);
		};
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Departement Departement}.
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

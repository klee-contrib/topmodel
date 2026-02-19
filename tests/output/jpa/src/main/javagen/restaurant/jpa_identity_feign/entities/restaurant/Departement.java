////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

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
	public static final Departement HAUTS_DE_SEINE = new Departement("92");

	@Transient
	public static final Departement PARIS = new Departement("75");

	@Transient
	public static final Departement SEINE_ET_MARNE = new Departement("94");

	@Transient
	public static final Departement SEINE_SAINT_DENIS = new Departement("93");

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
			case "92":
				this.libelle = "restaurant.departement.values.HautsDeSeine";
				break;
			case "75":
				this.libelle = "restaurant.departement.values.Paris";
				break;
			case "94":
				this.libelle = "restaurant.departement.values.SeineEtMarne";
				break;
			case "93":
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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.Departement Departement}.
	 */
	public enum Fields {
		CODE(String.class),
		LIBELLE(String.class);

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

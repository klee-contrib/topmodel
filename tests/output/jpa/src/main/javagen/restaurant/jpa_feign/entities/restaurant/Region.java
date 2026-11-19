////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import restaurant.jpa_feign.enums.restaurant.RegionCode;

/**
 * Région.
 */
@Entity
@Table(name = "region")
@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Region {

	/**
	 * Code de la région.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "reg_code", nullable = false, length = 10, columnDefinition = "varchar")
	private RegionCode code;

	/**
	 * Libellé de la région.
	 */
	@Column(name = "reg_libelle", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Nom du responsable de la région.
	 */
	@Column(name = "reg_nom_responsable", length = 100, columnDefinition = "varchar")
	private String nomResponsable;

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public RegionCode getCode() {
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

	/**
	 * Set the value of {@link #code code}.
	 * @param code value to set.
	 */
	public void setCode(RegionCode code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link #libelle libelle}.
	 * @param libelle value to set.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link #nomResponsable nomResponsable}.
	 * @param nomResponsable value to set.
	 */
	public void setNomResponsable(String nomResponsable) {
		this.nomResponsable = nomResponsable;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Region Region}.
	 */
	public enum Fields {
		CODE(RegionCode.class),
		LIBELLE(String.class),
		NOM_RESPONSABLE(String.class);

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

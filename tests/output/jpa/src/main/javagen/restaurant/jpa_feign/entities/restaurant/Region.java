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
	 * Accesseur de 'code'.
	 *
	 * @return Valeur de {@link #code code}.
	 */
	public RegionCode getCode() {
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
	 * Accesseur de 'nomResponsable'.
	 *
	 * @return Valeur de {@link #nomResponsable nomResponsable}.
	 */
	public String getNomResponsable() {
		return this.nomResponsable;
	}

	/**
	 * Définit la valeur de {@link #code code}.
	 * @param code Valeur à définir.
	 */
	public void setCode(RegionCode code) {
		this.code = code;
	}

	/**
	 * Définit la valeur de {@link #libelle libelle}.
	 * @param libelle Valeur à définir.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Définit la valeur de {@link #nomResponsable nomResponsable}.
	 * @param nomResponsable Valeur à définir.
	 */
	public void setNomResponsable(String nomResponsable) {
		this.nomResponsable = nomResponsable;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Region Region}.
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
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

import java.util.List;
import java.util.Objects;

import jakarta.annotation.Generated;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

import restaurant.jpa_identity_feign.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Entity
@Table(name = "CATEGORIE_PLAT_REGION")
@IdClass(CategoriePlatRegion.CategoriePlatRegionId.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion {

	@Transient
	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.IDF, CategoriePlat.DESSERT);

	@Transient
	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.IDF, CategoriePlat.ENTREE);

	/**
	 * Liste de toutes les valeurs de l'énumération CategoriePlatRegion.
	 */
	public static final List<CategoriePlatRegion> VALUES = List.of(IDF_DESSERT, IDF_ENTREE);

	/**
	 * Région.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	private RegionCode regionCode;

	/**
	 * Catégorie de plat.
	 */
	@Id
	private CategoriePlat categoriePlat;

	/**
	 * No arg constructor.
	 */
	public CategoriePlatRegion() {
		// No arg constructor
	}

	/**
	 * All args constructor for 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlat Catégorie de plat.
	 */
	private CategoriePlatRegion(RegionCode regionCode, CategoriePlat categoriePlat) {
		this.regionCode = regionCode;
		this.categoriePlat = categoriePlat;
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
	 * Getter for categoriePlat.
	 *
	 * @return value of {@link #categoriePlat categoriePlat}.
	 */
	public CategoriePlat getCategoriePlat() {
		return this.categoriePlat;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.CategoriePlatRegion CategoriePlatRegion}.
	 */
	public enum Fields {
		REGION_CODE(RegionCode.class),
		CATEGORIE_PLAT(CategoriePlat.class);

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

	public static class CategoriePlatRegionId {

		@JoinColumn(name = "REG_CODE", referencedColumnName = "REG_CODE")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Region.class)
		private RegionCode regionCode;

		@JoinColumn(name = "CAT_CODE", referencedColumnName = "CAT_CODE")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CategoriePlat.class)
		private CategoriePlat categoriePlat;

		/**
		 * Getter for regionCode.
		 *
		 * @return value of {@link #regionCode regionCode}.
		 */
		public RegionCode getRegionCode() {
			return this.regionCode;
		}

		/**
		 * Set the value of {@link #regionCode regionCode}.
		 * @param regionCode value to set.
		 */
		public void setRegionCode(RegionCode regionCode) {
			this.regionCode = regionCode;
		}

		/**
		 * Getter for categoriePlat.
		 *
		 * @return value of {@link #categoriePlat categoriePlat}.
		 */
		public CategoriePlat getCategoriePlat() {
			return this.categoriePlat;
		}

		/**
		 * Set the value of {@link #categoriePlat categoriePlat}.
		 * @param categoriePlat value to set.
		 */
		public void setCategoriePlat(CategoriePlat categoriePlat) {
			this.categoriePlat = categoriePlat;
		}

		public boolean equals(Object o) {
			if (o == this) {
				return true;
			}

			if (o == null) {
				return false;
			}

			if (this.getClass() != o.getClass()) {
				return false;
			}

			CategoriePlatRegionId oId = (CategoriePlatRegionId) o;

			if (this.regionCode == null || oId.regionCode == null || this.categoriePlat == null || oId.categoriePlat == null) {
				return false;
			}

			return Objects.equals(this.regionCode, oId.regionCode)
			 && Objects.equals(this.categoriePlat.getCode(), oId.categoriePlat.getCode());
		}

		@Override
		public int hashCode() {
			return Objects.hash(regionCode == null ? null : regionCode, categoriePlat == null ? null : categoriePlat.getCode());
		}
	}
}

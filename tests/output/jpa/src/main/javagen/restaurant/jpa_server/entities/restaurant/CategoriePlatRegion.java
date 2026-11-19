////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

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

import restaurant.jpa_server.enums.restaurant.RegionCode;

/**
 * Catégories de plats disponibles par région.
 */
@Entity
@Table(name = "categorie_plat_region")
@IdClass(CategoriePlatRegion.CategoriePlatRegionId.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion {

	public static final CategoriePlatRegion IDF_DESSERT = new CategoriePlatRegion(RegionCode.IDF, CategoriePlat.DESSERT);

	public static final CategoriePlatRegion IDF_ENTREE = new CategoriePlatRegion(RegionCode.IDF, CategoriePlat.ENTREE);

	/**
	 * Liste de toutes les valeurs de l'énumération 'CategoriePlatRegion'.
	 */
	public static final List<CategoriePlatRegion> VALUES = List.of(IDF_ENTREE, IDF_DESSERT);

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
	 * Constructeur sans argument.
	 */
	public CategoriePlatRegion() {
		// Constructeur sans argument
	}

	/**
	 * Constructeur tout arguments pour 'CategoriePlatRegion'.
	 * @param regionCode Région.
	 * @param categoriePlat Catégorie de plat.
	 */
	private CategoriePlatRegion(RegionCode regionCode, CategoriePlat categoriePlat) {
		this.regionCode = regionCode;
		this.categoriePlat = categoriePlat;
	}

	/**
	 * Accesseur de 'regionCode'.
	 *
	 * @return Valeur de {@link #regionCode regionCode}.
	 */
	public RegionCode getRegionCode() {
		return this.regionCode;
	}

	/**
	 * Accesseur de 'categoriePlat'.
	 *
	 * @return Valeur de {@link #categoriePlat categoriePlat}.
	 */
	public CategoriePlat getCategoriePlat() {
		return this.categoriePlat;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.CategoriePlatRegion CategoriePlatRegion}.
	 */
	public enum Fields {
		REGION_CODE(RegionCode.class),
		CATEGORIE_PLAT(CategoriePlat.class);

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

	public static class CategoriePlatRegionId {

		@JoinColumn(name = "reg_code", referencedColumnName = "reg_code")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Region.class)
		private RegionCode regionCode;

		@JoinColumn(name = "cat_code", referencedColumnName = "cat_code")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CategoriePlat.class)
		private CategoriePlat categoriePlat;

		/**
		 * Accesseur de 'regionCode'.
		 *
		 * @return Valeur de {@link #regionCode regionCode}.
		 */
		public RegionCode getRegionCode() {
			return this.regionCode;
		}

		/**
		 * Définit la valeur de {@link #regionCode regionCode}.
		 * @param regionCode Valeur à définir.
		 */
		public void setRegionCode(RegionCode regionCode) {
			this.regionCode = regionCode;
		}

		/**
		 * Accesseur de 'categoriePlat'.
		 *
		 * @return Valeur de {@link #categoriePlat categoriePlat}.
		 */
		public CategoriePlat getCategoriePlat() {
			return this.categoriePlat;
		}

		/**
		 * Définit la valeur de {@link #categoriePlat categoriePlat}.
		 * @param categoriePlat Valeur à définir.
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
			return Objects.hash(regionCode, categoriePlat == null ? null : categoriePlat.getCode());
		}
	}
}

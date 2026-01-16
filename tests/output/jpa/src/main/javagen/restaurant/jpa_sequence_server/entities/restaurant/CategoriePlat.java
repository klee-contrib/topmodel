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

import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatCode;

/**
 * Catégorie de plat.
 */
@Entity
@Immutable
@Table(name = "CATEGORIE_PLAT")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat {

	@Transient
	public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.BOISSON);

	@Transient
	public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.DESSERT);

	@Transient
	public static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.ENTREE);

	@Transient
	public static final CategoriePlat PLAT = new CategoriePlat(CategoriePlatCode.PLAT);

	/**
	 * Code de la catégorie.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "CAT_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private CategoriePlatCode code;

	/**
	 * Libellé de la catégorie.
	 */
	@Column(name = "CAT_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public CategoriePlat() {
		// No arg constructor
	}

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public CategoriePlat(CategoriePlatCode code) {
		this.code = code;
		switch(code) {
			case BOISSON:
				this.libelle = "restaurant.categoriePlat.values.Boisson";
				break;
			case DESSERT:
				this.libelle = "restaurant.categoriePlat.values.Dessert";
				break;
			case ENTREE:
				this.libelle = "restaurant.categoriePlat.values.Entree";
				break;
			case PLAT:
				this.libelle = "restaurant.categoriePlat.values.Plat";
				break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public CategoriePlatCode getCode() {
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
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.CategoriePlat CategoriePlat}.
	 */
	public enum Fields {
		CODE(CategoriePlatCode.class),
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

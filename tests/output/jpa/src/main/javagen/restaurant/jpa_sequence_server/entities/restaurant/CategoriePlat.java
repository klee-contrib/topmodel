////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import java.math.BigDecimal;

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
import jakarta.persistence.UniqueConstraint;

import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatOrdre;

/**
 * Catégorie de plat.
 */
@Entity
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "CATEGORIE_PLAT",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"CAT_ORDRE"})
	}
)
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
	 * Ordre d'affichage dans le menu.
	 */
	@Column(name = "CAT_ORDRE", nullable = false, columnDefinition = "int")
	private Integer ordre;

	/**
	 * Prix moyen de la catégorie, à titre indicatif.
	 */
	@Column(name = "CAT_PRIX_MOYEN", scale = 2, columnDefinition = "decimal")
	private BigDecimal prixMoyen;

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
				this.ordre = CategoriePlatOrdre.Boisson;
				this.prixMoyen = new BigDecimal(2);
				break;
			case DESSERT:
				this.libelle = "restaurant.categoriePlat.values.Dessert";
				this.ordre = CategoriePlatOrdre.Dessert;
				this.prixMoyen = null;
				break;
			case ENTREE:
				this.libelle = "restaurant.categoriePlat.values.Entree";
				this.ordre = CategoriePlatOrdre.Entree;
				this.prixMoyen = null;
				break;
			case PLAT:
				this.libelle = "restaurant.categoriePlat.values.Plat";
				this.ordre = CategoriePlatOrdre.Plat;
				this.prixMoyen = new BigDecimal(10);
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
	 * Getter for ordre.
	 *
	 * @return value of {@link #ordre ordre}.
	 */
	public Integer getOrdre() {
		return this.ordre;
	}

	/**
	 * Getter for prixMoyen.
	 *
	 * @return value of {@link #prixMoyen prixMoyen}.
	 */
	public BigDecimal getPrixMoyen() {
		return this.prixMoyen;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.CategoriePlat CategoriePlat}.
	 */
	public enum Fields {
		CODE(CategoriePlatCode.class),
		LIBELLE(String.class),
		ORDRE(Integer.class),
		PRIX_MOYEN(BigDecimal.class);

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

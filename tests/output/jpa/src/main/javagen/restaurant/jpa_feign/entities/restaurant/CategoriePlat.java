////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.math.BigDecimal;
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
import jakarta.persistence.UniqueConstraint;

import restaurant.jpa_feign.enums.restaurant.CategoriePlatCode;

/**
 * Catégorie de plat.
 */
@Entity
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "categorie_plat",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"cat_ordre"})
	}
)
public class CategoriePlat {

	public static final CategoriePlat AUTRE = new CategoriePlat(CategoriePlatCode.AUTRE, "Autre", 5, null);

	public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.BOISSON, "Boisson", 1, new BigDecimal(2));

	public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.DESSERT, "Dessert", 4, null);

	public static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.ENTREE, "Entrée", 2, null);

	public static final CategoriePlat PRINCIPAL = new CategoriePlat(CategoriePlatCode.PRINCIPAL, "Plat principal", 3, new BigDecimal(10));

	/**
	 * Liste de toutes les valeurs de l'énumération 'CategoriePlat'.
	 */
	public static final List<CategoriePlat> VALUES = List.of(BOISSON, ENTREE, PRINCIPAL, DESSERT, AUTRE);

	/**
	 * Code de la catégorie.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "cat_code", nullable = false, length = 10, columnDefinition = "varchar")
	private CategoriePlatCode code;

	/**
	 * Libellé de la catégorie.
	 */
	@Column(name = "cat_libelle", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Ordre d'affichage dans le menu.
	 */
	@Column(name = "cat_ordre", nullable = false, columnDefinition = "int")
	private Integer ordre;

	/**
	 * Prix moyen de la catégorie, à titre indicatif.
	 */
	@Column(name = "cat_prix_moyen", scale = 2, columnDefinition = "decimal")
	private BigDecimal prixMoyen;

	/**
	 * Constructeur sans argument.
	 */
	public CategoriePlat() {
		// Constructeur sans argument
	}

	/**
	 * Constructeur tout arguments pour 'CategoriePlat'.
	 * @param code Code de la catégorie.
	 * @param libelle Libellé de la catégorie.
	 * @param ordre Ordre d'affichage dans le menu.
	 * @param prixMoyen Prix moyen de la catégorie, à titre indicatif.
	 */
	private CategoriePlat(CategoriePlatCode code, String libelle, Integer ordre, BigDecimal prixMoyen) {
		this.code = code;
		this.libelle = libelle;
		this.ordre = ordre;
		this.prixMoyen = prixMoyen;
	}

	/**
	 * Accesseur de 'code'.
	 *
	 * @return Valeur de {@link #code code}.
	 */
	public CategoriePlatCode getCode() {
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
	 * Accesseur de 'ordre'.
	 *
	 * @return Valeur de {@link #ordre ordre}.
	 */
	public Integer getOrdre() {
		return this.ordre;
	}

	/**
	 * Accesseur de 'prixMoyen'.
	 *
	 * @return Valeur de {@link #prixMoyen prixMoyen}.
	 */
	public BigDecimal getPrixMoyen() {
		return this.prixMoyen;
	}

	/**
	 * Retourne la valeur de l'énumération pour la clé spécifiée.
	 * @param code La clé de l'énumération pour laquelle obtenir la valeur.
	 *
	 * @return La valeur de l'énumération correspondant à la clé 'Code'.
	 */
	public static CategoriePlat getValue(CategoriePlatCode code) {
		return switch (code) {
			case AUTRE -> AUTRE;
			case BOISSON -> BOISSON;
			case DESSERT -> DESSERT;
			case ENTREE -> ENTREE;
			case PRINCIPAL -> PRINCIPAL;
		};
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.CategoriePlat CategoriePlat}.
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
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.entities.restaurant;

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

import restaurant.jpa_identity_feign.enums.restaurant.CategoriePlatCode;

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
	public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.BOISSON, "Boisson", 1, new BigDecimal(2));

	@Transient
	public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.DESSERT, "Dessert", 4, null);

	@Transient
	public static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.ENTREE, "Entrée", 2, null);

	@Transient
	public static final CategoriePlat PLAT = new CategoriePlat(CategoriePlatCode.PLAT, "Plat principal", 3, new BigDecimal(10));

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
	 * All args constructor for 'CategoriePlat'.
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
	 * Retourne la valeur de l'énumération pour la clé spécifiée.
	 * @param code La clé de l'énumération pour laquelle obtenir la valeur.
	 *
	 * @return La valeur de l'énumération correspondant à la clé 'Code'.
	 */
	public static CategoriePlat getValue(CategoriePlatCode code) {
		return switch (code) {
			case BOISSON -> BOISSON;
			case DESSERT -> DESSERT;
			case ENTREE -> ENTREE;
			case PLAT -> PLAT;
		};
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.entities.restaurant.CategoriePlat CategoriePlat}.
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

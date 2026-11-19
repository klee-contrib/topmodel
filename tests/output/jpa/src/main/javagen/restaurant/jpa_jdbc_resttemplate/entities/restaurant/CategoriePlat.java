////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.math.BigDecimal;
import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_resttemplate.enums.restaurant.CategoriePlatOrdre;

/**
 * Catégorie de plat.
 */
@Table(name = "categorie_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat {

	public static final CategoriePlat AUTRE = new CategoriePlat(CategoriePlatCode.Autre, "restaurant.categoriePlat.values.Autre", CategoriePlatOrdre.Autre, null);

	public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.Boisson, "restaurant.categoriePlat.values.Boisson", CategoriePlatOrdre.Boisson, new BigDecimal(2));

	public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.Dessert, "restaurant.categoriePlat.values.Dessert", CategoriePlatOrdre.Dessert, null);

	public static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.Entree, "restaurant.categoriePlat.values.Entree", CategoriePlatOrdre.Entree, null);

	public static final CategoriePlat PRINCIPAL = new CategoriePlat(CategoriePlatCode.Principal, "restaurant.categoriePlat.values.Principal", CategoriePlatOrdre.Principal, new BigDecimal(10));

	/**
	 * Liste de toutes les valeurs de l'énumération 'CategoriePlat'.
	 */
	public static final List<CategoriePlat> VALUES = List.of(BOISSON, ENTREE, PRINCIPAL, DESSERT, AUTRE);

	/**
	 * Code de la catégorie.
	 */
	@Id
	@Column("cat_code")
	private String code;

	/**
	 * Libellé de la catégorie.
	 */
	@NotNull
	@Column("cat_libelle")
	private String libelle;

	/**
	 * Ordre d'affichage dans le menu.
	 */
	@NotNull
	@Column("cat_ordre")
	private Integer ordre;

	/**
	 * Prix moyen de la catégorie, à titre indicatif.
	 */
	@Column("cat_prix_moyen")
	private BigDecimal prixMoyen;

	/**
	 * Constructeur tout arguments pour 'CategoriePlat'.
	 * @param code Code de la catégorie.
	 * @param libelle Libellé de la catégorie.
	 * @param ordre Ordre d'affichage dans le menu.
	 * @param prixMoyen Prix moyen de la catégorie, à titre indicatif.
	 */
	private CategoriePlat(String code, String libelle, Integer ordre, BigDecimal prixMoyen) {
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
	public String getCode() {
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
}

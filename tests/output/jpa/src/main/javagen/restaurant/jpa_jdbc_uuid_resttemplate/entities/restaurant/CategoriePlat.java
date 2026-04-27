////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import java.math.BigDecimal;
import java.util.List;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Transient;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_jdbc_uuid_resttemplate.enums.restaurant.CategoriePlatOrdre;

/**
 * Catégorie de plat.
 */
@Table(name = "categorie_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat {

	@Transient
	public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.Boisson, "restaurant.categoriePlat.values.Boisson", CategoriePlatOrdre.Boisson, new BigDecimal(2));

	@Transient
	public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.Dessert, "restaurant.categoriePlat.values.Dessert", CategoriePlatOrdre.Dessert, null);

	@Transient
	public static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.Entree, "restaurant.categoriePlat.values.Entree", CategoriePlatOrdre.Entree, null);

	@Transient
	public static final CategoriePlat PLAT = new CategoriePlat(CategoriePlatCode.Plat, "restaurant.categoriePlat.values.Plat", CategoriePlatOrdre.Plat, new BigDecimal(10));

	/**
	 * Liste de toutes les valeurs de l'énumération CategoriePlat.
	 */
	public static final List<CategoriePlat> VALUES = List.of(BOISSON, DESSERT, ENTREE, PLAT);

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
	 * All args constructor for 'CategoriePlat'.
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
}

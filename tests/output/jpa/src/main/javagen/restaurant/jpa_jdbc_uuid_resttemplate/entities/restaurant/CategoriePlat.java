////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
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
	private static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.Boisson);

	@Transient
	private static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.Dessert);

	@Transient
	private static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.Entree);

	@Transient
	private static final CategoriePlat PLAT = new CategoriePlat(CategoriePlatCode.Plat);

	/**
	 * Code de la catégorie.
	 */
	@Id
	@Column("cat_code")
	@Enumerated(EnumType.STRING)
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
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public CategoriePlat(String code) {
		this.code = code;
		switch(code) {
			case CategoriePlatCode.Boisson:
				this.libelle = "restaurant.categoriePlat.values.Boisson";
				this.ordre = CategoriePlatOrdre.Boisson;
				break;
			case CategoriePlatCode.Dessert:
				this.libelle = "restaurant.categoriePlat.values.Dessert";
				this.ordre = CategoriePlatOrdre.Dessert;
				break;
			case CategoriePlatCode.Entree:
				this.libelle = "restaurant.categoriePlat.values.Entree";
				this.ordre = CategoriePlatOrdre.Entree;
				break;
			case CategoriePlatCode.Plat:
				this.libelle = "restaurant.categoriePlat.values.Plat";
				this.ordre = CategoriePlatOrdre.Plat;
				break;
		}
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
}

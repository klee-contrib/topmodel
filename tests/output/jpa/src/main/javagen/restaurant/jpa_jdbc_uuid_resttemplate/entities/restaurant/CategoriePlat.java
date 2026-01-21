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

/**
 * Catégorie de plat.
 */
@Table(name = "categorie_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat {

	@Transient
	private static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.BOISSON);

	@Transient
	private static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.DESSERT);

	@Transient
	private static final CategoriePlat ENTREE = new CategoriePlat(CategoriePlatCode.ENTREE);

	@Transient
	private static final CategoriePlat PLAT = new CategoriePlat(CategoriePlatCode.PLAT);

	/**
	 * Code de la catégorie.
	 */
	@Id
	@Column("cat_code")
	@Enumerated(EnumType.STRING)
	private CategoriePlatCode code;

	/**
	 * Libellé de la catégorie.
	 */
	@NotNull
	@Column("cat_libelle")
	private String libelle;

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
}

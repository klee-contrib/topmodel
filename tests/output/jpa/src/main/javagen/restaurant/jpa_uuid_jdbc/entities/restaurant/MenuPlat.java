////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Plat dans un menu.
 */
@Table(name = "menu_plat")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuPlat {

	/**
	 * Identifiant de la relation.
	 */
	@Id
	@Column("mpl_id")
	private Integer id;

	/**
	 * Ordre d'affichage du plat dans le menu.
	 */
	@NotNull
	@Column("mpl_ordre")
	private Integer ordre;

	/**
	 * Menu contenant ce plat.
	 */
	@Column("men_id_menu")
	private Integer menuIdMenu;

	/**
	 * Plat du menu.
	 */
	@Column("pla_id_plat")
	private Integer platIdPlat;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
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
	 * Getter for menuIdMenu.
	 *
	 * @return value of {@link #menuIdMenu menuIdMenu}.
	 */
	public Integer getMenuIdMenu() {
		return this.menuIdMenu;
	}

	/**
	 * Getter for platIdPlat.
	 *
	 * @return value of {@link #platIdPlat platIdPlat}.
	 */
	public Integer getPlatIdPlat() {
		return this.platIdPlat;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #ordre ordre}.
	 * @param ordre value to set.
	 */
	public void setOrdre(Integer ordre) {
		this.ordre = ordre;
	}

	/**
	 * Set the value of {@link #menuIdMenu menuIdMenu}.
	 * @param menuIdMenu value to set.
	 */
	public void setMenuIdMenu(Integer menuIdMenu) {
		this.menuIdMenu = menuIdMenu;
	}

	/**
	 * Set the value of {@link #platIdPlat platIdPlat}.
	 * @param platIdPlat value to set.
	 */
	public void setPlatIdPlat(Integer platIdPlat) {
		this.platIdPlat = platIdPlat;
	}
}

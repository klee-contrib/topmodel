////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

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
	 * Menu contenant ce plat.
	 */
	@Id
	private Integer menuId;

	/**
	 * Plat du menu.
	 */
	@Id
	private Integer platId;

	/**
	 * Ordre d'affichage du plat dans le menu.
	 */
	@NotNull
	@Column("mpl_ordre")
	private Integer ordre;

	/**
	 * Getter for menuId.
	 *
	 * @return value of {@link #menuId menuId}.
	 */
	public Integer getMenuId() {
		return this.menuId;
	}

	/**
	 * Getter for platId.
	 *
	 * @return value of {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
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
	 * Set the value of {@link #menuId menuId}.
	 * @param menuId value to set.
	 */
	public void setMenuId(Integer menuId) {
		this.menuId = menuId;
	}

	/**
	 * Set the value of {@link #platId platId}.
	 * @param platId value to set.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Set the value of {@link #ordre ordre}.
	 * @param ordre value to set.
	 */
	public void setOrdre(Integer ordre) {
		this.ordre = ordre;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Plat dans un menu.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "MENU_PLAT", uniqueConstraints = {@UniqueConstraint(columnNames = {"MEN_ID_MENU", "MPL_ORDRE"}), @UniqueConstraint(columnNames = {"MEN_ID_MENU", "PLA_ID_PLAT"})})
public class MenuPlat {

	/**
	 * Identifiant de la relation.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "MPL_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Ordre d'affichage du plat dans le menu.
	 */
	@Column(name = "MPL_ORDRE", nullable = false, columnDefinition = "int")
	private Integer ordre;

	/**
	 * Menu contenant ce plat.
	 */
	@JoinColumn(name = "MEN_ID_MENU", referencedColumnName = "MEN_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Menu.class)
	private Menu menuMenu;

	/**
	 * Plat du menu.
	 */
	@JoinColumn(name = "PLA_ID_PLAT", referencedColumnName = "PLA_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
	private Plat platPlat;

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
	 * Getter for menuMenu.
	 *
	 * @return value of {@link #menuMenu menuMenu}.
	 */
	public Menu getMenuMenu() {
		return this.menuMenu;
	}

	/**
	 * Getter for platPlat.
	 *
	 * @return value of {@link #platPlat platPlat}.
	 */
	public Plat getPlatPlat() {
		return this.platPlat;
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
	 * Set the value of {@link #menuMenu menuMenu}.
	 * @param menuMenu value to set.
	 */
	public void setMenuMenu(Menu menuMenu) {
		this.menuMenu = menuMenu;
	}

	/**
	 * Set the value of {@link #platPlat platPlat}.
	 * @param platPlat value to set.
	 */
	public void setPlatPlat(Plat platPlat) {
		this.platPlat = platPlat;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.entities.restaurant.MenuPlat MenuPlat}.
	 */
	public enum Fields {
		ID(Integer.class),
		ORDRE(Integer.class),
		MENU_MENU(Menu.class),
		PLAT_PLAT(Plat.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;
import java.util.Objects;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Plat dans un menu.
 */
@Entity
@IdClass(MenuPlat.MenuPlatId.class)
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
	name = "menu_plat",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"men_id", "mpl_ordre"})
	}
)
public class MenuPlat {

	/**
	 * Menu contenant ce plat.
	 */
	@Id
	private Menu menu;

	/**
	 * Plat du menu.
	 */
	@Id
	private Plat plat;

	/**
	 * Ordre d'affichage du plat dans le menu.
	 */
	@Column(name = "mpl_ordre", nullable = false, columnDefinition = "int")
	private Integer ordre;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "mpl_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'menu'.
	 *
	 * @return Valeur de {@link #menu menu}.
	 */
	public Menu getMenu() {
		return this.menu;
	}

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
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
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Définit la valeur de {@link #menu menu}.
	 * @param menu Valeur à définir.
	 */
	public void setMenu(Menu menu) {
		this.menu = menu;
	}

	/**
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Définit la valeur de {@link #ordre ordre}.
	 * @param ordre Valeur à définir.
	 */
	public void setOrdre(Integer ordre) {
		this.ordre = ordre;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.MenuPlat MenuPlat}.
	 */
	public enum Fields {
		MENU(Menu.class),
		PLAT(Plat.class),
		ORDRE(Integer.class),
		DATE_CREATION(LocalDateTime.class);

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

	public static class MenuPlatId {

		@JoinColumn(name = "men_id", referencedColumnName = "men_id")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Menu.class)
		private Menu menu;

		@JoinColumn(name = "pla_id", referencedColumnName = "pla_id")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
		private Plat plat;

		/**
		 * Accesseur de 'menu'.
		 *
		 * @return Valeur de {@link #menu menu}.
		 */
		public Menu getMenu() {
			return this.menu;
		}

		/**
		 * Définit la valeur de {@link #menu menu}.
		 * @param menu Valeur à définir.
		 */
		public void setMenu(Menu menu) {
			this.menu = menu;
		}

		/**
		 * Accesseur de 'plat'.
		 *
		 * @return Valeur de {@link #plat plat}.
		 */
		public Plat getPlat() {
			return this.plat;
		}

		/**
		 * Définit la valeur de {@link #plat plat}.
		 * @param plat Valeur à définir.
		 */
		public void setPlat(Plat plat) {
			this.plat = plat;
		}

		public boolean equals(Object o) {
			if (o == this) {
				return true;
			}

			if (o == null) {
				return false;
			}

			if (this.getClass() != o.getClass()) {
				return false;
			}

			MenuPlatId oId = (MenuPlatId) o;

			if (this.menu == null || oId.menu == null || this.plat == null || oId.plat == null) {
				return false;
			}

			return Objects.equals(this.menu.getId(), oId.menu.getId())
					&& Objects.equals(this.plat.getId(), oId.plat.getId());
		}

		@Override
		public int hashCode() {
			return Objects.hash(menu == null ? null : menu.getId(), plat == null ? null : plat.getId());
		}
	}
}

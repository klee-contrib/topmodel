////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

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
	name = "MENU_PLAT",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"MEN_ID", "MPL_ORDRE"})
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
	@Column(name = "MPL_ORDRE", nullable = false, columnDefinition = "int")
	private Integer ordre;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "MPL_DATE_CREATION", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Getter for menu.
	 *
	 * @return value of {@link #menu menu}.
	 */
	public Menu getMenu() {
		return this.menu;
	}

	/**
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
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
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Set the value of {@link #menu menu}.
	 * @param menu value to set.
	 */
	public void setMenu(Menu menu) {
		this.menu = menu;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Set the value of {@link #ordre ordre}.
	 * @param ordre value to set.
	 */
	public void setOrdre(Integer ordre) {
		this.ordre = ordre;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.MenuPlat MenuPlat}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}

	public static class MenuPlatId {

		@JoinColumn(name = "MEN_ID", referencedColumnName = "MEN_ID")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Menu.class)
		private Menu menu;

		@JoinColumn(name = "PLA_ID", referencedColumnName = "PLA_ID")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Plat.class)
		private Plat plat;

		/**
		 * Getter for menu.
		 *
		 * @return value of {@link #menu menu}.
		 */
		public Menu getMenu() {
			return this.menu;
		}

		/**
		 * Set the value of {@link #menu menu}.
		 * @param menu value to set.
		 */
		public void setMenu(Menu menu) {
			this.menu = menu;
		}

		/**
		 * Getter for plat.
		 *
		 * @return value of {@link #plat plat}.
		 */
		public Plat getPlat() {
			return this.plat;
		}

		/**
		 * Set the value of {@link #plat plat}.
		 * @param plat value to set.
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

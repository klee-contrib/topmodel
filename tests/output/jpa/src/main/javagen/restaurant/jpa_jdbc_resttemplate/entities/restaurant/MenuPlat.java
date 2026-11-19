////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

/**
 * Plat dans un menu.
 */
@Table(name = "menu_plat")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuPlat {

	/**
	 * Menu contenant ce plat.
	 */
	@Id
	private Integer menu;

	/**
	 * Plat du menu.
	 */
	@Id
	private Integer plat;

	/**
	 * Ordre d'affichage du plat dans le menu.
	 */
	@NotNull
	@Column("mpl_ordre")
	private Integer ordre;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("mpl_date_creation")
	private LocalDateTime dateCreation;

	/**
	 * Getter for menu.
	 *
	 * @return value of {@link #menu menu}.
	 */
	public Integer getMenu() {
		return this.menu;
	}

	/**
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Integer getPlat() {
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
	public void setMenu(Integer menu) {
		this.menu = menu;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Integer plat) {
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
}

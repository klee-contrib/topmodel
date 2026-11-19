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
	 * Accesseur de 'menu'.
	 *
	 * @return Valeur de {@link #menu menu}.
	 */
	public Integer getMenu() {
		return this.menu;
	}

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Integer getPlat() {
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
	public void setMenu(Integer menu) {
		this.menu = menu;
	}

	/**
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Integer plat) {
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
}

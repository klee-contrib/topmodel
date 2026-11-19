////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un menu en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom du menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getNom() Menu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("men_nom")
	private String nom;

	/**
	 * Description du menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getDescription() Menu#getDescription()}
	 */
	@Size(max = 100)
	@Column("men_description")
	private String description;

	/**
	 * Prix du menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getPrix() Menu#getPrix()}
	 */
	@NotNull
	@Column("men_prix")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getDisponible() Menu#getDisponible()}
	 */
	@NotNull
	@Column("men_disponible")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getDateDebut() Menu#getDateDebut()}
	 */
	@Column("men_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getDateFin() Menu#getDateFin()}
	 */
	@Column("men_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Menu#getRestaurant() Menu#getRestaurant()}
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurantId;

	/**
	 * Catégories de plat disponibles dans le menu.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.CategoriePlat#getCode() CategoriePlat#getCode()}
	 */
	@NotNull
	@Column("cat_code")
	private List<String> categoriesPlat;

	/**
	 * Accesseur de 'nom'.
	 *
	 * @return Valeur de {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Accesseur de 'description'.
	 *
	 * @return Valeur de {@link #description description}.
	 */
	public String getDescription() {
		return this.description;
	}

	/**
	 * Accesseur de 'prix'.
	 *
	 * @return Valeur de {@link #prix prix}.
	 */
	public BigDecimal getPrix() {
		return this.prix;
	}

	/**
	 * Accesseur de 'disponible'.
	 *
	 * @return Valeur de {@link #disponible disponible}.
	 */
	public Boolean getDisponible() {
		return this.disponible;
	}

	/**
	 * Accesseur de 'dateDebut'.
	 *
	 * @return Valeur de {@link #dateDebut dateDebut}.
	 */
	public LocalDateTime getDateDebut() {
		return this.dateDebut;
	}

	/**
	 * Accesseur de 'dateFin'.
	 *
	 * @return Valeur de {@link #dateFin dateFin}.
	 */
	public LocalDateTime getDateFin() {
		return this.dateFin;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Accesseur de 'categoriesPlat'.
	 *
	 * @return Valeur de {@link #categoriesPlat categoriesPlat}.
	 */
	public List<String> getCategoriesPlat() {
		return this.categoriesPlat;
	}

	/**
	 * Définit la valeur de {@link #nom nom}.
	 * @param nom Valeur à définir.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Définit la valeur de {@link #description description}.
	 * @param description Valeur à définir.
	 */
	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * Définit la valeur de {@link #prix prix}.
	 * @param prix Valeur à définir.
	 */
	public void setPrix(BigDecimal prix) {
		this.prix = prix;
	}

	/**
	 * Définit la valeur de {@link #disponible disponible}.
	 * @param disponible Valeur à définir.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Définit la valeur de {@link #dateDebut dateDebut}.
	 * @param dateDebut Valeur à définir.
	 */
	public void setDateDebut(LocalDateTime dateDebut) {
		this.dateDebut = dateDebut;
	}

	/**
	 * Définit la valeur de {@link #dateFin dateFin}.
	 * @param dateFin Valeur à définir.
	 */
	public void setDateFin(LocalDateTime dateFin) {
		this.dateFin = dateFin;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #categoriesPlat categoriesPlat}.
	 * @param categoriesPlat Valeur à définir.
	 */
	public void setCategoriesPlat(List<String> categoriesPlat) {
		this.categoriesPlat = categoriesPlat;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

import restaurant.jpa_server.entities.restaurant.CategoriePlat;

/**
 * Détail d'un menu en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getId() Menu#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getNom() Menu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Description du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getDescription() Menu#getDescription()}
	 */
	@Size(max = 100)
	private String description;

	/**
	 * Prix du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getPrix() Menu#getPrix()}
	 */
	@NotNull
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getDisponible() Menu#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getDateDebut() Menu#getDateDebut()}
	 */
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getDateFin() Menu#getDateFin()}
	 */
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getRestaurant() Menu#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Menu#getDateCreation() Menu#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Catégories de plat dans le menu.
	 */
	@Valid
	@NotNull
	private List<CategoriePlat> categoriesPlat;

	/**
	 * Liste des plats du menu.
	 */
	@Valid
	@NotNull
	private List<PlatItem> plats;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Accesseur de 'categoriesPlat'.
	 *
	 * @return Valeur de {@link #categoriesPlat categoriesPlat}.
	 */
	public List<CategoriePlat> getCategoriesPlat() {
		return this.categoriesPlat;
	}

	/**
	 * Accesseur de 'plats'.
	 *
	 * @return Valeur de {@link #plats plats}.
	 */
	public List<PlatItem> getPlats() {
		return this.plats;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #categoriesPlat categoriesPlat}.
	 * @param categoriesPlat Valeur à définir.
	 */
	public void setCategoriesPlat(List<CategoriePlat> categoriesPlat) {
		this.categoriesPlat = categoriesPlat;
	}

	/**
	 * Définit la valeur de {@link #plats plats}.
	 * @param plats Valeur à définir.
	 */
	public void setPlats(List<PlatItem> plats) {
		this.plats = plats;
	}
}

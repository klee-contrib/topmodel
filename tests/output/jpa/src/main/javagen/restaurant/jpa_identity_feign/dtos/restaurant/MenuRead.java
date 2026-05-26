////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.dtos.restaurant;

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

import restaurant.jpa_identity_feign.entities.restaurant.CategoriePlat;
import restaurant.jpa_identity_feign.entities.restaurant.Menu;
import restaurant.jpa_identity_feign.entities.restaurant.RestaurantMappers;

/**
 * Détail d'un menu en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getId() Menu#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getNom() Menu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Description du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getDescription() Menu#getDescription()}
	 */
	@Size(max = 100)
	private String description;

	/**
	 * Prix du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getPrix() Menu#getPrix()}
	 */
	@NotNull
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getDisponible() Menu#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getDateDebut() Menu#getDateDebut()}
	 */
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getDateFin() Menu#getDateFin()}
	 */
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getRestaurant() Menu#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_identity_feign.entities.restaurant.Menu#getDateCreation() Menu#getDateCreation()}
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
	 * No arg constructor.
	 */
	public MenuRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'MenuRead'.
	 * @param menu Instance de 'Menu'.
	 *
	 * @return Une nouvelle instance de 'MenuRead'.
	 */
	public MenuRead(Menu menu) {
		RestaurantMappers.mapMenuRead(menu, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for description.
	 *
	 * @return value of {@link #description description}.
	 */
	public String getDescription() {
		return this.description;
	}

	/**
	 * Getter for prix.
	 *
	 * @return value of {@link #prix prix}.
	 */
	public BigDecimal getPrix() {
		return this.prix;
	}

	/**
	 * Getter for disponible.
	 *
	 * @return value of {@link #disponible disponible}.
	 */
	public Boolean getDisponible() {
		return this.disponible;
	}

	/**
	 * Getter for dateDebut.
	 *
	 * @return value of {@link #dateDebut dateDebut}.
	 */
	public LocalDateTime getDateDebut() {
		return this.dateDebut;
	}

	/**
	 * Getter for dateFin.
	 *
	 * @return value of {@link #dateFin dateFin}.
	 */
	public LocalDateTime getDateFin() {
		return this.dateFin;
	}

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Getter for categoriesPlat.
	 *
	 * @return value of {@link #categoriesPlat categoriesPlat}.
	 */
	public List<CategoriePlat> getCategoriesPlat() {
		return this.categoriesPlat;
	}

	/**
	 * Getter for plats.
	 *
	 * @return value of {@link #plats plats}.
	 */
	public List<PlatItem> getPlats() {
		return this.plats;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #description description}.
	 * @param description value to set.
	 */
	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * Set the value of {@link #prix prix}.
	 * @param prix value to set.
	 */
	public void setPrix(BigDecimal prix) {
		this.prix = prix;
	}

	/**
	 * Set the value of {@link #disponible disponible}.
	 * @param disponible value to set.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Set the value of {@link #dateDebut dateDebut}.
	 * @param dateDebut value to set.
	 */
	public void setDateDebut(LocalDateTime dateDebut) {
		this.dateDebut = dateDebut;
	}

	/**
	 * Set the value of {@link #dateFin dateFin}.
	 * @param dateFin value to set.
	 */
	public void setDateFin(LocalDateTime dateFin) {
		this.dateFin = dateFin;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link #categoriesPlat categoriesPlat}.
	 * @param categoriesPlat value to set.
	 */
	public void setCategoriesPlat(List<CategoriePlat> categoriesPlat) {
		this.categoriesPlat = categoriesPlat;
	}

	/**
	 * Set the value of {@link #plats plats}.
	 * @param plats value to set.
	 */
	public void setPlats(List<PlatItem> plats) {
		this.plats = plats;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_feign.dtos.restaurant.MenuRead MenuRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		RESTAURANT_ID(Integer.class),
		DATE_CREATION(LocalDateTime.class),
		CATEGORIES_PLAT(List.class),
		PLATS(List.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

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
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getId() Menu#getId()}
	 */
	@NotNull
	@Column("men_id")
	private Integer id;

	/**
	 * Nom du menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getNom() Menu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("men_nom")
	private String nom;

	/**
	 * Description du menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getDescription() Menu#getDescription()}
	 */
	@Size(max = 100)
	@Column("men_description")
	private String description;

	/**
	 * Prix du menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getPrix() Menu#getPrix()}
	 */
	@NotNull
	@Column("men_prix")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getDisponible() Menu#getDisponible()}
	 */
	@NotNull
	@Column("men_disponible")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getDateDebut() Menu#getDateDebut()}
	 */
	@Column("men_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getDateFin() Menu#getDateFin()}
	 */
	@Column("men_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getRestaurantIdRestaurant() Menu#getRestaurantIdRestaurant()}
	 */
	@NotNull
	@Column("res_id_restaurant")
	private Integer restaurantIdRestaurant;

	/**
	 * Association réciproque de MenuPlat.MenuIdMenu.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Menu#getMenuPlatsMenu() Menu#getMenuPlatsMenu()}
	 */
	@NotNull
	@Column("mpl_id_menu")
	private List<Integer> menuPlatsMenu;

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
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
	}

	/**
	 * Getter for menuPlatsMenu.
	 *
	 * @return value of {@link #menuPlatsMenu menuPlatsMenu}.
	 */
	public List<Integer> getMenuPlatsMenu() {
		return this.menuPlatsMenu;
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
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #menuPlatsMenu menuPlatsMenu}.
	 * @param menuPlatsMenu value to set.
	 */
	public void setMenuPlatsMenu(List<Integer> menuPlatsMenu) {
		this.menuPlatsMenu = menuPlatsMenu;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.MenuRead MenuRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		RESTAURANT_ID_RESTAURANT(Integer.class),
		MENU_PLATS_MENU(List.class);

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

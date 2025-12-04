////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.OrderBy;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Menu du restaurant.
 */
@Entity
@Table(name = "MENU")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Menu {

	/**
	 * Identifiant du menu.
	 */
	@Id
	@Column(name = "MEN_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_MENU")
	@SequenceGenerator(sequenceName = "SEQ_MENU", name = "SEQ_MENU", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du menu.
	 */
	@Column(name = "MEN_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Description du menu.
	 */
	@Column(name = "MEN_DESCRIPTION", length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Prix du menu.
	 */
	@Column(name = "MEN_PRIX", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 */
	@Column(name = "MEN_DISPONIBLE", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 */
	@Column(name = "MEN_DATE_DEBUT", columnDefinition = "timestamp")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 */
	@Column(name = "MEN_DATE_FIN", columnDefinition = "timestamp")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 */
	@JoinColumn(name = "RES_ID_RESTAURANT", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurantRestaurant;

	/**
	 * Association réciproque de MenuPlat.MenuIdMenu.
	 */
	@OrderBy("ordre ASC")
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "menuMenu")
	private List<MenuPlat> menuPlatsMenu;

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
	 * Getter for restaurantRestaurant.
	 *
	 * @return value of {@link #restaurantRestaurant restaurantRestaurant}.
	 */
	public Restaurant getRestaurantRestaurant() {
		return this.restaurantRestaurant;
	}

	/**
	 * Getter for menuPlatsMenu.
	 *
	 * @return value of {@link #menuPlatsMenu menuPlatsMenu}.
	 */
	public List<MenuPlat> getMenuPlatsMenu() {
		if (this.menuPlatsMenu == null) {
			this.menuPlatsMenu = new ArrayList<>();
		}
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
	 * Set the value of {@link #restaurantRestaurant restaurantRestaurant}.
	 * @param restaurantRestaurant value to set.
	 */
	public void setRestaurantRestaurant(Restaurant restaurantRestaurant) {
		this.restaurantRestaurant = restaurantRestaurant;
	}

	/**
	 * Set the value of {@link #menuPlatsMenu menuPlatsMenu}.
	 * @param menuPlatsMenu value to set.
	 */
	public void setMenuPlatsMenu(List<MenuPlat> menuPlatsMenu) {
		this.menuPlatsMenu = menuPlatsMenu;
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Menu#menuPlatsMenu menuPlatsMenu}.
	 * @param menuPlat value to add to menuMenu.
	 */
	void addMenuPlatMenu(MenuPlat menuPlat) {
		this.menuPlatsMenu.add(menuPlat);
		menuPlat.setMenuMenu(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Menu#menuPlatsMenu menuPlatsMenu}.
	 * @param menuPlat menuPlat value to remove.
	 */
	void removeMenuPlatMenu(MenuPlat menuPlat) {
		this.menuPlatsMenu.remove(menuPlat);
		menuPlat.setMenuMenu(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Menu Menu}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		RESTAURANT_RESTAURANT(Restaurant.class),
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

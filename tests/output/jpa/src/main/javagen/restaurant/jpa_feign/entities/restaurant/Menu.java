////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
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
@Table(name = "menu")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Menu {

	/**
	 * Identifiant du menu.
	 */
	@Id
	@Column(name = "men_id", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_MENU")
	@SequenceGenerator(sequenceName = "SEQ_MENU", name = "SEQ_MENU", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du menu.
	 */
	@Column(name = "men_nom", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Description du menu.
	 */
	@Column(name = "men_description", length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Prix du menu.
	 */
	@Column(name = "men_prix", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 */
	@Column(name = "men_disponible", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 */
	@Column(name = "men_date_debut", columnDefinition = "timestamp")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 */
	@Column(name = "men_date_fin", columnDefinition = "timestamp")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Association réciproque de MenuPlat.Menu.
	 */
	@OrderBy("ordre ASC")
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "menu")
	private List<MenuPlat> plats;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "men_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

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
	 * Accesseur de 'restaurant'.
	 *
	 * @return Valeur de {@link #restaurant restaurant}.
	 */
	public Restaurant getRestaurant() {
		return this.restaurant;
	}

	/**
	 * Accesseur de 'plats'.
	 *
	 * @return Valeur de {@link #plats plats}.
	 */
	public List<MenuPlat> getPlats() {
		if (this.plats == null) {
			this.plats = new ArrayList<>();
		}
		return this.plats;
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
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #plats plats}.
	 * @param plats Valeur à définir.
	 */
	public void setPlats(List<MenuPlat> plats) {
		this.plats = plats;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Menu#plats plats}.
	 * @param menuPlat Valeur à ajouter à 'menu'.
	 */
	void addMenuPlat(MenuPlat menuPlat) {
		this.plats.add(menuPlat);
		menuPlat.setMenu(this);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Menu#plats plats}.
	 * @param menuPlat Valeur de 'menuPlat' à supprimer.
	 */
	void removeMenuPlat(MenuPlat menuPlat) {
		this.plats.remove(menuPlat);
		menuPlat.setMenu(null);
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Menu Menu}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		RESTAURANT(Restaurant.class),
		PLATS(List.class),
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
}

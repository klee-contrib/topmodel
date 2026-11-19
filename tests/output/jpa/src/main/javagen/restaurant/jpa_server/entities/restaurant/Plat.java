////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorColumn;
import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Inheritance;
import jakarta.persistence.InheritanceType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToOne;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Plat du menu.
 */
@Entity
@Table(name = "plat")
@DiscriminatorValue("AUTRE")
@DiscriminatorColumn(name = "cat_code")
@EntityListeners(AuditingEntityListener.class)
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Plat {

	/**
	 * Identifiant du plat.
	 */
	@Id
	@Column(name = "pla_id", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PLAT")
	@SequenceGenerator(sequenceName = "SEQ_PLAT", name = "SEQ_PLAT", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du plat.
	 */
	@Column(name = "pla_nom", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Description du plat.
	 */
	@Column(name = "pla_description", length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Prix du plat.
	 */
	@Column(name = "pla_prix", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 */
	@Column(name = "pla_disponible", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CategoriePlat.class)
	@JoinColumn(name = "cat_code", referencedColumnName = "cat_code", insertable = false, updatable = false)
	private CategoriePlat categoriePlat;

	/**
	 * Restaurant proposant ce plat.
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Restaurant qui a créé ce plat.
	 */
	@JoinColumn(name = "lie_id_createur", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurantCreateur;

	/**
	 * Association réciproque de Promotion.Plat.
	 */
	@OneToOne(fetch = FetchType.LAZY, optional = true, cascade = CascadeType.ALL, mappedBy = "plat")
	private Promotion promotion;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "pla_date_creation", nullable = false, columnDefinition = "timestamp")
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
	 * Accesseur de 'categoriePlat'.
	 *
	 * @return Valeur de {@link #categoriePlat categoriePlat}.
	 */
	public CategoriePlat getCategoriePlat() {
		return this.categoriePlat;
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
	 * Accesseur de 'restaurantCreateur'.
	 *
	 * @return Valeur de {@link #restaurantCreateur restaurantCreateur}.
	 */
	public Restaurant getRestaurantCreateur() {
		return this.restaurantCreateur;
	}

	/**
	 * Accesseur de 'promotion'.
	 *
	 * @return Valeur de {@link #promotion promotion}.
	 */
	public Promotion getPromotion() {
		return this.promotion;
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
	 * Définit la valeur de {@link #categoriePlat categoriePlat}.
	 * @param categoriePlat Valeur à définir.
	 */
	public void setCategoriePlat(CategoriePlat categoriePlat) {
		this.categoriePlat = categoriePlat;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #restaurantCreateur restaurantCreateur}.
	 * @param restaurantCreateur Valeur à définir.
	 */
	public void setRestaurantCreateur(Restaurant restaurantCreateur) {
		this.restaurantCreateur = restaurantCreateur;
	}

	/**
	 * Définit la valeur de {@link #promotion promotion}.
	 * @param promotion Valeur à définir.
	 */
	public void setPromotion(Promotion promotion) {
		this.promotion = promotion;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Plat Plat}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		CATEGORIE_PLAT(CategoriePlat.class),
		RESTAURANT(Restaurant.class),
		RESTAURANT_CREATEUR(Restaurant.class),
		PROMOTION(Promotion.class),
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

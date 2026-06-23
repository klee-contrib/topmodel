////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

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
@Table(name = "PLAT")
@DiscriminatorValue("AUTRE")
@DiscriminatorColumn(name = "CAT_CODE")
@EntityListeners(AuditingEntityListener.class)
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Plat {

	/**
	 * Identifiant du plat.
	 */
	@Id
	@Column(name = "PLA_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PLAT")
	@SequenceGenerator(sequenceName = "SEQ_PLAT", name = "SEQ_PLAT", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du plat.
	 */
	@Column(name = "PLA_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Description du plat.
	 */
	@Column(name = "PLA_DESCRIPTION", length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Prix du plat.
	 */
	@Column(name = "PLA_PRIX", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 */
	@Column(name = "PLA_DISPONIBLE", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CategoriePlat.class)
	@JoinColumn(name = "CAT_CODE", referencedColumnName = "CAT_CODE", insertable = false, updatable = false)
	private CategoriePlat categoriePlat;

	/**
	 * Restaurant proposant ce plat.
	 */
	@JoinColumn(name = "LIE_ID", referencedColumnName = "LIE_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Association réciproque de Promotion.Plat.
	 */
	@OneToOne(fetch = FetchType.LAZY, optional = true, cascade = CascadeType.ALL, mappedBy = "plat")
	private Promotion promotion;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "PLA_DATE_CREATION", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

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
	 * Getter for categoriePlat.
	 *
	 * @return value of {@link #categoriePlat categoriePlat}.
	 */
	public CategoriePlat getCategoriePlat() {
		return this.categoriePlat;
	}

	/**
	 * Getter for restaurant.
	 *
	 * @return value of {@link #restaurant restaurant}.
	 */
	public Restaurant getRestaurant() {
		return this.restaurant;
	}

	/**
	 * Getter for promotion.
	 *
	 * @return value of {@link #promotion promotion}.
	 */
	public Promotion getPromotion() {
		return this.promotion;
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
	 * Set the value of {@link #categoriePlat categoriePlat}.
	 * @param categoriePlat value to set.
	 */
	public void setCategoriePlat(CategoriePlat categoriePlat) {
		this.categoriePlat = categoriePlat;
	}

	/**
	 * Set the value of {@link #restaurant restaurant}.
	 * @param restaurant value to set.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Set the value of {@link #promotion promotion}.
	 * @param promotion value to set.
	 */
	public void setPromotion(Promotion promotion) {
		this.promotion = promotion;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Plat Plat}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		CATEGORIE_PLAT(CategoriePlat.class),
		RESTAURANT(Restaurant.class),
		PROMOTION(Promotion.class),
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
}

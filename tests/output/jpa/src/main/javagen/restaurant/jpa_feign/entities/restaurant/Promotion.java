////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.MapsId;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;

/**
 * Promotion sur un plat.
 */
@Entity
@Table(name = "promotion")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Promotion {

	/**
	 * Identifiant technique mappé avec celui de la classe {@link restaurant.jpa_feign.entities.restaurant.Plat} Plat.
	 */
	@Id
	private Integer platId;

	/**
	 * Plat concerné par la promotion.
	 */
	@MapsId
	@JoinColumn(name = "pla_id", referencedColumnName = "pla_id", unique = true)
	@OneToOne(fetch = FetchType.LAZY, optional = false, cascade = CascadeType.ALL)
	private Plat plat;

	/**
	 * Libellé de la promotion.
	 */
	@Column(name = "pro_libelle", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 */
	@Column(name = "pro_pourcentage_reduction", nullable = false, columnDefinition = "int")
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 */
	@Column(name = "pro_date_debut", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 */
	@Column(name = "pro_date_fin", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 */
	@Column(name = "pro_active", nullable = false, columnDefinition = "boolean")
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 */
	@JoinColumn(name = "lie_id", referencedColumnName = "lie_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Restaurant.class)
	private Restaurant restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "pro_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
	}

	/**
	 * Accesseur de 'libelle'.
	 *
	 * @return Valeur de {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Accesseur de 'pourcentageReduction'.
	 *
	 * @return Valeur de {@link #pourcentageReduction pourcentageReduction}.
	 */
	public Integer getPourcentageReduction() {
		return this.pourcentageReduction;
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
	 * Accesseur de 'active'.
	 *
	 * @return Valeur de {@link #active active}.
	 */
	public Boolean getActive() {
		return this.active;
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
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Accesseur de 'platId'.
	 *
	 * @return Valeur de {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Définit la valeur de {@link #libelle libelle}.
	 * @param libelle Valeur à définir.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Définit la valeur de {@link #pourcentageReduction pourcentageReduction}.
	 * @param pourcentageReduction Valeur à définir.
	 */
	public void setPourcentageReduction(Integer pourcentageReduction) {
		this.pourcentageReduction = pourcentageReduction;
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
	 * Définit la valeur de {@link #active active}.
	 * @param active Valeur à définir.
	 */
	public void setActive(Boolean active) {
		this.active = active;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link restaurant.jpa_feign.entities.restaurant.Promotion#platId platId}.
	 * @param platId Définit la valeur de {@link restaurant.jpa_feign.entities.restaurant.Promotion#platId platId}.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Promotion Promotion}.
	 */
	public enum Fields {
		PLAT(Plat.class),
		LIBELLE(String.class),
		POURCENTAGE_REDUCTION(Integer.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		ACTIVE(Boolean.class),
		RESTAURANT(Restaurant.class),
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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

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
	 * Identifiant technique mappé avec celui de la classe {@link restaurant.jpa_server.entities.restaurant.Plat} Plat.
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
	 * Getter for plat.
	 *
	 * @return value of {@link #plat plat}.
	 */
	public Plat getPlat() {
		return this.plat;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for pourcentageReduction.
	 *
	 * @return value of {@link #pourcentageReduction pourcentageReduction}.
	 */
	public Integer getPourcentageReduction() {
		return this.pourcentageReduction;
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
	 * Getter for active.
	 *
	 * @return value of {@link #active active}.
	 */
	public Boolean getActive() {
		return this.active;
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
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for platId.
	 *
	 * @return value of {@link restaurant.jpa_server.entities.restaurant.Promotion#platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Set the value of {@link #plat plat}.
	 * @param plat value to set.
	 */
	public void setPlat(Plat plat) {
		this.plat = plat;
	}

	/**
	 * Set the value of {@link #libelle libelle}.
	 * @param libelle value to set.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link #pourcentageReduction pourcentageReduction}.
	 * @param pourcentageReduction value to set.
	 */
	public void setPourcentageReduction(Integer pourcentageReduction) {
		this.pourcentageReduction = pourcentageReduction;
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
	 * Set the value of {@link #active active}.
	 * @param active value to set.
	 */
	public void setActive(Boolean active) {
		this.active = active;
	}

	/**
	 * Set the value of {@link #restaurant restaurant}.
	 * @param restaurant value to set.
	 */
	public void setRestaurant(Restaurant restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Setter for platId.
	 * @param platId Set the value of {@link restaurant.jpa_server.entities.restaurant.Promotion#platId platId}.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Promotion Promotion}.
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
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

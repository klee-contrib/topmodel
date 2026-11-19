////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une promotion en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Plat concerné par la promotion.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getPlat() Promotion#getPlat()}
	 */
	@NotNull
	@Column("pla_id")
	private Integer platId;

	/**
	 * Libellé de la promotion.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getLibelle() Promotion#getLibelle()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("pro_libelle")
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getPourcentageReduction() Promotion#getPourcentageReduction()}
	 */
	@NotNull
	@Column("pro_pourcentage_reduction")
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getDateDebut() Promotion#getDateDebut()}
	 */
	@NotNull
	@Column("pro_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getDateFin() Promotion#getDateFin()}
	 */
	@NotNull
	@Column("pro_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getActive() Promotion#getActive()}
	 */
	@NotNull
	@Column("pro_active")
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getRestaurant() Promotion#getRestaurant()}
	 */
	@Column("lie_id")
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Promotion#getDateCreation() Promotion#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	@Column("pro_date_creation")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'platId'.
	 *
	 * @return Valeur de {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
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
	 * Définit la valeur de {@link #platId platId}.
	 * @param platId Valeur à définir.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
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
}

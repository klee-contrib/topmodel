////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Promotion sur les plats.
 */
@Table(name = "promotion")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Promotion {

	/**
	 * Identifiant de la promotion.
	 */
	@Id
	@Column("pro_id")
	private Integer id;

	/**
	 * Libellé de la promotion.
	 */
	@NotNull
	@Column("pro_libelle")
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 */
	@NotNull
	@Column("pro_pourcentage_reduction")
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 */
	@NotNull
	@Column("pro_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 */
	@NotNull
	@Column("pro_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 */
	@NotNull
	@Column("pro_active")
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 */
	@Column("res_id_restaurant")
	private Integer restaurantIdRestaurant;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
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
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}
}

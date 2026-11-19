////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

/**
 * Promotion sur un plat.
 */
@Table(name = "promotion")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Promotion {

	/**
	 * Plat concerné par la promotion.
	 */
	@Id
	@Column("pla_id")
	private Integer plat;

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
	@Column("lie_id")
	private Integer restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("pro_date_creation")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'plat'.
	 *
	 * @return Valeur de {@link #plat plat}.
	 */
	public Integer getPlat() {
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
	public Integer getRestaurant() {
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
	 * Définit la valeur de {@link #plat plat}.
	 * @param plat Valeur à définir.
	 */
	public void setPlat(Integer plat) {
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
	public void setRestaurant(Integer restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

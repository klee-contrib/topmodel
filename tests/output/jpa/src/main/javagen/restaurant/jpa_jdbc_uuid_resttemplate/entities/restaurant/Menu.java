////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant;

import java.math.BigDecimal;
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
 * Menu du restaurant.
 */
@Table(name = "menu")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Menu {

	/**
	 * Identifiant du menu.
	 */
	@Id
	@Column("men_id")
	private Integer id;

	/**
	 * Nom du menu.
	 */
	@NotNull
	@Column("men_nom")
	private String nom;

	/**
	 * Description du menu.
	 */
	@Column("men_description")
	private String description;

	/**
	 * Prix du menu.
	 */
	@NotNull
	@Column("men_prix")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 */
	@NotNull
	@Column("men_disponible")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 */
	@Column("men_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 */
	@Column("men_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("men_date_creation")
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
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

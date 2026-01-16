////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

/**
 * Détail d'un menu en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class MenuRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getId() Menu#getId()}
	 */
	@NotNull
	@Column("men_id")
	private Integer id;

	/**
	 * Nom du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getNom() Menu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("men_nom")
	private String nom;

	/**
	 * Description du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getDescription() Menu#getDescription()}
	 */
	@Size(max = 100)
	@Column("men_description")
	private String description;

	/**
	 * Prix du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getPrix() Menu#getPrix()}
	 */
	@NotNull
	@Column("men_prix")
	private BigDecimal prix;

	/**
	 * Indique si le menu est disponible.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getDisponible() Menu#getDisponible()}
	 */
	@NotNull
	@Column("men_disponible")
	private Boolean disponible = true;

	/**
	 * Date de début de validité du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getDateDebut() Menu#getDateDebut()}
	 */
	@Column("men_date_debut")
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de validité du menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getDateFin() Menu#getDateFin()}
	 */
	@Column("men_date_fin")
	private LocalDateTime dateFin;

	/**
	 * Restaurant proposant ce menu.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Menu#getRestaurantId() Menu#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

	/**
	 * Liste des plats du menu.
	 */
	@Valid
	@NotNull
	private List<PlatItem> plats;

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
	 * Getter for plats.
	 *
	 * @return value of {@link #plats plats}.
	 */
	public List<PlatItem> getPlats() {
		return this.plats;
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
	 * Set the value of {@link #plats plats}.
	 * @param plats value to set.
	 */
	public void setPlats(List<PlatItem> plats) {
		this.plats = plats;
	}
}

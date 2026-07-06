////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

import restaurant.jpa_server.enums.restaurant.CategoriePlatCode;

/**
 * Détail d'un plat en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getId() Plat#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getNom() Plat#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Description du plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getDescription() Plat#getDescription()}
	 */
	@Size(max = 100)
	private String description;

	/**
	 * Prix du plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getPrix() Plat#getPrix()}
	 */
	@NotNull
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getDisponible() Plat#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getCategoriePlat() Plat#getCategoriePlat()}
	 */
	@NotNull
	private CategoriePlatCode categoriePlatCode;

	/**
	 * Restaurant proposant ce plat.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getRestaurant() Plat#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias of {@link restaurant.jpa_server.entities.restaurant.Plat#getDateCreation() Plat#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
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
	 * Getter for categoriePlatCode.
	 *
	 * @return value of {@link #categoriePlatCode categoriePlatCode}.
	 */
	public CategoriePlatCode getCategoriePlatCode() {
		return this.categoriePlatCode;
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
	 * Set the value of {@link #categoriePlatCode categoriePlatCode}.
	 * @param categoriePlatCode value to set.
	 */
	public void setCategoriePlatCode(CategoriePlatCode categoriePlatCode) {
		this.categoriePlatCode = categoriePlatCode;
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

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
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getId() Plat#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getNom() Plat#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Description du plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getDescription() Plat#getDescription()}
	 */
	@Size(max = 100)
	private String description;

	/**
	 * Prix du plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getPrix() Plat#getPrix()}
	 */
	@NotNull
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getDisponible() Plat#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getCategoriePlat() Plat#getCategoriePlat()}
	 */
	@NotNull
	private CategoriePlatCode categoriePlatCode;

	/**
	 * Restaurant proposant ce plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getRestaurant() Plat#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Restaurant qui a créé ce plat.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getRestaurantCreateur() Plat#getRestaurantCreateur()}
	 */
	@NotNull
	private Integer restaurantIdCreateur;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.Plat#getDateCreation() Plat#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
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
	 * Accesseur de 'categoriePlatCode'.
	 *
	 * @return Valeur de {@link #categoriePlatCode categoriePlatCode}.
	 */
	public CategoriePlatCode getCategoriePlatCode() {
		return this.categoriePlatCode;
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
	 * Accesseur de 'restaurantIdCreateur'.
	 *
	 * @return Valeur de {@link #restaurantIdCreateur restaurantIdCreateur}.
	 */
	public Integer getRestaurantIdCreateur() {
		return this.restaurantIdCreateur;
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
	 * Définit la valeur de {@link #categoriePlatCode categoriePlatCode}.
	 * @param categoriePlatCode Valeur à définir.
	 */
	public void setCategoriePlatCode(CategoriePlatCode categoriePlatCode) {
		this.categoriePlatCode = categoriePlatCode;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #restaurantIdCreateur restaurantIdCreateur}.
	 * @param restaurantIdCreateur Valeur à définir.
	 */
	public void setRestaurantIdCreateur(Integer restaurantIdCreateur) {
		this.restaurantIdCreateur = restaurantIdCreateur;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

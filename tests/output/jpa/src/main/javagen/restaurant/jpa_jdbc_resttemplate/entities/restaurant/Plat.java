////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

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
 * Plat du menu.
 */
@Table(name = "plat")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Plat {

	/**
	 * Identifiant du plat.
	 */
	@Id
	@Column("pla_id")
	private Integer id;

	/**
	 * Nom du plat.
	 */
	@NotNull
	@Column("pla_nom")
	private String nom;

	/**
	 * Description du plat.
	 */
	@Column("pla_description")
	private String description;

	/**
	 * Prix du plat.
	 */
	@NotNull
	@Column("pla_prix")
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 */
	@NotNull
	@Column("pla_disponible")
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 */
	@NotNull
	@Column("cat_code")
	private String categoriePlat;

	/**
	 * Restaurant proposant ce plat.
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurant;

	/**
	 * Restaurant qui a créé ce plat.
	 */
	@NotNull
	@Column("lie_id_createur")
	private Integer restaurantCreateur;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("pla_date_creation")
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
	 * Accesseur de 'categoriePlat'.
	 *
	 * @return Valeur de {@link #categoriePlat categoriePlat}.
	 */
	public String getCategoriePlat() {
		return this.categoriePlat;
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
	 * Accesseur de 'restaurantCreateur'.
	 *
	 * @return Valeur de {@link #restaurantCreateur restaurantCreateur}.
	 */
	public Integer getRestaurantCreateur() {
		return this.restaurantCreateur;
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
	 * Définit la valeur de {@link #categoriePlat categoriePlat}.
	 * @param categoriePlat Valeur à définir.
	 */
	public void setCategoriePlat(String categoriePlat) {
		this.categoriePlat = categoriePlat;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Integer restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #restaurantCreateur restaurantCreateur}.
	 * @param restaurantCreateur Valeur à définir.
	 */
	public void setRestaurantCreateur(Integer restaurantCreateur) {
		this.restaurantCreateur = restaurantCreateur;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

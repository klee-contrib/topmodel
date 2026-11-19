////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

/**
 * Table du restaurant.
 */
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@org.springframework.data.relational.core.mapping.Table(name = "table")
public class Table {

	/**
	 * Identifiant de la table.
	 */
	@Id
	@Column("tab_id")
	private Integer id;

	/**
	 * Numéro de la table.
	 */
	@NotNull
	@Column("tab_numero")
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 */
	@NotNull
	@Column("tab_capacite")
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 */
	@NotNull
	@Column("tab_disponible")
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("tab_date_creation")
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
	 * Accesseur de 'numero'.
	 *
	 * @return Valeur de {@link #numero numero}.
	 */
	public String getNumero() {
		return this.numero;
	}

	/**
	 * Accesseur de 'capacite'.
	 *
	 * @return Valeur de {@link #capacite capacite}.
	 */
	public Integer getCapacite() {
		return this.capacite;
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
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #numero numero}.
	 * @param numero Valeur à définir.
	 */
	public void setNumero(String numero) {
		this.numero = numero;
	}

	/**
	 * Définit la valeur de {@link #capacite capacite}.
	 * @param capacite Valeur à définir.
	 */
	public void setCapacite(Integer capacite) {
		this.capacite = capacite;
	}

	/**
	 * Définit la valeur de {@link #disponible disponible}.
	 * @param disponible Valeur à définir.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
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

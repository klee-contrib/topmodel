////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.RestaurantMappers;
import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant;

/**
 * Détail d'une table en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la table.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant#getId() TableRestaurant#getId()}
	 */
	@NotNull
	@Column("tab_id")
	private Integer id;

	/**
	 * Numéro de la table.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant#getNumero() TableRestaurant#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("tab_numero")
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant#getCapacite() TableRestaurant#getCapacite()}
	 */
	@NotNull
	@Column("tab_capacite")
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant#getDisponible() TableRestaurant#getDisponible()}
	 */
	@NotNull
	@Column("tab_disponible")
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.TableRestaurant#getRestaurantId() TableRestaurant#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

	/**
	 * No arg constructor.
	 */
	public TableRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'TableRead'.
	 * @param table Instance de 'TableRestaurant'.
	 *
	 * @return Une nouvelle instance de 'TableRead'.
	 */
	public TableRead(TableRestaurant table) {
		RestaurantMappers.mapTableRead(table, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for numero.
	 *
	 * @return value of {@link #numero numero}.
	 */
	public String getNumero() {
		return this.numero;
	}

	/**
	 * Getter for capacite.
	 *
	 * @return value of {@link #capacite capacite}.
	 */
	public Integer getCapacite() {
		return this.capacite;
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
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #numero numero}.
	 * @param numero value to set.
	 */
	public void setNumero(String numero) {
		this.numero = numero;
	}

	/**
	 * Set the value of {@link #capacite capacite}.
	 * @param capacite value to set.
	 */
	public void setCapacite(Integer capacite) {
		this.capacite = capacite;
	}

	/**
	 * Set the value of {@link #disponible disponible}.
	 * @param disponible value to set.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une table en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TableRead implements TableItem, Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la table.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getId() Table#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Numéro de la table.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getNumero() Table#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getCapacite() Table#getCapacite()}
	 */
	@NotNull
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getDisponible() Table#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getRestaurantId() Table#getRestaurantId()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Table#getDateCreation() Table#getDateCreation()}
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

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.TableRead TableRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NUMERO(String.class),
		CAPACITE(Integer.class),
		DISPONIBLE(Boolean.class),
		RESTAURANT_ID(Integer.class),
		DATE_CREATION(LocalDateTime.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

/**
 * Table du restaurant.
 */
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "TABLE", uniqueConstraints = {@UniqueConstraint(columnNames = {"RES_ID", "TAB_NUMERO"})})
public class TableRestaurant {

	/**
	 * Identifiant de la table.
	 */
	@Id
	@Column(columnDefinition = "int", name = "TAB_ID", nullable = false)
	@GeneratedValue(generator = "SEQ_TABLE", strategy = GenerationType.SEQUENCE)
	@SequenceGenerator(allocationSize = 50, initialValue = 1000, name = "SEQ_TABLE", sequenceName = "SEQ_TABLE")
	private Integer id;

	/**
	 * Numéro de la table.
	 */
	@Column(columnDefinition = "varchar", length = 10, name = "TAB_NUMERO", nullable = false)
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 */
	@Column(columnDefinition = "int", name = "TAB_CAPACITE", nullable = false)
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 */
	@Column(columnDefinition = "boolean", name = "TAB_DISPONIBLE", nullable = false)
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 */
	@Column(columnDefinition = "int", name = "RES_ID", nullable = false)
	private Integer restaurantId;

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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.TableRestaurant TableRestaurant}.
	 */
	public enum Fields {
		ID(Integer.class),
		NUMERO(String.class),
		CAPACITE(Integer.class),
		DISPONIBLE(Boolean.class),
		RESTAURANT_ID(Integer.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

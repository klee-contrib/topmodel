////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.UniqueConstraint;

/**
 * Table du restaurant.
 */
@Entity
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@jakarta.persistence.Table(
	name = "table",
	uniqueConstraints = {
		@UniqueConstraint(columnNames = {"lie_id", "tab_numero"})
	}
)
public class Table {

	/**
	 * Identifiant de la table.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "tab_id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Numéro de la table.
	 */
	@Column(name = "tab_numero", nullable = false, length = 10, columnDefinition = "varchar")
	private String numero;

	/**
	 * Capacité de la table (nombre de places).
	 */
	@Column(name = "tab_capacite", nullable = false, columnDefinition = "int")
	private Integer capacite;

	/**
	 * Indique si la table est disponible.
	 */
	@Column(name = "tab_disponible", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Restaurant auquel appartient la table.
	 */
	@Column(name = "lie_id", nullable = false, columnDefinition = "int")
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "tab_date_creation", nullable = false, columnDefinition = "timestamp")
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
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Table Table}.
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

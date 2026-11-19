////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Inheritance;
import jakarta.persistence.InheritanceType;
import jakarta.persistence.SequenceGenerator;

/**
 * Vaisselle de restaurant.
 */
@Entity
@Inheritance(strategy = InheritanceType.TABLE_PER_CLASS)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class Vaisselle {

	/**
	 * Id de la vaisselle.
	 */
	@Id
	@Column(name = "vsl_id", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_VAISSELLE")
	@SequenceGenerator(sequenceName = "SEQ_VAISSELLE", name = "SEQ_VAISSELLE", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Description de la vaisselle.
	 */
	@Column(name = "vsl_description", nullable = false, length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
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
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #description description}.
	 * @param description Valeur à définir.
	 */
	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Vaisselle Vaisselle}.
	 */
	public enum Fields {
		ID(Integer.class),
		DESCRIPTION(String.class);

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

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Résultat de suppression d'une commande.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeDeleteResult implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	private LocalDateTime dateCommande;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'dateCommande'.
	 *
	 * @return Valeur de {@link #dateCommande dateCommande}.
	 */
	public LocalDateTime getDateCommande() {
		return this.dateCommande;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #dateCommande dateCommande}.
	 * @param dateCommande Valeur à définir.
	 */
	public void setDateCommande(LocalDateTime dateCommande) {
		this.dateCommande = dateCommande;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.CommandeDeleteResult CommandeDeleteResult}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_COMMANDE(LocalDateTime.class);

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

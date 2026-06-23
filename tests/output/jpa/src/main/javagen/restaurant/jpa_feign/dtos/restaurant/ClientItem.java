////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

import restaurant.jpa_feign.entities.restaurant.Client;
import restaurant.jpa_feign.entities.restaurant.RestaurantMappers;

/**
 * Détail d'un client en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientItem extends PersonneItem implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom complet du client (calculé).
	 */
	@Size(max = 100)
	private String nomComplet;

	/**
	 * No arg constructor.
	 */
	public ClientItem() {
		super();
	}

	/**
	 * Crée une nouvelle instance de 'ClientItem'.
	 * @param client Instance de 'Client'.
	 *
	 * @return Une nouvelle instance de 'ClientItem'.
	 */
	public ClientItem(Client client) {
		super();
		RestaurantMappers.mapClientItem(client, this);
	}

	/**
	 * Getter for nomComplet.
	 *
	 * @return value of {@link #nomComplet nomComplet}.
	 */
	public String getNomComplet() {
		return this.nomComplet;
	}

	/**
	 * Set the value of {@link #nomComplet nomComplet}.
	 * @param nomComplet value to set.
	 */
	public void setNomComplet(String nomComplet) {
		this.nomComplet = nomComplet;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.ClientItem ClientItem}.
	 */
	public enum Fields {
		NOM_COMPLET(String.class);

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

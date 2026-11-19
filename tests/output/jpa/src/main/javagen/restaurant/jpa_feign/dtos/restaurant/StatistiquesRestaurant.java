////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Statistiques d'un restaurant.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class StatistiquesRestaurant implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du restaurant.
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Nombre total de commandes.
	 */
	@NotNull
	private Integer nombreCommandes = 0;

	/**
	 * Chiffre d'affaires total.
	 */
	@NotNull
	private BigDecimal chiffreAffaires = new BigDecimal(0);

	/**
	 * Nombre de clients uniques.
	 */
	@NotNull
	private Integer nombreClients = 0;

	/**
	 * Note moyenne des avis.
	 */
	private BigDecimal noteMoyenne;

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Accesseur de 'nombreCommandes'.
	 *
	 * @return Valeur de {@link #nombreCommandes nombreCommandes}.
	 */
	public Integer getNombreCommandes() {
		return this.nombreCommandes;
	}

	/**
	 * Accesseur de 'chiffreAffaires'.
	 *
	 * @return Valeur de {@link #chiffreAffaires chiffreAffaires}.
	 */
	public BigDecimal getChiffreAffaires() {
		return this.chiffreAffaires;
	}

	/**
	 * Accesseur de 'nombreClients'.
	 *
	 * @return Valeur de {@link #nombreClients nombreClients}.
	 */
	public Integer getNombreClients() {
		return this.nombreClients;
	}

	/**
	 * Accesseur de 'noteMoyenne'.
	 *
	 * @return Valeur de {@link #noteMoyenne noteMoyenne}.
	 */
	public BigDecimal getNoteMoyenne() {
		return this.noteMoyenne;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #nombreCommandes nombreCommandes}.
	 * @param nombreCommandes Valeur à définir.
	 */
	public void setNombreCommandes(Integer nombreCommandes) {
		this.nombreCommandes = nombreCommandes;
	}

	/**
	 * Définit la valeur de {@link #chiffreAffaires chiffreAffaires}.
	 * @param chiffreAffaires Valeur à définir.
	 */
	public void setChiffreAffaires(BigDecimal chiffreAffaires) {
		this.chiffreAffaires = chiffreAffaires;
	}

	/**
	 * Définit la valeur de {@link #nombreClients nombreClients}.
	 * @param nombreClients Valeur à définir.
	 */
	public void setNombreClients(Integer nombreClients) {
		this.nombreClients = nombreClients;
	}

	/**
	 * Définit la valeur de {@link #noteMoyenne noteMoyenne}.
	 * @param noteMoyenne Valeur à définir.
	 */
	public void setNoteMoyenne(BigDecimal noteMoyenne) {
		this.noteMoyenne = noteMoyenne;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.StatistiquesRestaurant StatistiquesRestaurant}.
	 */
	public enum Fields {
		RESTAURANT_ID(Integer.class),
		NOMBRE_COMMANDES(Integer.class),
		CHIFFRE_AFFAIRES(BigDecimal.class),
		NOMBRE_CLIENTS(Integer.class),
		NOTE_MOYENNE(BigDecimal.class);

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

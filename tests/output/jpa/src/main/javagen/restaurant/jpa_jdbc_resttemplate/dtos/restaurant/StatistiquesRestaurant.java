////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Statistiques d'un restaurant.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class StatistiquesRestaurant implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du restaurant.
	 */
	@NotNull
	@Column("restaurant_id")
	private Integer restaurantId;

	/**
	 * Nombre total de commandes.
	 */
	@NotNull
	@Column("nombre_commandes")
	private Integer nombreCommandes = 0;

	/**
	 * Chiffre d'affaires total.
	 */
	@NotNull
	@Column("chiffre_affaires")
	private BigDecimal chiffreAffaires = new BigDecimal(0);

	/**
	 * Nombre de clients uniques.
	 */
	@NotNull
	@Column("nombre_clients")
	private Integer nombreClients = 0;

	/**
	 * Note moyenne des avis.
	 */
	@Column("note_moyenne")
	private BigDecimal noteMoyenne;

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Getter for nombreCommandes.
	 *
	 * @return value of {@link #nombreCommandes nombreCommandes}.
	 */
	public Integer getNombreCommandes() {
		return this.nombreCommandes;
	}

	/**
	 * Getter for chiffreAffaires.
	 *
	 * @return value of {@link #chiffreAffaires chiffreAffaires}.
	 */
	public BigDecimal getChiffreAffaires() {
		return this.chiffreAffaires;
	}

	/**
	 * Getter for nombreClients.
	 *
	 * @return value of {@link #nombreClients nombreClients}.
	 */
	public Integer getNombreClients() {
		return this.nombreClients;
	}

	/**
	 * Getter for noteMoyenne.
	 *
	 * @return value of {@link #noteMoyenne noteMoyenne}.
	 */
	public BigDecimal getNoteMoyenne() {
		return this.noteMoyenne;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Set the value of {@link #nombreCommandes nombreCommandes}.
	 * @param nombreCommandes value to set.
	 */
	public void setNombreCommandes(Integer nombreCommandes) {
		this.nombreCommandes = nombreCommandes;
	}

	/**
	 * Set the value of {@link #chiffreAffaires chiffreAffaires}.
	 * @param chiffreAffaires value to set.
	 */
	public void setChiffreAffaires(BigDecimal chiffreAffaires) {
		this.chiffreAffaires = chiffreAffaires;
	}

	/**
	 * Set the value of {@link #nombreClients nombreClients}.
	 * @param nombreClients value to set.
	 */
	public void setNombreClients(Integer nombreClients) {
		this.nombreClients = nombreClients;
	}

	/**
	 * Set the value of {@link #noteMoyenne noteMoyenne}.
	 * @param noteMoyenne value to set.
	 */
	public void setNoteMoyenne(BigDecimal noteMoyenne) {
		this.noteMoyenne = noteMoyenne;
	}
}

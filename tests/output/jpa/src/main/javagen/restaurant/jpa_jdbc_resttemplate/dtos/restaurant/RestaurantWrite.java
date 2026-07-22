////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

/**
 * Détail d'un restaurant en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom du restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getNom() RestaurantRead#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("lie_nom")
	private String nom;

	/**
	 * Adresse du restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getAdresse() RestaurantRead#getAdresse()}
	 */
	@Size(max = 100)
	@Column("lie_adresse")
	private String adresse;

	/**
	 * Numéro de téléphone.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getTelephone() RestaurantRead#getTelephone()}
	 */
	@Size(max = 20)
	@Column("res_telephone")
	private String telephone;

	/**
	 * Association réciproque de Menu.Restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getMenus() RestaurantRead#getMenus()}
	 */
	@NotNull
	private List<Integer> menus;

	/**
	 * Association réciproque de Plat.Restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getPlats() RestaurantRead#getPlats()}
	 */
	@NotNull
	private List<Integer> plats;

	/**
	 * Association réciproque de Promotion.Restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getPromotions() RestaurantRead#getPromotions()}
	 */
	private List<Integer> promotions;

	/**
	 * Association réciproque de AvisClient.Restaurant.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getAvisClients() RestaurantRead#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Association réciproque de Table.RestaurantId.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.dtos.restaurant.RestaurantRead#getTables() RestaurantRead#getTables()}
	 */
	@Valid
	@NotNull
	private List<TableItem> tables;

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for adresse.
	 *
	 * @return value of {@link #adresse adresse}.
	 */
	public String getAdresse() {
		return this.adresse;
	}

	/**
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Getter for menus.
	 *
	 * @return value of {@link #menus menus}.
	 */
	public List<Integer> getMenus() {
		return this.menus;
	}

	/**
	 * Getter for plats.
	 *
	 * @return value of {@link #plats plats}.
	 */
	public List<Integer> getPlats() {
		return this.plats;
	}

	/**
	 * Getter for promotions.
	 *
	 * @return value of {@link #promotions promotions}.
	 */
	public List<Integer> getPromotions() {
		return this.promotions;
	}

	/**
	 * Getter for avisClients.
	 *
	 * @return value of {@link #avisClients avisClients}.
	 */
	public List<Integer> getAvisClients() {
		return this.avisClients;
	}

	/**
	 * Getter for tables.
	 *
	 * @return value of {@link #tables tables}.
	 */
	public List<TableItem> getTables() {
		return this.tables;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #adresse adresse}.
	 * @param adresse value to set.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Set the value of {@link #menus menus}.
	 * @param menus value to set.
	 */
	public void setMenus(List<Integer> menus) {
		this.menus = menus;
	}

	/**
	 * Set the value of {@link #plats plats}.
	 * @param plats value to set.
	 */
	public void setPlats(List<Integer> plats) {
		this.plats = plats;
	}

	/**
	 * Set the value of {@link #promotions promotions}.
	 * @param promotions value to set.
	 */
	public void setPromotions(List<Integer> promotions) {
		this.promotions = promotions;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Set the value of {@link #tables tables}.
	 * @param tables value to set.
	 */
	public void setTables(List<TableItem> tables) {
		this.tables = tables;
	}
}

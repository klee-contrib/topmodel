////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Restaurant avec ses statistiques.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantAvecStatistiques implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du restaurant.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getId() Restaurant#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du restaurant.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getNom() Restaurant#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Adresse du restaurant.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getAdresse() Restaurant#getAdresse()}
	 */
	@Size(max = 100)
	private String adresse;

	/**
	 * Numéro de téléphone.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getTelephone() Restaurant#getTelephone()}
	 */
	@Size(max = 20)
	private String telephone;

	/**
	 * Association réciproque de Menu.RestaurantId.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getMenus() Restaurant#getMenus()}
	 */
	@NotNull
	private List<Integer> menus;

	/**
	 * Association réciproque de Plat.RestaurantId.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getPlats() Restaurant#getPlats()}
	 */
	@NotNull
	private List<Integer> plats;

	/**
	 * Association réciproque de Promotion.RestaurantId.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getPromotions() Restaurant#getPromotions()}
	 */
	private List<Integer> promotions;

	/**
	 * Association réciproque de AvisClient.RestaurantId.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getAvisClients() Restaurant#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Association réciproque de TableRestaurant.RestaurantId.
	 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getTables() Restaurant#getTables()}
	 */
	@NotNull
	private List<Integer> tables;

	/**
	 * Nombre de plats du restaurant.
	 */
	@NotNull
	private Integer nombrePlats = 0;

	/**
	 * Nombre de tables du restaurant.
	 */
	@NotNull
	private Integer nombreTables = 0;

	/**
	 * Note moyenne des avis clients.
	 */
	private BigDecimal noteMoyenne;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	public List<Integer> getTables() {
		return this.tables;
	}

	/**
	 * Getter for nombrePlats.
	 *
	 * @return value of {@link #nombrePlats nombrePlats}.
	 */
	public Integer getNombrePlats() {
		return this.nombrePlats;
	}

	/**
	 * Getter for nombreTables.
	 *
	 * @return value of {@link #nombreTables nombreTables}.
	 */
	public Integer getNombreTables() {
		return this.nombreTables;
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
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	public void setTables(List<Integer> tables) {
		this.tables = tables;
	}

	/**
	 * Set the value of {@link #nombrePlats nombrePlats}.
	 * @param nombrePlats value to set.
	 */
	public void setNombrePlats(Integer nombrePlats) {
		this.nombrePlats = nombrePlats;
	}

	/**
	 * Set the value of {@link #nombreTables nombreTables}.
	 * @param nombreTables value to set.
	 */
	public void setNombreTables(Integer nombreTables) {
		this.nombreTables = nombreTables;
	}

	/**
	 * Set the value of {@link #noteMoyenne noteMoyenne}.
	 * @param noteMoyenne value to set.
	 */
	public void setNoteMoyenne(BigDecimal noteMoyenne) {
		this.noteMoyenne = noteMoyenne;
	}
}

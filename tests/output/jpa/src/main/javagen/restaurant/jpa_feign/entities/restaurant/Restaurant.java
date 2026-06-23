////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Transient;

/**
 * Restaurant.
 */
@Entity
@DiscriminatorValue("RESTAURANT")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Restaurant extends Lieu {

	/**
	 * Numéro de téléphone.
	 */
	@Column(name = "RES_TELEPHONE", length = 20, columnDefinition = "varchar")
	private String telephone;

	/**
	 * Association réciproque de Menu.Restaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Menu> menus;

	/**
	 * Association réciproque de Plat.Restaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Plat> plats;

	/**
	 * Association réciproque de Promotion.Restaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Promotion> promotions;

	/**
	 * Association réciproque de AvisClient.Restaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<AvisClient> avisClients;

	/**
	 * Association réciproque de TableRestaurant.RestaurantId.
	 */
	@Transient
	private List<Integer> tableIds;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "RES_DATE_CREATION", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

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
	public List<Menu> getMenus() {
		if (this.menus == null) {
			this.menus = new ArrayList<>();
		}
		return this.menus;
	}

	/**
	 * Getter for plats.
	 *
	 * @return value of {@link #plats plats}.
	 */
	public List<Plat> getPlats() {
		if (this.plats == null) {
			this.plats = new ArrayList<>();
		}
		return this.plats;
	}

	/**
	 * Getter for promotions.
	 *
	 * @return value of {@link #promotions promotions}.
	 */
	public List<Promotion> getPromotions() {
		if (this.promotions == null) {
			this.promotions = new ArrayList<>();
		}
		return this.promotions;
	}

	/**
	 * Getter for avisClients.
	 *
	 * @return value of {@link #avisClients avisClients}.
	 */
	public List<AvisClient> getAvisClients() {
		if (this.avisClients == null) {
			this.avisClients = new ArrayList<>();
		}
		return this.avisClients;
	}

	/**
	 * Getter for tableIds.
	 *
	 * @return value of {@link #tableIds tableIds}.
	 */
	public List<Integer> getTableIds() {
		if (this.tableIds == null) {
			this.tableIds = new ArrayList<>();
		}
		return this.tableIds;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
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
	public void setMenus(List<Menu> menus) {
		this.menus = menus;
	}

	/**
	 * Set the value of {@link #plats plats}.
	 * @param plats value to set.
	 */
	public void setPlats(List<Plat> plats) {
		this.plats = plats;
	}

	/**
	 * Set the value of {@link #promotions promotions}.
	 * @param promotions value to set.
	 */
	public void setPromotions(List<Promotion> promotions) {
		this.promotions = promotions;
	}

	/**
	 * Set the value of {@link #avisClients avisClients}.
	 * @param avisClients value to set.
	 */
	public void setAvisClients(List<AvisClient> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Set the value of {@link #tableIds tableIds}.
	 * @param tableIds value to set.
	 */
	public void setTableIds(List<Integer> tableIds) {
		this.tableIds = tableIds;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Restaurant#menus menus}.
	 * @param menu value to add to restaurant.
	 */
	void addMenu(Menu menu) {
		this.menus.add(menu);
		menu.setRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Restaurant#plats plats}.
	 * @param plat value to add to restaurant.
	 */
	void addPlat(Plat plat) {
		this.plats.add(plat);
		plat.setRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Restaurant#promotions promotions}.
	 * @param promotion value to add to restaurant.
	 */
	void addPromotion(Promotion promotion) {
		this.promotions.add(promotion);
		promotion.setRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_feign.entities.restaurant.Restaurant#avisClients avisClients}.
	 * @param avisClient value to add to restaurant.
	 */
	void addAvisClient(AvisClient avisClient) {
		this.avisClients.add(avisClient);
		avisClient.setRestaurant(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Restaurant#menus menus}.
	 * @param menu menu value to remove.
	 */
	void removeMenu(Menu menu) {
		this.menus.remove(menu);
		menu.setRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Restaurant#plats plats}.
	 * @param plat plat value to remove.
	 */
	void removePlat(Plat plat) {
		this.plats.remove(plat);
		plat.setRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Restaurant#promotions promotions}.
	 * @param promotion promotion value to remove.
	 */
	void removePromotion(Promotion promotion) {
		this.promotions.remove(promotion);
		promotion.setRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_feign.entities.restaurant.Restaurant#avisClients avisClients}.
	 * @param avisClient avisClient value to remove.
	 */
	void removeAvisClient(AvisClient avisClient) {
		this.avisClients.remove(avisClient);
		avisClient.setRestaurant(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Restaurant Restaurant}.
	 */
	public enum Fields {
		TELEPHONE(String.class),
		MENUS(List.class),
		PLATS(List.class),
		PROMOTIONS(List.class),
		AVIS_CLIENTS(List.class),
		TABLE_IDS(List.class),
		DATE_CREATION(LocalDateTime.class);

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

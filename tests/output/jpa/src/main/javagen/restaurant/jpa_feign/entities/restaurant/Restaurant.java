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
	@Column(name = "res_telephone", length = 20, columnDefinition = "varchar")
	private String telephone = "XX.XX.XX.XX.XX";

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
	 * Association réciproque de Table.RestaurantId.
	 */
	@Transient
	private List<Integer> tableIds;

	/**
	 * Date de création de l'enregistrement.
	 */
	@CreatedDate
	@Column(name = "res_date_creation", nullable = false, columnDefinition = "timestamp")
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'telephone'.
	 *
	 * @return Valeur de {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Accesseur de 'menus'.
	 *
	 * @return Valeur de {@link #menus menus}.
	 */
	public List<Menu> getMenus() {
		if (this.menus == null) {
			this.menus = new ArrayList<>();
		}
		return this.menus;
	}

	/**
	 * Accesseur de 'plats'.
	 *
	 * @return Valeur de {@link #plats plats}.
	 */
	public List<Plat> getPlats() {
		if (this.plats == null) {
			this.plats = new ArrayList<>();
		}
		return this.plats;
	}

	/**
	 * Accesseur de 'promotions'.
	 *
	 * @return Valeur de {@link #promotions promotions}.
	 */
	public List<Promotion> getPromotions() {
		if (this.promotions == null) {
			this.promotions = new ArrayList<>();
		}
		return this.promotions;
	}

	/**
	 * Accesseur de 'avisClients'.
	 *
	 * @return Valeur de {@link #avisClients avisClients}.
	 */
	public List<AvisClient> getAvisClients() {
		if (this.avisClients == null) {
			this.avisClients = new ArrayList<>();
		}
		return this.avisClients;
	}

	/**
	 * Accesseur de 'tableIds'.
	 *
	 * @return Valeur de {@link #tableIds tableIds}.
	 */
	public List<Integer> getTableIds() {
		if (this.tableIds == null) {
			this.tableIds = new ArrayList<>();
		}
		return this.tableIds;
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
	 * Définit la valeur de {@link #telephone telephone}.
	 * @param telephone Valeur à définir.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Définit la valeur de {@link #menus menus}.
	 * @param menus Valeur à définir.
	 */
	public void setMenus(List<Menu> menus) {
		this.menus = menus;
	}

	/**
	 * Définit la valeur de {@link #plats plats}.
	 * @param plats Valeur à définir.
	 */
	public void setPlats(List<Plat> plats) {
		this.plats = plats;
	}

	/**
	 * Définit la valeur de {@link #promotions promotions}.
	 * @param promotions Valeur à définir.
	 */
	public void setPromotions(List<Promotion> promotions) {
		this.promotions = promotions;
	}

	/**
	 * Définit la valeur de {@link #avisClients avisClients}.
	 * @param avisClients Valeur à définir.
	 */
	public void setAvisClients(List<AvisClient> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Définit la valeur de {@link #tableIds tableIds}.
	 * @param tableIds Valeur à définir.
	 */
	public void setTableIds(List<Integer> tableIds) {
		this.tableIds = tableIds;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Restaurant#menus menus}.
	 * @param menu Valeur à ajouter à 'restaurant'.
	 */
	void addMenu(Menu menu) {
		this.menus.add(menu);
		menu.setRestaurant(this);
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Restaurant#plats plats}.
	 * @param plat Valeur à ajouter à 'restaurant'.
	 */
	void addPlat(Plat plat) {
		this.plats.add(plat);
		plat.setRestaurant(this);
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Restaurant#promotions promotions}.
	 * @param promotion Valeur à ajouter à 'restaurant'.
	 */
	void addPromotion(Promotion promotion) {
		this.promotions.add(promotion);
		promotion.setRestaurant(this);
	}

	/**
	 * Ajoute une valeur à {@link restaurant.jpa_feign.entities.restaurant.Restaurant#avisClients avisClients}.
	 * @param avisClient Valeur à ajouter à 'restaurant'.
	 */
	void addAvisClient(AvisClient avisClient) {
		this.avisClients.add(avisClient);
		avisClient.setRestaurant(this);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#menus menus}.
	 * @param menu Valeur de 'menu' à supprimer.
	 */
	void removeMenu(Menu menu) {
		this.menus.remove(menu);
		menu.setRestaurant(null);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#plats plats}.
	 * @param plat Valeur de 'plat' à supprimer.
	 */
	void removePlat(Plat plat) {
		this.plats.remove(plat);
		plat.setRestaurant(null);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#promotions promotions}.
	 * @param promotion Valeur de 'promotion' à supprimer.
	 */
	void removePromotion(Promotion promotion) {
		this.promotions.remove(promotion);
		promotion.setRestaurant(null);
	}

	/**
	 * Supprime une valeur de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#avisClients avisClients}.
	 * @param avisClient Valeur de 'avisClient' à supprimer.
	 */
	void removeAvisClient(AvisClient avisClient) {
		this.avisClients.remove(avisClient);
		avisClient.setRestaurant(null);
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.entities.restaurant.Restaurant Restaurant}.
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
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.SequenceGenerator;
import jakarta.persistence.Table;

/**
 * Restaurant.
 */
@Entity
@Table(name = "RESTAURANT")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Restaurant {

	/**
	 * Identifiant du restaurant.
	 */
	@Id
	@Column(name = "RES_ID", nullable = false, columnDefinition = "int")
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_RESTAURANT")
	@SequenceGenerator(sequenceName = "SEQ_RESTAURANT", name = "SEQ_RESTAURANT", initialValue = 1000, allocationSize = 50)
	private Integer id;

	/**
	 * Nom du restaurant.
	 */
	@Column(name = "RES_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Adresse du restaurant.
	 */
	@Column(name = "RES_ADRESSE", length = 100, columnDefinition = "varchar")
	private String adresse;

	/**
	 * Numéro de téléphone.
	 */
	@Column(name = "RES_TELEPHONE", length = 20, columnDefinition = "varchar")
	private String telephone;

	/**
	 * Association réciproque de Menu.RestaurantId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Menu> menus;

	/**
	 * Association réciproque de Plat.RestaurantId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Plat> plats;

	/**
	 * Association réciproque de Promotion.RestaurantId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<Promotion> promotions;

	/**
	 * Association réciproque de AvisClient.RestaurantId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<AvisClient> avisClients;

	/**
	 * Association réciproque de TableRestaurant.RestaurantId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurant")
	private List<TableRestaurant> tables;

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
	 * Getter for tables.
	 *
	 * @return value of {@link #tables tables}.
	 */
	public List<TableRestaurant> getTables() {
		if (this.tables == null) {
			this.tables = new ArrayList<>();
		}
		return this.tables;
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
	 * Set the value of {@link #tables tables}.
	 * @param tables value to set.
	 */
	public void setTables(List<TableRestaurant> tables) {
		this.tables = tables;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant Restaurant}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		ADRESSE(String.class),
		TELEPHONE(String.class),
		MENUS(List.class),
		PLATS(List.class),
		PROMOTIONS(List.class),
		AVIS_CLIENTS(List.class),
		TABLES(List.class);

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

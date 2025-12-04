////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.entities.restaurant;

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
	 * Association réciproque de TableClient.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<TableClient> tableClientsRestaurant;

	/**
	 * Association réciproque de Plat.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<Plat> platsRestaurant;

	/**
	 * Association réciproque de AvisClient.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<AvisClient> avisClientsRestaurant;

	/**
	 * Association réciproque de Menu.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<Menu> menusRestaurant;

	/**
	 * Association réciproque de Reservation.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<Reservation> reservationsRestaurant;

	/**
	 * Association réciproque de Promotion.RestaurantIdRestaurant.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "restaurantRestaurant")
	private List<Promotion> promotionsRestaurant;

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
	 * Getter for tableClientsRestaurant.
	 *
	 * @return value of {@link #tableClientsRestaurant tableClientsRestaurant}.
	 */
	public List<TableClient> getTableClientsRestaurant() {
		if (this.tableClientsRestaurant == null) {
			this.tableClientsRestaurant = new ArrayList<>();
		}
		return this.tableClientsRestaurant;
	}

	/**
	 * Getter for platsRestaurant.
	 *
	 * @return value of {@link #platsRestaurant platsRestaurant}.
	 */
	public List<Plat> getPlatsRestaurant() {
		if (this.platsRestaurant == null) {
			this.platsRestaurant = new ArrayList<>();
		}
		return this.platsRestaurant;
	}

	/**
	 * Getter for avisClientsRestaurant.
	 *
	 * @return value of {@link #avisClientsRestaurant avisClientsRestaurant}.
	 */
	public List<AvisClient> getAvisClientsRestaurant() {
		if (this.avisClientsRestaurant == null) {
			this.avisClientsRestaurant = new ArrayList<>();
		}
		return this.avisClientsRestaurant;
	}

	/**
	 * Getter for menusRestaurant.
	 *
	 * @return value of {@link #menusRestaurant menusRestaurant}.
	 */
	public List<Menu> getMenusRestaurant() {
		if (this.menusRestaurant == null) {
			this.menusRestaurant = new ArrayList<>();
		}
		return this.menusRestaurant;
	}

	/**
	 * Getter for reservationsRestaurant.
	 *
	 * @return value of {@link #reservationsRestaurant reservationsRestaurant}.
	 */
	public List<Reservation> getReservationsRestaurant() {
		if (this.reservationsRestaurant == null) {
			this.reservationsRestaurant = new ArrayList<>();
		}
		return this.reservationsRestaurant;
	}

	/**
	 * Getter for promotionsRestaurant.
	 *
	 * @return value of {@link #promotionsRestaurant promotionsRestaurant}.
	 */
	public List<Promotion> getPromotionsRestaurant() {
		if (this.promotionsRestaurant == null) {
			this.promotionsRestaurant = new ArrayList<>();
		}
		return this.promotionsRestaurant;
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
	 * Set the value of {@link #tableClientsRestaurant tableClientsRestaurant}.
	 * @param tableClientsRestaurant value to set.
	 */
	public void setTableClientsRestaurant(List<TableClient> tableClientsRestaurant) {
		this.tableClientsRestaurant = tableClientsRestaurant;
	}

	/**
	 * Set the value of {@link #platsRestaurant platsRestaurant}.
	 * @param platsRestaurant value to set.
	 */
	public void setPlatsRestaurant(List<Plat> platsRestaurant) {
		this.platsRestaurant = platsRestaurant;
	}

	/**
	 * Set the value of {@link #avisClientsRestaurant avisClientsRestaurant}.
	 * @param avisClientsRestaurant value to set.
	 */
	public void setAvisClientsRestaurant(List<AvisClient> avisClientsRestaurant) {
		this.avisClientsRestaurant = avisClientsRestaurant;
	}

	/**
	 * Set the value of {@link #menusRestaurant menusRestaurant}.
	 * @param menusRestaurant value to set.
	 */
	public void setMenusRestaurant(List<Menu> menusRestaurant) {
		this.menusRestaurant = menusRestaurant;
	}

	/**
	 * Set the value of {@link #reservationsRestaurant reservationsRestaurant}.
	 * @param reservationsRestaurant value to set.
	 */
	public void setReservationsRestaurant(List<Reservation> reservationsRestaurant) {
		this.reservationsRestaurant = reservationsRestaurant;
	}

	/**
	 * Set the value of {@link #promotionsRestaurant promotionsRestaurant}.
	 * @param promotionsRestaurant value to set.
	 */
	public void setPromotionsRestaurant(List<Promotion> promotionsRestaurant) {
		this.promotionsRestaurant = promotionsRestaurant;
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#tableClientsRestaurant tableClientsRestaurant}.
	 * @param tableClient value to add to restaurantRestaurant.
	 */
	void addTableClientRestaurant(TableClient tableClient) {
		this.tableClientsRestaurant.add(tableClient);
		tableClient.setRestaurantRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#platsRestaurant platsRestaurant}.
	 * @param plat value to add to restaurantRestaurant.
	 */
	void addPlatRestaurant(Plat plat) {
		this.platsRestaurant.add(plat);
		plat.setRestaurantRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#avisClientsRestaurant avisClientsRestaurant}.
	 * @param avisClient value to add to restaurantRestaurant.
	 */
	void addAvisClientRestaurant(AvisClient avisClient) {
		this.avisClientsRestaurant.add(avisClient);
		avisClient.setRestaurantRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#menusRestaurant menusRestaurant}.
	 * @param menu value to add to restaurantRestaurant.
	 */
	void addMenuRestaurant(Menu menu) {
		this.menusRestaurant.add(menu);
		menu.setRestaurantRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#reservationsRestaurant reservationsRestaurant}.
	 * @param reservation value to add to restaurantRestaurant.
	 */
	void addReservationRestaurant(Reservation reservation) {
		this.reservationsRestaurant.add(reservation);
		reservation.setRestaurantRestaurant(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#promotionsRestaurant promotionsRestaurant}.
	 * @param promotion value to add to restaurantRestaurant.
	 */
	void addPromotionRestaurant(Promotion promotion) {
		this.promotionsRestaurant.add(promotion);
		promotion.setRestaurantRestaurant(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#tableClientsRestaurant tableClientsRestaurant}.
	 * @param tableClient tableClient value to remove.
	 */
	void removeTableClientRestaurant(TableClient tableClient) {
		this.tableClientsRestaurant.remove(tableClient);
		tableClient.setRestaurantRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#platsRestaurant platsRestaurant}.
	 * @param plat plat value to remove.
	 */
	void removePlatRestaurant(Plat plat) {
		this.platsRestaurant.remove(plat);
		plat.setRestaurantRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#avisClientsRestaurant avisClientsRestaurant}.
	 * @param avisClient avisClient value to remove.
	 */
	void removeAvisClientRestaurant(AvisClient avisClient) {
		this.avisClientsRestaurant.remove(avisClient);
		avisClient.setRestaurantRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#menusRestaurant menusRestaurant}.
	 * @param menu menu value to remove.
	 */
	void removeMenuRestaurant(Menu menu) {
		this.menusRestaurant.remove(menu);
		menu.setRestaurantRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#reservationsRestaurant reservationsRestaurant}.
	 * @param reservation reservation value to remove.
	 */
	void removeReservationRestaurant(Reservation reservation) {
		this.reservationsRestaurant.remove(reservation);
		reservation.setRestaurantRestaurant(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant#promotionsRestaurant promotionsRestaurant}.
	 * @param promotion promotion value to remove.
	 */
	void removePromotionRestaurant(Promotion promotion) {
		this.promotionsRestaurant.remove(promotion);
		promotion.setRestaurantRestaurant(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_metamodel.entities.restaurant.Restaurant Restaurant}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		ADRESSE(String.class),
		TELEPHONE(String.class),
		TABLE_CLIENTS_RESTAURANT(List.class),
		PLATS_RESTAURANT(List.class),
		AVIS_CLIENTS_RESTAURANT(List.class),
		MENUS_RESTAURANT(List.class),
		RESERVATIONS_RESTAURANT(List.class),
		PROMOTIONS_RESTAURANT(List.class);

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

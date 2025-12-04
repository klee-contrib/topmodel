////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

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
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getId() Restaurant#getId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer id;

	/**
	 * Nom du restaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getNom() Restaurant#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("res_nom")
	private String nom;

	/**
	 * Adresse du restaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getAdresse() Restaurant#getAdresse()}
	 */
	@Size(max = 100)
	@Column("res_adresse")
	private String adresse;

	/**
	 * Numéro de téléphone.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getTelephone() Restaurant#getTelephone()}
	 */
	@Size(max = 20)
	@Column("res_telephone")
	private String telephone;

	/**
	 * Association réciproque de TableClient.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getTableClientsRestaurant() Restaurant#getTableClientsRestaurant()}
	 */
	@NotNull
	@Column("tab_id_restaurant")
	private List<Integer> tableClientsRestaurant;

	/**
	 * Association réciproque de Plat.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getPlatsRestaurant() Restaurant#getPlatsRestaurant()}
	 */
	@NotNull
	@Column("pla_id_restaurant")
	private List<Integer> platsRestaurant;

	/**
	 * Association réciproque de AvisClient.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getAvisClientsRestaurant() Restaurant#getAvisClientsRestaurant()}
	 */
	@NotNull
	@Column("avi_id_restaurant")
	private List<Integer> avisClientsRestaurant;

	/**
	 * Association réciproque de Menu.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getMenusRestaurant() Restaurant#getMenusRestaurant()}
	 */
	@NotNull
	@Column("men_id_restaurant")
	private List<Integer> menusRestaurant;

	/**
	 * Association réciproque de Reservation.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getReservationsRestaurant() Restaurant#getReservationsRestaurant()}
	 */
	@NotNull
	@Column("rev_id_restaurant")
	private List<Integer> reservationsRestaurant;

	/**
	 * Association réciproque de Promotion.RestaurantIdRestaurant.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Restaurant#getPromotionsRestaurant() Restaurant#getPromotionsRestaurant()}
	 */
	@Column("pro_id_restaurant")
	private List<Integer> promotionsRestaurant;

	/**
	 * Nombre de plats du restaurant.
	 */
	@NotNull
	@Column("nombre_plats")
	private Integer nombrePlats = 0;

	/**
	 * Nombre de tables du restaurant.
	 */
	@NotNull
	@Column("nombre_tables")
	private Integer nombreTables = 0;

	/**
	 * Note moyenne des avis clients.
	 */
	@Column("note_moyenne")
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
	 * Getter for tableClientsRestaurant.
	 *
	 * @return value of {@link #tableClientsRestaurant tableClientsRestaurant}.
	 */
	public List<Integer> getTableClientsRestaurant() {
		return this.tableClientsRestaurant;
	}

	/**
	 * Getter for platsRestaurant.
	 *
	 * @return value of {@link #platsRestaurant platsRestaurant}.
	 */
	public List<Integer> getPlatsRestaurant() {
		return this.platsRestaurant;
	}

	/**
	 * Getter for avisClientsRestaurant.
	 *
	 * @return value of {@link #avisClientsRestaurant avisClientsRestaurant}.
	 */
	public List<Integer> getAvisClientsRestaurant() {
		return this.avisClientsRestaurant;
	}

	/**
	 * Getter for menusRestaurant.
	 *
	 * @return value of {@link #menusRestaurant menusRestaurant}.
	 */
	public List<Integer> getMenusRestaurant() {
		return this.menusRestaurant;
	}

	/**
	 * Getter for reservationsRestaurant.
	 *
	 * @return value of {@link #reservationsRestaurant reservationsRestaurant}.
	 */
	public List<Integer> getReservationsRestaurant() {
		return this.reservationsRestaurant;
	}

	/**
	 * Getter for promotionsRestaurant.
	 *
	 * @return value of {@link #promotionsRestaurant promotionsRestaurant}.
	 */
	public List<Integer> getPromotionsRestaurant() {
		return this.promotionsRestaurant;
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
	 * Set the value of {@link #tableClientsRestaurant tableClientsRestaurant}.
	 * @param tableClientsRestaurant value to set.
	 */
	public void setTableClientsRestaurant(List<Integer> tableClientsRestaurant) {
		this.tableClientsRestaurant = tableClientsRestaurant;
	}

	/**
	 * Set the value of {@link #platsRestaurant platsRestaurant}.
	 * @param platsRestaurant value to set.
	 */
	public void setPlatsRestaurant(List<Integer> platsRestaurant) {
		this.platsRestaurant = platsRestaurant;
	}

	/**
	 * Set the value of {@link #avisClientsRestaurant avisClientsRestaurant}.
	 * @param avisClientsRestaurant value to set.
	 */
	public void setAvisClientsRestaurant(List<Integer> avisClientsRestaurant) {
		this.avisClientsRestaurant = avisClientsRestaurant;
	}

	/**
	 * Set the value of {@link #menusRestaurant menusRestaurant}.
	 * @param menusRestaurant value to set.
	 */
	public void setMenusRestaurant(List<Integer> menusRestaurant) {
		this.menusRestaurant = menusRestaurant;
	}

	/**
	 * Set the value of {@link #reservationsRestaurant reservationsRestaurant}.
	 * @param reservationsRestaurant value to set.
	 */
	public void setReservationsRestaurant(List<Integer> reservationsRestaurant) {
		this.reservationsRestaurant = reservationsRestaurant;
	}

	/**
	 * Set the value of {@link #promotionsRestaurant promotionsRestaurant}.
	 * @param promotionsRestaurant value to set.
	 */
	public void setPromotionsRestaurant(List<Integer> promotionsRestaurant) {
		this.promotionsRestaurant = promotionsRestaurant;
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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantAvecStatistiques RestaurantAvecStatistiques}.
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
		PROMOTIONS_RESTAURANT(List.class),
		NOMBRE_PLATS(Integer.class),
		NOMBRE_TABLES(Integer.class),
		NOTE_MOYENNE(BigDecimal.class);

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

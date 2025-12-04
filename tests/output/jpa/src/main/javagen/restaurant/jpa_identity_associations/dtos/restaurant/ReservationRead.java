////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_associations.entities.restaurant.Reservation;
import restaurant.jpa_identity_associations.entities.restaurant.RestaurantMappers;

/**
 * Détail d'une réservation en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ReservationRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la réservation.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getId() Reservation#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getDateReservation() Reservation#getDateReservation()}
	 */
	@NotNull
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getNombrePersonnes() Reservation#getNombrePersonnes()}
	 */
	@NotNull
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getCommentaire() Reservation#getCommentaire()}
	 */
	@Size(max = 100)
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getConfirmee() Reservation#getConfirmee()}
	 */
	@NotNull
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getClientClient() Reservation#getClientClient()}
	 */
	@NotNull
	private Integer clientIdClient;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getTableClientTable() Reservation#getTableClientTable()}
	 */
	private Integer tableClientIdTable;

	/**
	 * Restaurant concerné par la réservation.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Reservation#getRestaurantRestaurant() Reservation#getRestaurantRestaurant()}
	 */
	@NotNull
	private Integer restaurantIdRestaurant;

	/**
	 * No arg constructor.
	 */
	public ReservationRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'ReservationRead'.
	 * @param reservation Instance de 'Reservation'.
	 *
	 * @return Une nouvelle instance de 'ReservationRead'.
	 */
	public ReservationRead(Reservation reservation) {
		RestaurantMappers.mapReservationRead(reservation, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for dateReservation.
	 *
	 * @return value of {@link #dateReservation dateReservation}.
	 */
	public LocalDateTime getDateReservation() {
		return this.dateReservation;
	}

	/**
	 * Getter for nombrePersonnes.
	 *
	 * @return value of {@link #nombrePersonnes nombrePersonnes}.
	 */
	public Integer getNombrePersonnes() {
		return this.nombrePersonnes;
	}

	/**
	 * Getter for commentaire.
	 *
	 * @return value of {@link #commentaire commentaire}.
	 */
	public String getCommentaire() {
		return this.commentaire;
	}

	/**
	 * Getter for confirmee.
	 *
	 * @return value of {@link #confirmee confirmee}.
	 */
	public Boolean getConfirmee() {
		return this.confirmee;
	}

	/**
	 * Getter for clientIdClient.
	 *
	 * @return value of {@link #clientIdClient clientIdClient}.
	 */
	public Integer getClientIdClient() {
		return this.clientIdClient;
	}

	/**
	 * Getter for tableClientIdTable.
	 *
	 * @return value of {@link #tableClientIdTable tableClientIdTable}.
	 */
	public Integer getTableClientIdTable() {
		return this.tableClientIdTable;
	}

	/**
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #dateReservation dateReservation}.
	 * @param dateReservation value to set.
	 */
	public void setDateReservation(LocalDateTime dateReservation) {
		this.dateReservation = dateReservation;
	}

	/**
	 * Set the value of {@link #nombrePersonnes nombrePersonnes}.
	 * @param nombrePersonnes value to set.
	 */
	public void setNombrePersonnes(Integer nombrePersonnes) {
		this.nombrePersonnes = nombrePersonnes;
	}

	/**
	 * Set the value of {@link #commentaire commentaire}.
	 * @param commentaire value to set.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Set the value of {@link #confirmee confirmee}.
	 * @param confirmee value to set.
	 */
	public void setConfirmee(Boolean confirmee) {
		this.confirmee = confirmee;
	}

	/**
	 * Set the value of {@link #clientIdClient clientIdClient}.
	 * @param clientIdClient value to set.
	 */
	public void setClientIdClient(Integer clientIdClient) {
		this.clientIdClient = clientIdClient;
	}

	/**
	 * Set the value of {@link #tableClientIdTable tableClientIdTable}.
	 * @param tableClientIdTable value to set.
	 */
	public void setTableClientIdTable(Integer tableClientIdTable) {
		this.tableClientIdTable = tableClientIdTable;
	}

	/**
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.dtos.restaurant.ReservationRead ReservationRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_RESERVATION(LocalDateTime.class),
		NOMBRE_PERSONNES(Integer.class),
		COMMENTAIRE(String.class),
		CONFIRMEE(Boolean.class),
		CLIENT_ID_CLIENT(Integer.class),
		TABLE_CLIENT_ID_TABLE(Integer.class),
		RESTAURANT_ID_RESTAURANT(Integer.class);

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

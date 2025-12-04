////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Réservation avec détails du client et de la table.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ReservationAvecDetails implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getId() Reservation#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getDateReservation() Reservation#getDateReservation()}
	 */
	@NotNull
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getNombrePersonnes() Reservation#getNombrePersonnes()}
	 */
	@NotNull
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getCommentaire() Reservation#getCommentaire()}
	 */
	@Size(max = 100)
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getConfirmee() Reservation#getConfirmee()}
	 */
	@NotNull
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getClientClient() Reservation#getClientClient()}
	 */
	@NotNull
	private Integer clientIdClient;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getTableClientTable() Reservation#getTableClientTable()}
	 */
	private Integer tableClientIdTable;

	/**
	 * Restaurant concerné par la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getRestaurantRestaurant() Reservation#getRestaurantRestaurant()}
	 */
	@NotNull
	private Integer restaurantIdRestaurant;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getNom() Client#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String clientNom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getPrenom() Client#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String clientPrenom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getTelephone() Client#getTelephone()}
	 */
	@Size(max = 20)
	private String clientTelephone;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getNumero() TableClient#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String tableClientNumero;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableClient#getCapacite() TableClient#getCapacite()}
	 */
	@NotNull
	private Integer tableClientCapacite;

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
	 * Getter for clientNom.
	 *
	 * @return value of {@link #clientNom clientNom}.
	 */
	public String getClientNom() {
		return this.clientNom;
	}

	/**
	 * Getter for clientPrenom.
	 *
	 * @return value of {@link #clientPrenom clientPrenom}.
	 */
	public String getClientPrenom() {
		return this.clientPrenom;
	}

	/**
	 * Getter for clientTelephone.
	 *
	 * @return value of {@link #clientTelephone clientTelephone}.
	 */
	public String getClientTelephone() {
		return this.clientTelephone;
	}

	/**
	 * Getter for tableClientNumero.
	 *
	 * @return value of {@link #tableClientNumero tableClientNumero}.
	 */
	public String getTableClientNumero() {
		return this.tableClientNumero;
	}

	/**
	 * Getter for tableClientCapacite.
	 *
	 * @return value of {@link #tableClientCapacite tableClientCapacite}.
	 */
	public Integer getTableClientCapacite() {
		return this.tableClientCapacite;
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
	 * Set the value of {@link #clientNom clientNom}.
	 * @param clientNom value to set.
	 */
	public void setClientNom(String clientNom) {
		this.clientNom = clientNom;
	}

	/**
	 * Set the value of {@link #clientPrenom clientPrenom}.
	 * @param clientPrenom value to set.
	 */
	public void setClientPrenom(String clientPrenom) {
		this.clientPrenom = clientPrenom;
	}

	/**
	 * Set the value of {@link #clientTelephone clientTelephone}.
	 * @param clientTelephone value to set.
	 */
	public void setClientTelephone(String clientTelephone) {
		this.clientTelephone = clientTelephone;
	}

	/**
	 * Set the value of {@link #tableClientNumero tableClientNumero}.
	 * @param tableClientNumero value to set.
	 */
	public void setTableClientNumero(String tableClientNumero) {
		this.tableClientNumero = tableClientNumero;
	}

	/**
	 * Set the value of {@link #tableClientCapacite tableClientCapacite}.
	 * @param tableClientCapacite value to set.
	 */
	public void setTableClientCapacite(Integer tableClientCapacite) {
		this.tableClientCapacite = tableClientCapacite;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.ReservationAvecDetails ReservationAvecDetails}.
	 */
	public enum Fields {
		ID(Integer.class),
		DATE_RESERVATION(LocalDateTime.class),
		NOMBRE_PERSONNES(Integer.class),
		COMMENTAIRE(String.class),
		CONFIRMEE(Boolean.class),
		CLIENT_ID_CLIENT(Integer.class),
		TABLE_CLIENT_ID_TABLE(Integer.class),
		RESTAURANT_ID_RESTAURANT(Integer.class),
		CLIENT_NOM(String.class),
		CLIENT_PRENOM(String.class),
		CLIENT_TELEPHONE(String.class),
		TABLE_CLIENT_NUMERO(String.class),
		TABLE_CLIENT_CAPACITE(Integer.class);

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

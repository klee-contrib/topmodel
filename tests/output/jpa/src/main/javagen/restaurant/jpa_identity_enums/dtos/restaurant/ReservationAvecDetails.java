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
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getClient() Reservation#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getTable() Reservation#getTable()}
	 */
	private Integer tableId;

	/**
	 * Restaurant concerné par la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Reservation#getRestaurant() Reservation#getRestaurant()}
	 */
	@NotNull
	private Integer restaurantId;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String clientNom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String clientPrenom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	private String clientEmail;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableRestaurant#getNumero() TableRestaurant#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	private String tableNumero;

	/**
	 * Table réservée.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.TableRestaurant#getCapacite() TableRestaurant#getCapacite()}
	 */
	@NotNull
	private Integer tableCapacite;

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
	 * Getter for clientId.
	 *
	 * @return value of {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Getter for tableId.
	 *
	 * @return value of {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
	}

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Getter for clientEmail.
	 *
	 * @return value of {@link #clientEmail clientEmail}.
	 */
	public String getClientEmail() {
		return this.clientEmail;
	}

	/**
	 * Getter for tableNumero.
	 *
	 * @return value of {@link #tableNumero tableNumero}.
	 */
	public String getTableNumero() {
		return this.tableNumero;
	}

	/**
	 * Getter for tableCapacite.
	 *
	 * @return value of {@link #tableCapacite tableCapacite}.
	 */
	public Integer getTableCapacite() {
		return this.tableCapacite;
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
	 * Set the value of {@link #clientId clientId}.
	 * @param clientId value to set.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Set the value of {@link #tableId tableId}.
	 * @param tableId value to set.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
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
	 * Set the value of {@link #clientEmail clientEmail}.
	 * @param clientEmail value to set.
	 */
	public void setClientEmail(String clientEmail) {
		this.clientEmail = clientEmail;
	}

	/**
	 * Set the value of {@link #tableNumero tableNumero}.
	 * @param tableNumero value to set.
	 */
	public void setTableNumero(String tableNumero) {
		this.tableNumero = tableNumero;
	}

	/**
	 * Set the value of {@link #tableCapacite tableCapacite}.
	 * @param tableCapacite value to set.
	 */
	public void setTableCapacite(Integer tableCapacite) {
		this.tableCapacite = tableCapacite;
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
		CLIENT_ID(Integer.class),
		TABLE_ID(Integer.class),
		RESTAURANT_ID(Integer.class),
		CLIENT_NOM(String.class),
		CLIENT_PRENOM(String.class),
		CLIENT_EMAIL(String.class),
		TABLE_NUMERO(String.class),
		TABLE_CAPACITE(Integer.class);

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

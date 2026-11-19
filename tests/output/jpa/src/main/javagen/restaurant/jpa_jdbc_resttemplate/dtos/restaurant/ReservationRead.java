////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une réservation en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ReservationRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getId() Reservation#getId()}
	 */
	@NotNull
	@Column("rev_id")
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getDateReservation() Reservation#getDateReservation()}
	 */
	@NotNull
	@Column("rev_date_reservation")
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getNombrePersonnes() Reservation#getNombrePersonnes()}
	 */
	@NotNull
	@Column("rev_nombre_personnes")
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getCommentaire() Reservation#getCommentaire()}
	 */
	@Size(max = 100)
	@Column("rev_commentaire")
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getConfirmee() Reservation#getConfirmee()}
	 */
	@NotNull
	@Column("rev_confirmee")
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getClient() Reservation#getClient()}
	 */
	@NotNull
	@Column("per_id")
	private Integer clientId;

	/**
	 * Table réservée.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getTableId() Reservation#getTableId()}
	 */
	@Column("tab_id")
	private Integer tableId;

	/**
	 * Restaurant concerné par la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getRestaurant() Reservation#getRestaurant()}
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurantId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Reservation#getDateCreation() Reservation#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	@Column("rev_date_creation")
	private LocalDateTime dateCreation;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_nom")
	private String clientNom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_prenom")
	private String clientPrenom;

	/**
	 * Informations du client ayant fait la réservation.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Client#getEmail() Client#getEmail()}
	 */
	@Size(max = 100)
	@Column("cli_email")
	private String clientEmail;

	/**
	 * Table réservée.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Table#getNumero() Table#getNumero()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("tab_numero")
	private String tableNumero;

	/**
	 * Table réservée.
	 * Alias de {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Table#getCapacite() Table#getCapacite()}
	 */
	@NotNull
	@Column("tab_capacite")
	private Integer tableCapacite;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'dateReservation'.
	 *
	 * @return Valeur de {@link #dateReservation dateReservation}.
	 */
	public LocalDateTime getDateReservation() {
		return this.dateReservation;
	}

	/**
	 * Accesseur de 'nombrePersonnes'.
	 *
	 * @return Valeur de {@link #nombrePersonnes nombrePersonnes}.
	 */
	public Integer getNombrePersonnes() {
		return this.nombrePersonnes;
	}

	/**
	 * Accesseur de 'commentaire'.
	 *
	 * @return Valeur de {@link #commentaire commentaire}.
	 */
	public String getCommentaire() {
		return this.commentaire;
	}

	/**
	 * Accesseur de 'confirmee'.
	 *
	 * @return Valeur de {@link #confirmee confirmee}.
	 */
	public Boolean getConfirmee() {
		return this.confirmee;
	}

	/**
	 * Accesseur de 'clientId'.
	 *
	 * @return Valeur de {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Accesseur de 'tableId'.
	 *
	 * @return Valeur de {@link #tableId tableId}.
	 */
	public Integer getTableId() {
		return this.tableId;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Accesseur de 'clientNom'.
	 *
	 * @return Valeur de {@link #clientNom clientNom}.
	 */
	public String getClientNom() {
		return this.clientNom;
	}

	/**
	 * Accesseur de 'clientPrenom'.
	 *
	 * @return Valeur de {@link #clientPrenom clientPrenom}.
	 */
	public String getClientPrenom() {
		return this.clientPrenom;
	}

	/**
	 * Accesseur de 'clientEmail'.
	 *
	 * @return Valeur de {@link #clientEmail clientEmail}.
	 */
	public String getClientEmail() {
		return this.clientEmail;
	}

	/**
	 * Accesseur de 'tableNumero'.
	 *
	 * @return Valeur de {@link #tableNumero tableNumero}.
	 */
	public String getTableNumero() {
		return this.tableNumero;
	}

	/**
	 * Accesseur de 'tableCapacite'.
	 *
	 * @return Valeur de {@link #tableCapacite tableCapacite}.
	 */
	public Integer getTableCapacite() {
		return this.tableCapacite;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #dateReservation dateReservation}.
	 * @param dateReservation Valeur à définir.
	 */
	public void setDateReservation(LocalDateTime dateReservation) {
		this.dateReservation = dateReservation;
	}

	/**
	 * Définit la valeur de {@link #nombrePersonnes nombrePersonnes}.
	 * @param nombrePersonnes Valeur à définir.
	 */
	public void setNombrePersonnes(Integer nombrePersonnes) {
		this.nombrePersonnes = nombrePersonnes;
	}

	/**
	 * Définit la valeur de {@link #commentaire commentaire}.
	 * @param commentaire Valeur à définir.
	 */
	public void setCommentaire(String commentaire) {
		this.commentaire = commentaire;
	}

	/**
	 * Définit la valeur de {@link #confirmee confirmee}.
	 * @param confirmee Valeur à définir.
	 */
	public void setConfirmee(Boolean confirmee) {
		this.confirmee = confirmee;
	}

	/**
	 * Définit la valeur de {@link #clientId clientId}.
	 * @param clientId Valeur à définir.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #clientNom clientNom}.
	 * @param clientNom Valeur à définir.
	 */
	public void setClientNom(String clientNom) {
		this.clientNom = clientNom;
	}

	/**
	 * Définit la valeur de {@link #clientPrenom clientPrenom}.
	 * @param clientPrenom Valeur à définir.
	 */
	public void setClientPrenom(String clientPrenom) {
		this.clientPrenom = clientPrenom;
	}

	/**
	 * Définit la valeur de {@link #clientEmail clientEmail}.
	 * @param clientEmail Valeur à définir.
	 */
	public void setClientEmail(String clientEmail) {
		this.clientEmail = clientEmail;
	}

	/**
	 * Définit la valeur de {@link #tableNumero tableNumero}.
	 * @param tableNumero Valeur à définir.
	 */
	public void setTableNumero(String tableNumero) {
		this.tableNumero = tableNumero;
	}

	/**
	 * Définit la valeur de {@link #tableCapacite tableCapacite}.
	 * @param tableCapacite Valeur à définir.
	 */
	public void setTableCapacite(Integer tableCapacite) {
		this.tableCapacite = tableCapacite;
	}
}

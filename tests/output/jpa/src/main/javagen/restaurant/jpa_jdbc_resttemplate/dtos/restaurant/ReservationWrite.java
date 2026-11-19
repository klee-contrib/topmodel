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
import jakarta.validation.constraints.Size;

/**
 * Détail d'une réservation en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ReservationWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

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
}

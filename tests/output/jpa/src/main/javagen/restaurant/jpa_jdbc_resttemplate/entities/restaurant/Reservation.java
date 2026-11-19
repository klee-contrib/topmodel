////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.ReadOnlyProperty;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityListeners;
import jakarta.validation.constraints.NotNull;

/**
 * Réservation d'une table.
 */
@Table(name = "reservation")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Reservation {

	/**
	 * Identifiant de la réservation.
	 */
	@Id
	@ReadOnlyProperty
	@Column("rev_id")
	private Integer id;

	/**
	 * Date et heure de la réservation.
	 */
	@NotNull
	@Column("rev_date_reservation")
	private LocalDateTime dateReservation;

	/**
	 * Nombre de personnes.
	 */
	@NotNull
	@Column("rev_nombre_personnes")
	private Integer nombrePersonnes;

	/**
	 * Commentaire sur la réservation.
	 */
	@Column("rev_commentaire")
	private String commentaire;

	/**
	 * Indique si la réservation est confirmée.
	 */
	@NotNull
	@Column("rev_confirmee")
	private Boolean confirmee = false;

	/**
	 * Client ayant fait la réservation.
	 */
	@NotNull
	@Column("per_id")
	private Integer client;

	/**
	 * Table réservée.
	 */
	@Column("tab_id")
	private Integer tableId;

	/**
	 * Restaurant concerné par la réservation.
	 */
	@NotNull
	@Column("lie_id")
	private Integer restaurant;

	/**
	 * Date de création de l'enregistrement.
	 */
	@NotNull
	@CreatedDate
	@Column("rev_date_creation")
	private LocalDateTime dateCreation;

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
	 * Accesseur de 'client'.
	 *
	 * @return Valeur de {@link #client client}.
	 */
	public Integer getClient() {
		return this.client;
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
	 * Accesseur de 'restaurant'.
	 *
	 * @return Valeur de {@link #restaurant restaurant}.
	 */
	public Integer getRestaurant() {
		return this.restaurant;
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
	 * Définit la valeur de {@link #client client}.
	 * @param client Valeur à définir.
	 */
	public void setClient(Integer client) {
		this.client = client;
	}

	/**
	 * Définit la valeur de {@link #tableId tableId}.
	 * @param tableId Valeur à définir.
	 */
	public void setTableId(Integer tableId) {
		this.tableId = tableId;
	}

	/**
	 * Définit la valeur de {@link #restaurant restaurant}.
	 * @param restaurant Valeur à définir.
	 */
	public void setRestaurant(Integer restaurant) {
		this.restaurant = restaurant;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}

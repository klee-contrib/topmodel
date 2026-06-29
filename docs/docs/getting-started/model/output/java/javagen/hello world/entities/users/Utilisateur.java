////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.entities.users;

import java.time.LocalDate;

import hello world.enums.refs.TypeUtilisateurCode;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * Utilisateur de l'application.
 */
@Entity
@Table(name = "UTILISATEUR")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Utilisateur {

	/**
	 * Identifiant unique de l'utilisateur.
	 */
	@Id
	@Column(name = "UTI_ID", nullable = false, columnDefinition = "int8")
	private long id;

	/**
	 * Adresse mail de l'utilisateur.
	 */
	@Column(name = "UTI_EMAIL", nullable = false, length = 50, columnDefinition = "varchar")
	private String email;

	/**
	 * Nom de l'utilisateur.
	 */
	@Column(name = "UTI_NOM", length = 15, columnDefinition = "varchar")
	private String nom;

	/**
	 * Date d'inscription.
	 */
	@Column(name = "UTI_DATE_INSCRIPTION", columnDefinition = "timestamp")
	private LocalDate dateInscription;

	/**
	 * Type de l'utilisateur.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "TUT_CODE", length = 3, columnDefinition = "varchar")
	private TypeUtilisateurCode typeUtilisateurCode;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
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
	 * Getter for dateInscription.
	 *
	 * @return value of {@link #dateInscription dateInscription}.
	 */
	public LocalDate getDateInscription() {
		return this.dateInscription;
	}

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link #typeUtilisateurCode typeUtilisateurCode}.
	 */
	public TypeUtilisateurCode getTypeUtilisateurCode() {
		return this.typeUtilisateurCode;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #dateInscription dateInscription}.
	 * @param dateInscription value to set.
	 */
	public void setDateInscription(LocalDate dateInscription) {
		this.dateInscription = dateInscription;
	}

	/**
	 * Set the value of {@link #typeUtilisateurCode typeUtilisateurCode}.
	 * @param typeUtilisateurCode value to set.
	 */
	public void setTypeUtilisateurCode(TypeUtilisateurCode typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
	}
}

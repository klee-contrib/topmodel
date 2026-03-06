////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.dtos.users;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDate;

import hello world.entities.users.UsersMappers;
import hello world.entities.users.Utilisateur;
import hello world.enums.refs.TypeUtilisateurCode;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Objet de transfert pour la classe Utilisateur dans le cas d'une création.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurCreateDto implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Adresse mail de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getEmail() Utilisateur#getEmail()}
	 */
	@NotNull
	@Size(max = 50)
	private String utilisateurEmail;

	/**
	 * Nom de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getNom() Utilisateur#getNom()}
	 */
	@Size(max = 15)
	private String utilisateurNom;

	/**
	 * Date d'inscription.
	 * Alias of {@link hello world.entities.users.Utilisateur#getDateInscription() Utilisateur#getDateInscription()}
	 */
	private LocalDate utilisateurDateInscription;

	/**
	 * Type de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()}
	 */
	private TypeUtilisateurCode utilisateurTypeUtilisateurCode;

	/**
	 * Getter for utilisateurEmail.
	 *
	 * @return value of {@link #utilisateurEmail utilisateurEmail}.
	 */
	public String getUtilisateurEmail() {
		return this.utilisateurEmail;
	}

	/**
	 * Getter for utilisateurNom.
	 *
	 * @return value of {@link #utilisateurNom utilisateurNom}.
	 */
	public String getUtilisateurNom() {
		return this.utilisateurNom;
	}

	/**
	 * Getter for utilisateurDateInscription.
	 *
	 * @return value of {@link #utilisateurDateInscription utilisateurDateInscription}.
	 */
	public LocalDate getUtilisateurDateInscription() {
		return this.utilisateurDateInscription;
	}

	/**
	 * Getter for utilisateurTypeUtilisateurCode.
	 *
	 * @return value of {@link #utilisateurTypeUtilisateurCode utilisateurTypeUtilisateurCode}.
	 */
	public TypeUtilisateurCode getUtilisateurTypeUtilisateurCode() {
		return this.utilisateurTypeUtilisateurCode;
	}

	/**
	 * Set the value of {@link #utilisateurEmail utilisateurEmail}.
	 * @param utilisateurEmail value to set.
	 */
	public void setUtilisateurEmail(String utilisateurEmail) {
		this.utilisateurEmail = utilisateurEmail;
	}

	/**
	 * Set the value of {@link #utilisateurNom utilisateurNom}.
	 * @param utilisateurNom value to set.
	 */
	public void setUtilisateurNom(String utilisateurNom) {
		this.utilisateurNom = utilisateurNom;
	}

	/**
	 * Set the value of {@link #utilisateurDateInscription utilisateurDateInscription}.
	 * @param utilisateurDateInscription value to set.
	 */
	public void setUtilisateurDateInscription(LocalDate utilisateurDateInscription) {
		this.utilisateurDateInscription = utilisateurDateInscription;
	}

	/**
	 * Set the value of {@link #utilisateurTypeUtilisateurCode utilisateurTypeUtilisateurCode}.
	 * @param utilisateurTypeUtilisateurCode value to set.
	 */
	public void setUtilisateurTypeUtilisateurCode(TypeUtilisateurCode utilisateurTypeUtilisateurCode) {
		this.utilisateurTypeUtilisateurCode = utilisateurTypeUtilisateurCode;
	}

	/**
	 * Mappe 'UtilisateurCreateDto' vers 'Utilisateur'.
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Utilisateur'.
	 */
	public Utilisateur toUtilisateur(Utilisateur target) {
		return UsersMappers.toUtilisateur(this, target);
	}
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.dtos.users;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDate;

import hello world.entities.refs.TypeUtilisateur;
import hello world.entities.users.UsersMappers;
import hello world.entities.users.Utilisateur;
import hello world.enums.refs.TypeUtilisateurCode;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Objet de transfert pour la classe Utilisateur, dans le cas d'une recherche.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurSearchResultDto implements Serializable {

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
	private String email;

	/**
	 * Nom de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getNom() Utilisateur#getNom()}
	 */
	@Size(max = 15)
	private String nom;

	/**
	 * Date d'inscription.
	 * Alias of {@link hello world.entities.users.Utilisateur#getDateInscription() Utilisateur#getDateInscription()}
	 */
	private LocalDate dateInscription;

	/**
	 * Type de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()}
	 */
	private TypeUtilisateurCode typeUtilisateurCode;

	/**
	 * Libellé du type d'utilisateur.
	 * Alias of {@link hello world.entities.refs.TypeUtilisateur#getLibelle() TypeUtilisateur#getLibelle()}
	 */
	@NotNull
	@Size(max = 15)
	private String libelleTypeUtilisateur;

	/**
	 * No arg constructor.
	 */
	public UtilisateurSearchResultDto() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'UtilisateurSearchResultDto'.
	 * @param utilisateur Instance de 'Utilisateur'.
	 * @param typeUtilisateur Instance de 'TypeUtilisateur'.
	 *
	 * @return Une nouvelle instance de 'UtilisateurSearchResultDto'.
	 */
	public UtilisateurSearchResultDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur) {
		UsersMappers.mapUtilisateurSearchResultDto(utilisateur, typeUtilisateur, this);
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
	 * Getter for libelleTypeUtilisateur.
	 *
	 * @return value of {@link #libelleTypeUtilisateur libelleTypeUtilisateur}.
	 */
	public String getLibelleTypeUtilisateur() {
		return this.libelleTypeUtilisateur;
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

	/**
	 * Set the value of {@link #libelleTypeUtilisateur libelleTypeUtilisateur}.
	 * @param libelleTypeUtilisateur value to set.
	 */
	public void setLibelleTypeUtilisateur(String libelleTypeUtilisateur) {
		this.libelleTypeUtilisateur = libelleTypeUtilisateur;
	}
}

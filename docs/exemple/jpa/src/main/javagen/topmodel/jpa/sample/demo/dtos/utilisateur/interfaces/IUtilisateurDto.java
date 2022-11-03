////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur.interfaces;

import java.time.LocalDate;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurDto {

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateCreation dateCreation}.
	 */
	LocalDate getDateCreation();

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateModification dateModification}.
	 */
	LocalDateTime getDateModification();

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#id id}.
	 */
	Long getId();

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#age age}.
	 */
	Long getAge();

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 */
	Long getProfilId();

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#email email}.
	 */
	String getEmail();

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 */
	TypeUtilisateur.Values getTypeUtilisateurCode();

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	UtilisateurDto getUtilisateurParent();
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateur {

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#id id}.
	 */
	Long getId();

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#age age}.
	 */
	Long getAge();

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#profilId profilId}.
	 */
	Long getProfilId();

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#email email}.
	 */
	String getEmail();

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#nom nom}.
	 */
	String getNom();

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#typeUtilisateurCode typeUtilisateurCode}.
	 */
	TypeUtilisateur.Values getTypeUtilisateurCode();

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#dateCreation dateCreation}.
	 */
	LocalDate getDateCreation();

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur#dateModification dateModification}.
	 */
	LocalDateTime getDateModification();

	/**
	 * hydrate values of instance.
	 * @param id value to set
	 * @param age value to set
	 * @param profilId value to set
	 * @param email value to set
	 * @param nom value to set
	 * @param typeUtilisateurCode value to set
	 * @param dateCreation value to set
	 * @param dateModification value to set
	 */
	void hydrate(Long id, Long age, Long profilId, String email, String nom, TypeUtilisateur.Values typeUtilisateurCode, LocalDate dateCreation, LocalDateTime dateModification);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.enums.utilisateur.TypeUtilisateurCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurSearch {

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#id id}.
	 */
	Integer getId();

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#age age}.
	 */
	BigDecimal getAge();

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#profilId profilId}.
	 */
	Integer getProfilId();

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#email email}.
	 */
	String getEmail();

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#nom nom}.
	 */
	String getNom();

	/**
	 * Getter for actif.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#actif actif}.
	 */
	Boolean getActif();

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#typeUtilisateurCode typeUtilisateurCode}.
	 */
	TypeUtilisateurCode getTypeUtilisateurCode();

	/**
	 * Getter for utilisateursEnfant.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#utilisateursEnfant utilisateursEnfant}.
	 */
	List<Integer> getUtilisateursEnfant();

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#dateCreation dateCreation}.
	 */
	LocalDate getDateCreation();

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurSearch#dateModification dateModification}.
	 */
	LocalDateTime getDateModification();
}

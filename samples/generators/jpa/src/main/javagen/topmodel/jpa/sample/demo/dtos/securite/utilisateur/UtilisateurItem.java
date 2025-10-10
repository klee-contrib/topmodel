////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.utilisateur;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurItem {

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem#id id}.
	 */
	Integer getId();

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem#nom nom}.
	 */
	String getNom();

	/**
	 * Getter for prenom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem#prenom prenom}.
	 */
	String getPrenom();

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem#email email}.
	 */
	String getEmail();

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem#typeUtilisateurCode typeUtilisateurCode}.
	 */
	TypeUtilisateurCode getTypeUtilisateurCode();

	/**
	 * hydrate values of instance.
	 * @param id value to set
	 * @param nom value to set
	 * @param prenom value to set
	 * @param email value to set
	 * @param typeUtilisateurCode value to set
	 */
	void hydrate(Integer id, String nom, String prenom, String email, TypeUtilisateurCode typeUtilisateurCode);
}

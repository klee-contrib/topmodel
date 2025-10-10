////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.utilisateur;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

/**
 * Détail d'un utilisateur en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurItem {

	/**
	 * Getter for id.
	 */
	Integer getId();

	/**
	 * Getter for nom.
	 */
	String getNom();

	/**
	 * Getter for prenom.
	 */
	String getPrenom();

	/**
	 * Getter for email.
	 */
	String getEmail();

	/**
	 * Getter for typeUtilisateurCode.
	 */
	TypeUtilisateurCode getTypeUtilisateurCode();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param nom value to set.
	 * @param prenom value to set.
	 * @param email value to set.
	 * @param typeUtilisateurCode value to set.
	 */
	void hydrate(Integer id, String nom, String prenom, String email, TypeUtilisateurCode typeUtilisateurCode);
}

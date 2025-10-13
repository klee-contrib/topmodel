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
	 * Id de l'utilisateur.
	 */
	Integer getId();

	/**
	 * Nom de l'utilisateur.
	 */
	String getNom();

	/**
	 * Nom de l'utilisateur.
	 */
	String getPrenom();

	/**
	 * Email de l'utilisateur.
	 */
	String getEmail();

	/**
	 * Type d'utilisateur.
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

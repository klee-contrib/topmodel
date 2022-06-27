////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur.interfaces;

import javax.annotation.Generated;
import javax.validation.constraints.Email;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurDto {

	/**
	 * Getter for utilisateurId.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurId utilisateurId}.
	 */
	long getUtilisateurId();

	/**
	 * Getter for utilisateuremail.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateuremail utilisateuremail}.
	 */
	String getUtilisateuremail();

	/**
	 * Getter for utilisateurTypeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurTypeUtilisateurCode utilisateurTypeUtilisateurCode}.
	 */
	TypeUtilisateur.Values getUtilisateurTypeUtilisateurCode();

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	UtilisateurDto getUtilisateurParent();
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur.interfaces;

import javax.annotation.Generated;
import javax.validation.constraints.Email;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurDto {

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#id id}.
	 */
	long getId();

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#email email}.
	 */
	String getEmail();

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 */
	TypeUtilisateur.Values getTypeUtilisateurCode();

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 */
	long getProfilId();

	/**
	 * Getter for profilTypeProfilCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilTypeProfilCode profilTypeProfilCode}.
	 */
	TypeProfil.Values getProfilTypeProfilCode();

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	UtilisateurDto getUtilisateurParent();
}

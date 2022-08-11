////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.controller.securite.utilisateur;

import java.util.List;
import java.util.stream.Collectors;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.Valid;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurController {


	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	@GetMapping(path = "utilisateur/{utilisateurId}")
	default UtilisateurDto getUtilisateurMapping(@RequestParam(value = "utiId", required = true) long utiId) {
		return this.getUtilisateur(utiId);
	}

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	UtilisateurDto getUtilisateur(long utiId);

	/**
	 * Charge une liste d'utilisateurs par leur type.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Liste des utilisateurs
	 */
	@GetMapping(path = "utilisateur/list")
	default List<UtilisateurDto> getUtilisateurListMapping(@RequestParam(value = "typeUtilisateurCode", required = false) TypeUtilisateur.Values typeUtilisateurCode) {
		return this.getUtilisateurList(typeUtilisateurCode);
	}

	/**
	 * Charge une liste d'utilisateurs par leur type.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Liste des utilisateurs
	 */
	List<UtilisateurDto> getUtilisateurList(TypeUtilisateur.Values typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PostMapping(path = "utilisateur/save")
	default UtilisateurDto saveUtilisateurMapping(@RequestBody @Valid UtilisateurDto utilisateur) {
		return this.saveUtilisateur(utilisateur);
	}

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	UtilisateurDto saveUtilisateur(UtilisateurDto utilisateur);

	/**
	 * Sauvegarde une liste d'utilisateurs.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PostMapping(path = "utilisateur/saveAll")
	default List<UtilisateurDto> saveAllUtilisateurMapping(@RequestBody @Valid List<UtilisateurDto> utilisateur) {
		return this.saveAllUtilisateur(utilisateur);
	}

	/**
	 * Sauvegarde une liste d'utilisateurs.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	List<UtilisateurDto> saveAllUtilisateur(List<UtilisateurDto> utilisateur);

	/**
	 * Recherche des utilisateurs.
	 * @param utiUtilisateurId Id technique
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Utilisateurs matchant les critères
	 */
	@PostMapping(path = "utilisateur/search")
	default Page<UtilisateurDto> searchMapping(@RequestParam(value = "utiUtilisateurId", required = true) long utiUtilisateurId, @RequestParam(value = "utilisateuremail", required = false) String utilisateuremail, @RequestParam(value = "utilisateurTypeUtilisateurCode", required = false) TypeUtilisateur.Values utilisateurTypeUtilisateurCode) {
		return this.search(utiUtilisateurId, utilisateuremail, utilisateurTypeUtilisateurCode);
	}

	/**
	 * Recherche des utilisateurs.
	 * @param utiUtilisateurId Id technique
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Utilisateurs matchant les critères
	 */
	Page<UtilisateurDto> search(long utiUtilisateurId, String utilisateuremail, TypeUtilisateur.Values utilisateurTypeUtilisateurCode);
}

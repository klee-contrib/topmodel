////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.controller.securite.utilisateur;

import java.util.List;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.Valid;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

@RestController
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurController {


	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	@GetMapping(path = "utilisateur/{utilisateurId}")
	default UtilisateurDto getUtilisateurMapping(@RequestParam("utiId") long utiId) {
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
	default List<UtilisateurDto> getUtilisateurListMapping(@RequestParam("typeUtilisateurCode") TypeUtilisateur.Values typeUtilisateurCode) {
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
	 * @param utiId Id technique
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param typeUtilisateurCodeOrigin Type d'utilisateur en one to one
	 * @param proProfilId Id technique
	 * @param profilTypeProfilCode Type de profil
	 * @return Utilisateurs matchant les critères
	 */
	@PostMapping(path = "utilisateur/search")
	default Page<UtilisateurDto> searchMapping(@RequestParam("utiId") long utiId, @RequestParam("email") String email, @RequestParam("typeUtilisateurCode") TypeUtilisateur.Values typeUtilisateurCode, @RequestParam("typeUtilisateurCodeOrigin") TypeUtilisateur.Values typeUtilisateurCodeOrigin, @RequestParam("proProfilId") long proProfilId, @RequestParam("profilTypeProfilCode") TypeProfil.Values profilTypeProfilCode) {
		return this.search(utiId, email, typeUtilisateurCode, typeUtilisateurCodeOrigin, proProfilId, profilTypeProfilCode);
	}

	/**
	 * Recherche des utilisateurs.
	 * @param utiId Id technique
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param typeUtilisateurCodeOrigin Type d'utilisateur en one to one
	 * @param proProfilId Id technique
	 * @param profilTypeProfilCode Type de profil
	 * @return Utilisateurs matchant les critères
	 */
	Page<UtilisateurDto> search(long utiId, String email, TypeUtilisateur.Values typeUtilisateurCode, TypeUtilisateur.Values typeUtilisateurCodeOrigin, long proProfilId, TypeProfil.Values profilTypeProfilCode);
}

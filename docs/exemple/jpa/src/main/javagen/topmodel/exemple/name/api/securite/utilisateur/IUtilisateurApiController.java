////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.securite.utilisateur;

import java.util.List;
import java.util.stream.Collectors;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.Valid;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import topmodel.exemple.name.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.entities.utilisateur.TypeUtilisateur;

@RequestMapping("utilisateur")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurApiController {


	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id technique
	 * @return Le détail de l'utilisateur
	 */
	@GetMapping(path = "/{utiId}")
	UtilisateurDto getUtilisateur(@PathVariable("utiId") long utiId);

	/**
	 * Charge une liste d'utilisateurs par leur type.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Liste des utilisateurs
	 */
	@GetMapping(path = "/list")
	List<UtilisateurDto> getUtilisateurList(@RequestParam(value = "typeUtilisateurCode", required = false) TypeUtilisateur.Values typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PostMapping(path = "/save")
	UtilisateurDto saveUtilisateur(@RequestBody @Valid UtilisateurDto utilisateur);

	/**
	 * Sauvegarde une liste d'utilisateurs.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PostMapping(path = "/saveAll")
	List<UtilisateurDto> saveAllUtilisateur(@RequestBody @Valid List<UtilisateurDto> utilisateur);

	/**
	 * Recherche des utilisateurs.
	 * @param utiUtilisateurId Id technique
	 * @param utilisateurAge Age en années de l'utilisateur
	 * @param utilisateurProfilId Profil de l'utilisateur
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Utilisateurs matchant les critères
	 */
	@PostMapping(path = "/search")
	Page<UtilisateurDto> search(@RequestParam(value = "utiUtilisateurId", required = true) long utiUtilisateurId, @RequestParam(value = "utilisateurAge", required = false) Long utilisateurAge, @RequestParam(value = "utilisateurProfilId", required = false) long utilisateurProfilId, @RequestParam(value = "utilisateuremail", required = false) String utilisateuremail, @RequestParam(value = "utilisateurTypeUtilisateurCode", required = false) TypeUtilisateur.Values utilisateurTypeUtilisateurCode);
}

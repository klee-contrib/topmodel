////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.server.securite.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PreAuthorize;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;

@RequestMapping("utilisateur")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurApiController {


	/**
	 * Charge le détail d'un utilisateur.
	 * @param utilisateurId Id technique
	 * @return Le détail de l'utilisateur
	 */
	@GetMapping(path = "/{utiId}")
	UtilisateurDto find(@RequestParam(value = "utilisateurId", required = true) Long utilisateurId);

	/**
	 * Charge une liste d'utilisateurs par leur type.
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @return Liste des utilisateurs
	 */
	@GetMapping(path = "/list")
	List<UtilisateurDto> findAllByType(@RequestParam(value = "typeUtilisateurCode", required = false) TypeUtilisateur.Values typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PreAuthorize("hasRole('ROLE_ADMIN')")
	@PostMapping(path = "/save")
	UtilisateurDto save(@RequestBody @Valid UtilisateurDto utilisateur);

	/**
	 * Recherche des utilisateurs.
	 * @param utilisateurId Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @return Utilisateurs matchant les critères
	 */
	@PostMapping(path = "/search")
	Page<UtilisateurDto> search(@RequestParam(value = "utilisateurId", required = true) Long utilisateurId, @RequestParam(value = "age", required = false) Long age, @RequestParam(value = "profilId", required = false) Long profilId, @RequestParam(value = "email", required = false) String email, @RequestParam(value = "typeUtilisateurCode", required = false) TypeUtilisateur.Values typeUtilisateurCode, @RequestParam(value = "dateCreation", required = false) LocalDate dateCreation, @RequestParam(value = "dateModification", required = false) LocalDateTime dateModification);
}

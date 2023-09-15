////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.server.securite.utilisateur;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurRead;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurWrite;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

@RequestMapping("utilisateur")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurApiController {


	/**
	 * Ajoute un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PreAuthorize("hasRole('CREATE')")
	@PostMapping(path = "/")
	UtilisateurRead addUtilisateur(@RequestBody @Valid UtilisateurWrite utilisateur);

	/**
	 * Supprime un utilisateur.
	 * @param utiId Id de l'utilisateur
	 */
	@PreAuthorize("hasRole('DELETE')")
	@DeleteMapping(path = "/{utiId}")
	void deleteUtilisateur(@PathVariable("utiId") Integer utiId);

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id de l'utilisateur
	 * @return Le détail de l'utilisateur
	 */
	@PreAuthorize("hasRole('READ')")
	@GetMapping(path = "/{utiId}")
	UtilisateurRead getUtilisateur(@PathVariable("utiId") Integer utiId);

	/**
	 * Recherche des utilisateurs.
	 * @param nom Nom de l'utilisateur
	 * @param prenom Nom de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param dateNaissance Age de l'utilisateur
	 * @param actif Si l'utilisateur est actif
	 * @param profilId Profil de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur
	 * @return Utilisateurs matchant les critères
	 */
	@PreAuthorize("hasRole('READ')")
	@GetMapping(path = "/")
	List<UtilisateurItem> searchUtilisateur(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "prenom", required = true) String prenom, @RequestParam(value = "email", required = true) String email, @RequestParam(value = "dateNaissance", required = false) LocalDateTime dateNaissance, @RequestParam(value = "actif", required = true) Boolean actif, @RequestParam(value = "profilId", required = true) Integer profilId, @RequestParam(value = "typeUtilisateurCode", required = true) TypeUtilisateurCode typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utiId Id de l'utilisateur
	 * @param utilisateur Utilisateur à sauvegarder
	 * @return Utilisateur sauvegardé
	 */
	@PreAuthorize("hasRole('UPDATE')")
	@PutMapping(path = "/{utiId}")
	UtilisateurRead updateUtilisateur(@PathVariable("utiId") Integer utiId, @RequestBody @Valid UtilisateurWrite utilisateur);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.server.securite.utilisateur;

import java.time.LocalDate;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurRead;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurWrite;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

@RequestMapping("api/utilisateurs")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface UtilisateurController {

	/**
	 * Ajoute un utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder.
	 *
	 * @return Utilisateur sauvegardé.
	 */
	@PostMapping(path = "")
	@PreAuthorize("hasRole('CREATE')")
	UtilisateurRead addUtilisateur(@RequestBody @Valid UtilisateurWrite utilisateur);

	/**
	 * Supprime un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 */
	@DeleteMapping(path = "{utiId}")
	@PreAuthorize("hasRole('DELETE')")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	void deleteUtilisateur(@PathVariable("utiId") Integer utiId);

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 *
	 * @return Le détail de l'utilisateur.
	 */
	@GetMapping(path = "{utiId}")
	@PreAuthorize("hasRole('READ')")
	UtilisateurRead getUtilisateur(@PathVariable("utiId") Integer utiId);

	/**
	 * Recherche des utilisateurs.
	 * @param nom Nom de l'utilisateur.
	 * @param prenom Nom de l'utilisateur.
	 * @param email Email de l'utilisateur.
	 * @param dateNaissance Age de l'utilisateur.
	 * @param adresse Adresse de l'utilisateur.
	 * @param actif Si l'utilisateur est actif.
	 * @param profilId Profil de l'utilisateur.
	 * @param typeUtilisateurCode Type d'utilisateur.
	 *
	 * @return Utilisateurs matchant les critères.
	 */
	@GetMapping(path = "")
	@PreAuthorize("hasRole('READ')")
	List<UtilisateurItem> searchUtilisateur(@RequestParam(value = "nom", required = false) String nom, @RequestParam(value = "prenom", required = false) String prenom, @RequestParam(value = "email", required = false) String email, @RequestParam(value = "dateNaissance", required = false) LocalDate dateNaissance, @RequestParam(value = "adresse", required = false) String adresse, @RequestParam(value = "actif", required = false) Boolean actif, @RequestParam(value = "profilId", required = false) Integer profilId, @RequestParam(value = "typeUtilisateurCode", required = false) TypeUtilisateurCode typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder.
	 *
	 * @return Utilisateur sauvegardé.
	 */
	@PutMapping(path = "{utiId}")
	@PreAuthorize("hasRole('UPDATE')")
	UtilisateurRead updateUtilisateur(@PathVariable("utiId") Integer utiId, @RequestBody @Valid UtilisateurWrite utilisateur);
}

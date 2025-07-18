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

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

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
	@Operation(description = "Ajoute un utilisateur")
	UtilisateurRead addUtilisateur(@RequestBody @Valid @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Utilisateur à sauvegarder") UtilisateurWrite utilisateur);

	/**
	 * Supprime un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 */
	@DeleteMapping(path = "{utiId}")
	@PreAuthorize("hasRole('DELETE')")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un utilisateur")
	void deleteUtilisateur(@PathVariable("utiId") @Parameter(description = "Id de l'utilisateur") Integer utiId);

	/**
	 * Charge le détail d'un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 *
	 * @return Le détail de l'utilisateur.
	 */
	@GetMapping(path = "{utiId}")
	@PreAuthorize("hasRole('READ')")
	@Operation(description = "Charge le détail d'un utilisateur")
	UtilisateurRead getUtilisateur(@PathVariable("utiId") @Parameter(description = "Id de l'utilisateur") Integer utiId);

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
	@Operation(description = "Recherche des utilisateurs")
	List<UtilisateurItem> searchUtilisateur(@RequestParam(value = "nom", required = false) @Parameter(description = "Nom de l'utilisateur") String nom, @RequestParam(value = "prenom", required = false) @Parameter(description = "Nom de l'utilisateur") String prenom, @RequestParam(value = "email", required = false) @Parameter(description = "Email de l'utilisateur") String email, @RequestParam(value = "dateNaissance", required = false) @Parameter(description = "Age de l'utilisateur") LocalDate dateNaissance, @RequestParam(value = "adresse", required = false) @Parameter(description = "Adresse de l'utilisateur") String adresse, @RequestParam(value = "actif", required = false) @Parameter(description = "Si l'utilisateur est actif") Boolean actif, @RequestParam(value = "profilId", required = false) @Parameter(description = "Profil de l'utilisateur") Integer profilId, @RequestParam(value = "typeUtilisateurCode", required = false) @Parameter(description = "Type d'utilisateur") TypeUtilisateurCode typeUtilisateurCode);

	/**
	 * Sauvegarde un utilisateur.
	 * @param utiId Id de l'utilisateur.
	 * @param utilisateur Utilisateur à sauvegarder.
	 *
	 * @return Utilisateur sauvegardé.
	 */
	@PutMapping(path = "{utiId}")
	@PreAuthorize("hasRole('UPDATE')")
	@Operation(description = "Sauvegarde un utilisateur")
	UtilisateurRead updateUtilisateur(@PathVariable("utiId") @Parameter(description = "Id de l'utilisateur") Integer utiId, @RequestBody @Valid @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Utilisateur à sauvegarder") UtilisateurWrite utilisateur);
}

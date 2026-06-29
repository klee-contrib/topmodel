////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.api.users;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseStatus;

import hello world.dtos.users.UtilisateurCreateDto;
import hello world.dtos.users.UtilisateurDetailDto;
import hello world.dtos.users.UtilisateurUpdateDto;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EndpointsController {

	/**
	 * Créé un nouvel Utilisateur.
	 * @param detail Le détail de l'utilisateur à créer.
	 *
	 * @return Le détail de l'utilisateur créé.
	 */
	@PostMapping(path = "Utilisateur")
	UtilisateurDetailDto createUtilisateur(@RequestBody @Valid UtilisateurCreateDto detail);

	/**
	 * Supprime un Utilisateur.
	 * @param utiId Identifiant unique de l'utilisateur.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "Utilisateur/{utiId}")
	void deleteUtilisateur(@PathVariable("utiId") long utiId);

	/**
	 * Charge le détail d'un Utilisateur.
	 * @param utiId Identifiant unique de l'utilisateur.
	 *
	 * @return Le détail d'un Utilisateur.
	 */
	@GetMapping(path = "Utilisateur/{utiId}")
	UtilisateurDetailDto getUtilisateur(@PathVariable("utiId") long utiId);

	/**
	 * Modifie un Utilisateur.
	 * @param detail Le détail de l'utilisateur à modifier.
	 * @param utiId Identifiant unique de l'utilisateur.
	 *
	 * @return Le détail de l'utilisateur modifié.
	 */
	@PatchMapping(path = "Utilisateur/{utiId}")
	UtilisateurDetailDto updateUtilisateur(@RequestBody @Valid UtilisateurUpdateDto detail, @PathVariable("utiId") long utiId);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.server.securite.profil;

import java.util.List;

import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;

@RequestMapping("api/profils")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ProfilController {

	/**
	 * Ajoute un Profil.
	 * @param profil Profil à sauvegarder.
	 *
	 * @return Profil sauvegardé.
	 */
	@PostMapping(path = "")
	@PreAuthorize("hasRole('CREATE')")
	@Operation(description = "Ajoute un Profil")
	ProfilRead addProfil(@RequestBody @Valid @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Profil à sauvegarder") ProfilWrite profil);

	/**
	 * Charge le détail d'un Profil.
	 * @param proId Id technique.
	 *
	 * @return Le détail du profil.
	 */
	@GetMapping(path = "{proId}")
	@PreAuthorize("hasRole('READ')")
	@Operation(description = "Charge le détail d'un Profil")
	ProfilRead getProfil(@PathVariable("proId") @Parameter(description = "Id technique") Integer proId);

	/**
	 * Liste tous les Profils.
	 *
	 * @return Profils matchant les critères.
	 */
	@GetMapping(path = "")
	@PreAuthorize("hasRole('READ')")
	@Operation(description = "Liste tous les Profils")
	List<ProfilItem> getProfils();

	/**
	 * Sauvegarde un Profil.
	 * @param proId Id technique.
	 * @param profil Profil à sauvegarder.
	 *
	 * @return Profil sauvegardé.
	 */
	@PutMapping(path = "{proId}")
	@PreAuthorize("hasRole('UPDATE')")
	@Operation(description = "Sauvegarde un Profil")
	ProfilRead updateProfil(@PathVariable("proId") @Parameter(description = "Id technique") Integer proId, @RequestBody @Valid @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Profil à sauvegarder") ProfilWrite profil);
}

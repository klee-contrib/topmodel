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
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	@PreAuthorize("hasRole('CREATE')")
	@PostMapping(path = "/")
	ProfilRead addProfil(@RequestBody @Valid ProfilWrite profil);

	/**
	 * Charge le détail d'un Profil.
	 * @param proId Id technique
	 * @return Le détail de l'Profil
	 */
	@PreAuthorize("hasRole('READ')")
	@GetMapping(path = "/{proId}")
	ProfilRead getProfil(@PathVariable("proId") Integer proId);

	/**
	 * Liste tous les Profils.
	 * @return Profils matchant les critères
	 */
	@PreAuthorize("hasRole('READ')")
	@GetMapping(path = "/")
	List<ProfilItem> getProfils();

	/**
	 * Sauvegarde un Profil.
	 * @param proId Id technique
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	@PreAuthorize("hasRole('UPDATE')")
	@PutMapping(path = "/{proId}")
	ProfilRead updateProfil(@PathVariable("proId") Integer proId, @RequestBody @Valid ProfilWrite profil);
}

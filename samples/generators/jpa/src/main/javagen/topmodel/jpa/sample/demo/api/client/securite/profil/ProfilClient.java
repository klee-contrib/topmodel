////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.api.client.securite.profil;

import java.util.List;

import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.service.annotation;
import org.springframework.web.service.annotation.GetExchange;
import org.springframework.web.service.annotation.PostExchange;
import org.springframework.web.service.annotation.PutExchange;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;

@HttpExchange("api/profils")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ProfilClient {


	/**
	 * Ajoute un Profil.
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	@PreAuthorize("hasRole('CREATE')")
	@PostExchange("/")
	ProfilRead addProfil(@RequestBody @Valid ProfilWrite profil);

	/**
	 * Charge le détail d'un Profil.
	 * @param proId Id technique
	 * @return Le détail de l'Profil
	 */
	@PreAuthorize("hasRole('READ')")
	@GetExchange("/{proId}")
	ProfilRead getProfil(@PathVariable("proId") Integer proId);

	/**
	 * Liste tous les Profils.
	 * @return Profils matchant les critères
	 */
	@PreAuthorize("hasRole('READ')")
	@GetExchange("/")
	List<ProfilItem> getProfils();

	/**
	 * Sauvegarde un Profil.
	 * @param proId Id technique
	 * @param profil Profil à sauvegarder
	 * @return Profil sauvegardé
	 */
	@PreAuthorize("hasRole('UPDATE')")
	@PutExchange("/{proId}")
	ProfilRead updateProfil(@PathVariable("proId") Integer proId, @RequestBody @Valid ProfilWrite profil);
}

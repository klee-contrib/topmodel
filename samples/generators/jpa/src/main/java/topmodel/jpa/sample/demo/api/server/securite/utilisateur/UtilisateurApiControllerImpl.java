package topmodel.jpa.sample.demo.api.server.securite.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.RestController;

import jakarta.validation.Valid;
import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.dtos.utilisateur.interfaces.IUtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur.Values;
import topmodel.jpa.sample.demo.services.UtilisateurService;

@RestController
public class UtilisateurApiControllerImpl implements UtilisateurApiController {

	private final UtilisateurService utilisateurService;

	public UtilisateurApiControllerImpl(UtilisateurService utilisateurService) {
		this.utilisateurService = utilisateurService;
	}

	@Override
	public UtilisateurDto find(Long utilisateurId) {
		return utilisateurService.find(utilisateurId);
	}

	@Override
	public UtilisateurDto save(@Valid UtilisateurDto utilisateur) {
		return utilisateurService.save(utilisateur);
	}

	@Override
	public List<IUtilisateurDto> findAllByType(Values typeUtilisateurCode) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Page<IUtilisateurDto> search(Long utiId, Long age, Long profilId, String email, String nom,
			Values typeUtilisateurCode, LocalDate dateCreation, LocalDateTime dateModification) {
		// TODO Auto-generated method stub
		return null;
	}

}

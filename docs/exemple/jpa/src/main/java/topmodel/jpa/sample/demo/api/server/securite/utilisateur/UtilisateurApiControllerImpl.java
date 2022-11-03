package topmodel.jpa.sample.demo.api.server.securite.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.RestController;

import jakarta.validation.Valid;
import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
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
	public List<UtilisateurDto> findAllByType(Values typeUtilisateurCode) {
		return utilisateurService.findAllByType(typeUtilisateurCode);
	}

	@Override
	public UtilisateurDto save(@Valid UtilisateurDto utilisateur) {
		return utilisateurService.save(utilisateur);
	}

	@Override
	public Page<UtilisateurDto> search(LocalDate dateCreation, LocalDateTime dateModification, Long utilisateurId,
			Long age, Long profilId, String email, Values typeUtilisateurCode) {
		return utilisateurService.findAll();
	}

}

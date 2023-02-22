package topmodel.jpa.sample.demo.services;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import jakarta.transaction.Transactional;
import jakarta.validation.Valid;
import topmodel.jpa.sample.demo.daos.utilisateur.UtilisateurDAO;
import topmodel.jpa.sample.demo.dtos.utilisateur.IUtilisateur;
import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;

/**
 * @author gderuette
 *
 */
@Service
public class UtilisateurService {
	private final UtilisateurDAO utilisateurDAO;

	public UtilisateurService(UtilisateurDAO utilisateurDAO) {
		this.utilisateurDAO = utilisateurDAO;
	}

	/**
	 * @param typeUtilisateurCode
	 * @return
	 */
	public List<IUtilisateur> findAllByType(TypeUtilisateur.Values typeUtilisateurCode) {
		return utilisateurDAO.findAllByTypeUtilisateur_Code(typeUtilisateurCode);
	}

	/**
	 * @param utilisateurId
	 * @return
	 */
	public UtilisateurDto find(Long utilisateurId) {
		return new UtilisateurDto(utilisateurDAO.findById(utilisateurId).orElseThrow());
	}

	/**
	 * @param utilisateurDto
	 * @return
	 */
	@Transactional
	public UtilisateurDto save(@Valid UtilisateurDto utilisateurDto) {
		var utilisateur = utilisateurDAO.findById(utilisateurDto.getId()).orElse(null);
		utilisateurDto.toUtilisateur(utilisateur);
		if (utilisateur != null) {
			utilisateurDAO.save(utilisateur);
		}

		return new UtilisateurDto(utilisateur);
	}

	/**
	 * @return
	 */
	public Page<UtilisateurDto> findAll() {
		return utilisateurDAO.findAll(Pageable.unpaged()).map(UtilisateurDto::new);
	}

}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.utilisateur;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur.Values;
import topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur;

public interface UtilisateurDAO extends JpaRepository<Utilisateur, Long> {

	List<UtilisateurDto> findAllByTypeUtilisateur_Code(Values typeUtilisateurCode);
}

////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.securite.profil;

import org.springframework.data.repository.CrudRepository;

import topmodel.jpa.sample.demo.entities.securite.profil.Droit;
import topmodel.jpa.sample.demo.enums.securite.profil.DroitCode;


public interface DroitDAO extends CrudRepository<Droit, DroitCode> {

}

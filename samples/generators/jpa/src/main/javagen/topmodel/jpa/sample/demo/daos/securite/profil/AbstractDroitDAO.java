////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.securite.profil;

import org.springframework.data.repository.NoRepositoryBean;

import topmodel.jpa.sample.demo.daos.repository.CustomCrudRepository;
import topmodel.jpa.sample.demo.entities.securite.profil.Droit;
import topmodel.jpa.sample.demo.enums.securite.profil.DroitCode;


@NoRepositoryBean
interface AbstractDroitDAO extends CustomCrudRepository<Droit, DroitCode> {

}

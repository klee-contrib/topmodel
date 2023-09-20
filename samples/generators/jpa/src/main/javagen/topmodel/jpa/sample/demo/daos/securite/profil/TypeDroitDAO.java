////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.securite.profil;

import org.springframework.data.repository.CrudRepository;

import topmodel.jpa.sample.demo.entities.securite.profil.TypeDroit;
import topmodel.jpa.sample.demo.enums.securite.profil.TypeDroitCode;


public interface TypeDroitDAO extends CrudRepository<TypeDroit, TypeDroitCode> {

}

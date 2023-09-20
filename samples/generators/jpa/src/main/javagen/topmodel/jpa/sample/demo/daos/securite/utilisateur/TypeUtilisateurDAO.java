////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.securite.utilisateur;

import org.springframework.data.repository.CrudRepository;

import topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;


public interface TypeUtilisateurDAO extends CrudRepository<TypeUtilisateur, TypeUtilisateurCode> {

}

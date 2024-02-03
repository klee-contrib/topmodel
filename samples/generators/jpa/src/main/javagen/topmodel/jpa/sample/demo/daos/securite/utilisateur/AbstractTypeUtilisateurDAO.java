////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.daos.securite.utilisateur;

import org.springframework.data.repository.NoRepositoryBean;

import topmodel.jpa.sample.demo.daos.repository.CustomCrudRepository;
import topmodel.jpa.sample.demo.entities.securite.utilisateur.TypeUtilisateur;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;


@NoRepositoryBean
interface AbstractTypeUtilisateurDAO extends CustomCrudRepository<TypeUtilisateur, TypeUtilisateurCode> {

}

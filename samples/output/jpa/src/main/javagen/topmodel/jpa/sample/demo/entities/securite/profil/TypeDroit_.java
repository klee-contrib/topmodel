////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import topmodel.jpa.sample.demo.enums.securite.profil.TypeDroitCode;

@StaticMetamodel(TypeDroit.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class TypeDroit_ {

	public static volatile SingularAttribute<TypeDroit, TypeDroitCode> code;

	public static volatile SingularAttribute<TypeDroit, String> libelle;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";
}

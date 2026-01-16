////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_sequence_server.enums.restaurant.CategoriePlatCode;

@StaticMetamodel(CategoriePlat.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat_ {

	public static volatile SingularAttribute<CategoriePlat, CategoriePlatCode> code;

	public static volatile SingularAttribute<CategoriePlat, String> libelle;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";
}

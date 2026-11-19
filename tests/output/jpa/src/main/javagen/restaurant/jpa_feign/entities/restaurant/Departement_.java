////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_feign.enums.restaurant.RegionCode;

@StaticMetamodel(Departement.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Departement_ {

	public static volatile SingularAttribute<Departement, String> code;

	public static volatile SingularAttribute<Departement, String> libelle;

	public static volatile SingularAttribute<Departement, RegionCode> regionCode;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";

	public static final String REGION_CODE = "regionCode";
}

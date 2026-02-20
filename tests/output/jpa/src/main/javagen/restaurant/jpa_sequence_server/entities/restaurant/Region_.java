////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_sequence_server.enums.restaurant.RegionCode;

@StaticMetamodel(Region.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Region_ {

	public static volatile SingularAttribute<Region, RegionCode> code;

	public static volatile SingularAttribute<Region, String> libelle;

	public static volatile SingularAttribute<Region, String> nomResponsable;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";

	public static final String NOM_RESPONSABLE = "nomResponsable";
}

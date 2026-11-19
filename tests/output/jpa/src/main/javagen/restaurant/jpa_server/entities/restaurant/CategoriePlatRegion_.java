////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_server.enums.restaurant.RegionCode;

@StaticMetamodel(CategoriePlatRegion.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlatRegion_ {

	public static volatile SingularAttribute<CategoriePlatRegion, RegionCode> regionCode;

	public static volatile SingularAttribute<CategoriePlatRegion, CategoriePlat> categoriePlat;

	public static final String REGION_CODE = "regionCode";

	public static final String CATEGORIE_PLAT = "categoriePlat";
}

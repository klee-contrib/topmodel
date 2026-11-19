////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.entities.restaurant;

import java.math.BigDecimal;

import jakarta.annotation.Generated;
import jakarta.persistence.metamodel.SingularAttribute;
import jakarta.persistence.metamodel.StaticMetamodel;

import restaurant.jpa_feign.enums.restaurant.CategoriePlatCode;

@StaticMetamodel(CategoriePlat.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CategoriePlat_ {

	public static volatile SingularAttribute<CategoriePlat, CategoriePlatCode> code;

	public static volatile SingularAttribute<CategoriePlat, String> libelle;

	public static volatile SingularAttribute<CategoriePlat, Integer> ordre;

	public static volatile SingularAttribute<CategoriePlat, BigDecimal> prixMoyen;

	public static final String CODE = "code";

	public static final String LIBELLE = "libelle";

	public static final String ORDRE = "ordre";

	public static final String PRIX_MOYEN = "prixMoyen";
}

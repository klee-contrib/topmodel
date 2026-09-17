////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

import restaurant.jpa_jdbc_resttemplate.enums.restaurant.TypeFactureCode;

/**
 * Type de facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record TypeFacture(
	@Column("tfa_code") @Size(max = 10) String code,
	@Column("tfa_libelle") @Size(max = 100) String libelle
) {

	public static final TypeFacture ELE = new TypeFacture(TypeFactureCode.Ele, "restaurant.typeFacture.values.ELE");

	public static final TypeFacture PHY = new TypeFacture(TypeFactureCode.Phy, "restaurant.typeFacture.values.PHY");

	/**
	 * Liste de toutes les valeurs de l'énumération TypeFacture.
	 */
	public static final List<TypeFacture> VALUES = List.of(ELE, PHY);

	/**
	 * Retourne la valeur de l'énumération pour la clé spécifiée.
	 * @param code La clé de l'énumération pour laquelle obtenir la valeur.
	 *
	 * @return La valeur de l'énumération correspondant à la clé 'Code'.
	 */
	public static TypeFacture getValue(String code) {
		return switch (code) {
			case TypeFactureCode.Ele -> ELE;
			case TypeFactureCode.Phy -> PHY;
			default -> throw new IllegalArgumentException("Clé d'énumération inconnue : " + code);
		};
	}
}
